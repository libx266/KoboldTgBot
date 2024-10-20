﻿using KoboldTgBot.Database;
using KoboldTgBot.Errors;
using KoboldTgBot.Extensions.Database;
using KoboldTgBot.Extensions.Utils;
using KoboldTgBot.TgBot.Actions.Callbacks;
using KoboldTgBot.TgBot.Actions.Commands;
using KoboldTgBot.Utils;
using Newtonsoft.Json;
using System.Text;
using Telegram.Bot.Types;

namespace KoboldTgBot.Neuro
{
    internal static class GenerationApi
    {
        private static async Task<string?> SendRequestLocal(string promptText, long userId, string[] stop, int maxLength, float temperature, float topPSampling, float repetitionPenalty)
        {
            using var http = new HttpClient();

            http.Timeout = TimeSpan.FromHours(1);

            var endpoint = ConfigurationManager.NeuroApiEndpoint + "completions";

            var data = new
            {
                max_tokens = maxLength,
                prompt = promptText,
                repetition_penalty = repetitionPenalty,
                temperature = temperature,
                top_p = topPSampling,
                stop = stop
            };

            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await http.PostAsync(endpoint, content);

            response.EnsureSuccessStatusCode();

            var answer = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());

            string? text = answer?.choices[0].text;

            var gen = new DbGeneration
            {
                Answer = text ?? string.Empty,
                Prompt = promptText,
                Model = ConfigurationManager.ModelName,
                UserId = userId,
                IsLocal = true,
                Temperature = temperature,
                RepetitionPenalty = repetitionPenalty,
                TopPSampling = topPSampling
            };

            using (var db = new DataContext())
            {
                await db.Generations.AddAsync(gen);
                await db.SaveChangesAsync();
            }

            return text;
        }

        private const string ProxiAPIEndpoint = "https://api.proxyapi.ru/openai/v1/chat/completions";

        private static async Task<string?> SendRequestGpt4o(string promptText, long userId, string[] stop, int maxLength, float temperature)
        {
            using var http = new HttpClient();

            http.Timeout = TimeSpan.FromMinutes(3);

            var data = new
            {
                model = "gpt-4o",
                messages = new[]
                {
                    new
                    {
                        role = "user",
                        content = promptText
                    }
                },
                temperature = temperature,
                max_tokens = maxLength,
                stop = stop
            };

            var request = new HttpRequestMessage();

            request.RequestUri = new Uri(ProxiAPIEndpoint);
            request.Method = HttpMethod.Post;
            request.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", ConfigurationManager.ProxyAPIToken);

            var response = await http.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var answer = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());

            string? text = answer?.choices[0].message.content;

            using var db = new DataContext();

            var gen = new DbGeneration
            {
                Answer = text ?? string.Empty,
                Prompt = promptText,
                PromptTokens = answer!.usage.prompt_tokens,
                CompletionTokens = answer!.usage.completion_tokens,
                GenerationId = answer!.id,
                Model = "gpt-4o",
                UserId = userId,
                IsLocal = false,
                Temperature = temperature
            };

            await db.Generations.AddAsync(gen);

            var balance = await db.UpdateCabinetBalanceAsync(userId, gen.PromptTokens.Value, gen.CompletionTokens.Value);

            await db.SaveChangesAsync();

            return balance > 0 ? text : "Ваш баланс отрицательный, модель переключена на " + CallbackSelectModel.LLama3;
        }

        internal static async Task<string> GenerateAsync(PromptDto prompt, long userId, int attempts = 20)
        {
            string promptText = LLMProcessingHelper.RemoveEmojis(prompt.Prompt);
            try
            {
                if (!Convert.ToBoolean(attempts))
                {
                    return "._.";
                }

                var stop = new[]
                {
                    "###",
                    "assistant",
                    LLMProcessingHelper.RemoveEmojis(prompt.BotName) + ':',
                    LLMProcessingHelper.RemoveEmojis(prompt.UserName) + ':'
                };

                using var db = new DataContext();

                string? text = await new[]
                {
                    () => SendRequestLocal(promptText, userId, stop, ConfigurationManager.MaxGenerationLength, ConfigurationManager.Temperature, ConfigurationManager.TopPSampling, ConfigurationManager.RepetitionPenalty),
                    () => SendRequestGpt4o(promptText, userId, stop, ConfigurationManager.MaxGenerationLength, ConfigurationManager.Temperature)
                }
                [Convert.ToInt32(await db.IsGpt4oEnable(userId))]();

                if (string.IsNullOrEmpty(text = LLMProcessingHelper.Filter(text, stop)))
                {
                    throw new LLMEmptyAnswerException(promptText, ConfigurationManager.MaxGenerationLength, ConfigurationManager.Temperature, ConfigurationManager.TopPSampling, ConfigurationManager.RepetitionPenalty);
                }

                

                return LLMProcessingHelper.RemoveEmojis(text);
            }
            catch (Exception ex)
            {
                ex.Log();
                return await GenerateAsync(prompt, userId,  attempts - 1);
            }
        }
    }
}
