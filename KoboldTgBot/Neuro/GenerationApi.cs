using KoboldTgBot.Errors;
using KoboldTgBot.Extensions.Utils;
using KoboldTgBot.Utils;
using Newtonsoft.Json;
using System.Text;

namespace KoboldTgBot.Neuro
{
    internal static class GenerationApi
    {
        private static async Task<string?> SendRequestLocal(string promptText, string[] stop, ushort maxLength, float temperature, float topPSampling, float repetitionPenalty)
        {
            using var http = new HttpClient();

            http.Timeout = TimeSpan.FromMinutes(20);

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

            return answer?.choices[0].text;
        }

        private const string ProxiAPIEndpoint = "https://api.proxyapi.ru/openai/v1/chat/completions";

        private static async Task<string?> SendRequestGpt4o(string promptText, string[] stop, ushort maxLength, float temperature)
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
                temperature = temperature
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

            return answer?.choices[0].message.content;
        }

        internal static async Task<string> GenerateAsync(PromptDto prompt, bool gpt4o, ushort maxLength = 1024, float temperature = 0.8f, float topPSampling = 0.925f, float repetitionPenalty = 1.175f, int attempts = 20)
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

                string? text = await new[]
                {
                    () => SendRequestLocal(promptText, stop, maxLength, temperature, topPSampling, repetitionPenalty),
                    () => SendRequestGpt4o(promptText, stop, maxLength, temperature)
                }
                [Convert.ToInt32(gpt4o)]();

                if (string.IsNullOrEmpty(text = LLMProcessingHelper.Filter(text, stop)))
                {
                    throw new LLMEmptyAnswerException(promptText, maxLength, temperature, topPSampling, repetitionPenalty);
                }

                return text;
            }
            catch (Exception ex)
            {
                ex.Log();
                return await GenerateAsync(prompt, gpt4o, maxLength, temperature, topPSampling, repetitionPenalty, attempts - 1);
            }
        }
    }
}
