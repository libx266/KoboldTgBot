using KoboldTgBot.Database;
using KoboldTgBot.Errors;
using KoboldTgBot.Extensions.Database;
using KoboldTgBot.Extensions.Utils;
using KoboldTgBot.TgBot.Actions.Callbacks;
using KoboldTgBot.TgBot.Actions.Commands;
using KoboldTgBot.Utils;
using Microsoft.EntityFrameworkCore;
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


        private static async Task<string?> SendRequestGpt4o(string promptText, long userId, string[] stop, int maxLength)
        {
            using var http = new HttpClient();

            http.Timeout = TimeSpan.FromMinutes(3);

            using var db = new DataContext();

            var cab = await db.GetCabinetAsync(userId);
            var model = await db.Models.Where(m => m.ID == cab!.ModelType).Select(m => new { m.Name, m.Endpoint }).FirstAsync();


            object? data = null;

            bool openai = model.Endpoint.Contains("openai/v1/chat/completions");
            bool anthropic = model.Endpoint.Contains("anthropic/v1/messages");
            bool google = model.Endpoint.Contains("google/v1/models/");

            if (openai)
            {
                data = new
                {
                    model = model.Name,
                    messages = new[]
                    {
                        new
                        {
                            role = "user",
                            content = promptText
                        }
                    },
                    temperature = 0.8f,
                    max_tokens = maxLength,
                    stop = stop
                };
            }
            else if (anthropic)
            {
                data = new
                {
                    model = model.Name,
                    messages = new[]
                    {
                        new
                        {
                            role = "user",
                            content = promptText
                        }
                    },
                    max_tokens = maxLength,
                };
            }
            else if (google)
            {
                data = new
                {
                    contents = new[]
                    {
                        new
                        {
                            role = "user",
                            parts = new []
                            {
                                new
                                {
                                    text = promptText
                                }
                            }
                        }
                    }
                };
            }

            var request = new HttpRequestMessage();

            request.RequestUri = new Uri(model.Endpoint);
            request.Method = HttpMethod.Post;
            request.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            if (anthropic)
            {
                /*
                string date = model.Name.Split('-').Last();

                string year = String.Join(null, date.Take(4));
                string month = String.Join(null, date.Skip(4).Take(2));
                string day = String.Join(null, date.Skip(6).Take(2));
                */

                request.Headers.Add("Anthropic-Version", "2023-06-01");
            }
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", ConfigurationManager.ProxyAPIToken);

            var response = await http.SendAsync(request);

            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            var answer = JsonConvert.DeserializeObject<dynamic>(json);

            string? text = null;
            int? promptTokens = null;
            int? completionTokens = null;

            try
            {
                if (openai)
                {
                    promptTokens = answer?.usage.prompt_tokens;
                    completionTokens = answer?.usage.completion_tokens;
                    text = answer?.choices[0].message.content;
                }
                else if (google)
                {
                    promptTokens = answer?.usageMetadata.promptTokenCount;
                    completionTokens = answer?.usageMetadata.candidatesTokenCount;
                    text = answer?.candidates[0].content.parts[0].text;
                }
                else if (anthropic)
                {
                    promptTokens = answer?.usage.input_tokens;
                    completionTokens = answer?.usage.output_tokens;
                    text = answer?.content[0].text;
                }
            }
            catch
            {
                Console.WriteLine(json);
                promptTokens ??= Convert.ToInt32(promptText.Length / ConfigurationManager.AverageSymbolsPerToken);
                completionTokens ??= 0;
            }

            var gen = new DbGeneration
            {
                Answer = text ?? string.Empty,
                Prompt = promptText,
                PromptTokens = promptTokens,
                CompletionTokens = completionTokens,
                GenerationId = answer?.id,
                Model = model.Name,
                UserId = userId,
                IsLocal = false,
                Temperature = 0.8f
            };

            await db.Generations.AddAsync(gen);

            var balance = await db.UpdateCabinetBalanceAsync(userId, gen.PromptTokens!.Value, gen.CompletionTokens!.Value);

            await db.SaveChangesAsync();

            return balance > 0 ? text : "Ваш баланс отрицательный, модель переключена на " + ConfigurationManager.ModelName;
        }

        internal static async Task<string> GenerateAsync(PromptDto prompt, long userId, int attempts = 20)
        {
            string promptText = LLMProcessingHelper.RemoveEmojis(prompt.Prompt);
            try
            {
                if (!Convert.ToBoolean(attempts))
                {
                    return "System: Не удалось сгенерировать ответ. Вероятно, выбранная модель в настоящий момент не доступна.";
                }

                var stop = new[]
                {
                    "assistant",
                    LLMProcessingHelper.RemoveEmojis(prompt.UserName) + ':'
                };

                using var db = new DataContext();

                string? text = await new[]
                {
                    () => SendRequestLocal(promptText, userId, stop, ConfigurationManager.MaxGenerationLength, ConfigurationManager.Temperature, ConfigurationManager.TopPSampling, ConfigurationManager.RepetitionPenalty),
                    () => SendRequestGpt4o(promptText, userId, stop, ConfigurationManager.MaxGenerationLength)
                }
                [Convert.ToInt32(await db.IsExternalModelEnable(userId))]();

                if (string.IsNullOrEmpty(text = LLMProcessingHelper.Filter(text, stop, LLMProcessingHelper.RemoveEmojis(prompt.BotName) + ':')))
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
