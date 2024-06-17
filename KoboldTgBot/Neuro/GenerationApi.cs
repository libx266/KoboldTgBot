using KoboldTgBot.Database;
using KoboldTgBot.Errors;
using KoboldTgBot.Utils;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using Telegram.Bot.Types;

namespace KoboldTgBot.Neuro
{
    internal static class GenerationApi
    {
        internal static async Task<string> ConstructPropmptAsync(this DataContext db, long chatId, User? user)
        {
            var messages = await db.Messages.Where(m => m.ChatId == chatId && m.InMemory).OrderByDescending(m => m.ID).Take(byte.MaxValue).ToListAsync();

            string senderName;

            if (string.IsNullOrWhiteSpace(senderName = user!.FirstName ?? "" + ' ' + user.LastName ?? ""))
            {
                senderName = user.Username ?? "Anonymous";
            }

            var actualFilteredMessages =
            (
                from m in messages
                group m by m.TgId into mg
                let m2 = mg.MaxBy(m => m.ID)
                orderby m2.ID descending
                select new
                {
                    Text = m2.Text,
                    Sender = m2.UserId
                }
            );

            var dialog = new List<string>();
            int count = 0;

            foreach (var m in actualFilteredMessages)
            {
                string row = $"{new[] { senderName, Properties.Resources.BotName }[Convert.ToInt32(m.Sender == -1)]}:  {m.Text}";

                count += row.Length;

                if (count > 21504)
                {
                    break;
                }

                dialog.Add(row);
            }

            dialog.Reverse();

            var role =
            (
                from cr in db.CurrentRoles.Where(cr => cr.ChatId == chatId).DefaultIfEmpty()
                from r in db.Roles
                where r.ID == (cr == default ? 1 : cr.RoleId)
                select r.Description
            );

            return String.Format(Properties.Resources.NeuroCharacterPrompt, await role.FirstOrDefaultAsync() ?? Properties.Resources.NeuroCharacterSciencePrompt, String.Join("\n", dialog.Append($"{Properties.Resources.BotName}:  ")));
        }

        internal static async Task<string> GenerateAsync(string prompt, ushort maxLength = 1024, float temperature = 0.8f, float topPSampling = 0.925f, float repetitionPenalty = 1.175f, int attempts = 20)
        {
            try
            {
                if (!Convert.ToBoolean(attempts))
                {
                    return "._.";
                }

                using var http = new HttpClient();

                http.Timeout = TimeSpan.FromMinutes(20);

                var endpoint = ConfigurationManager.GetNeuroApiEndpoint() + "completions";


                prompt = Extensions.RemoveEmojis(prompt);
                var request = new
                {
                    max_tokens = maxLength,
                    prompt = prompt,
                    repetition_penalty = repetitionPenalty,
                    temperature = temperature,
                    top_p = topPSampling,
                    stop = new[] { "### Instruction:", "### Response:", "assistant" } 
                };

                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await http.PostAsync(endpoint, content);

                response.EnsureSuccessStatusCode();

                var data = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());

                string? text = data?.choices[0].text;

                if (string.IsNullOrEmpty(text))
                {
                    throw new LLMEmptyAnswerException(prompt, maxLength, temperature, topPSampling, repetitionPenalty);
                }

                if (!AnswerValidation.Validate(text))
                {
                    throw new LLMAnswerValidationException(prompt, maxLength, temperature, topPSampling, repetitionPenalty);
                }

                return AnswerFilering.Process(text);
            }
            catch (Exception ex)
            {
                ex.Log();
                return await GenerateAsync(prompt, maxLength, temperature, topPSampling, repetitionPenalty, attempts - 1);
            }
        }
    }
}
