using KoboldTgBot.Errors;
using KoboldTgBot.Extensions.Utils;
using KoboldTgBot.Utils;
using Newtonsoft.Json;
using System.Text;

namespace KoboldTgBot.Neuro
{
    internal static class GenerationApi
    {
        internal static async Task<string> GenerateAsync(PromptDto prompt, ushort maxLength = 1024, float temperature = 0.8f, float topPSampling = 0.925f, float repetitionPenalty = 1.175f, int attempts = 20)
        {
            string promptText = PromptHelper.RemoveEmojis(prompt.Prompt);
            try
            {
                if (!Convert.ToBoolean(attempts))
                {
                    return "._.";
                }

                using var http = new HttpClient();

                http.Timeout = TimeSpan.FromMinutes(20);

                var endpoint = ConfigurationManager.GetNeuroApiEndpoint() + "completions";

                var stop = new[] { "###", "assistant", prompt.BotName + ':', prompt.UserName + ':' };

                var request = new
                {
                    max_tokens = maxLength,
                    prompt = promptText,
                    repetition_penalty = repetitionPenalty,
                    temperature = temperature,
                    top_p = topPSampling,
                    stop = stop 
                };

                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await http.PostAsync(endpoint, content);

                response.EnsureSuccessStatusCode();

                var data = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());

                string? text = data?.choices[0].text;

                if (string.IsNullOrEmpty(text = PromptHelper.Filter(text, stop)))
                {
                    throw new LLMEmptyAnswerException(promptText, maxLength, temperature, topPSampling, repetitionPenalty);
                }

                return text;
            }
            catch (Exception ex)
            {
                ex.Log();
                return await GenerateAsync(prompt, maxLength, temperature, topPSampling, repetitionPenalty, attempts - 1);
            }
        }
    }
}
