using KoboldTgBot.Errors;
using KoboldTgBot.Utils;
using Newtonsoft.Json;
using System.Text;

namespace KoboldTgBot.Neuro
{
    internal static class GenerationApi
    {
        internal static async Task<string> GenerateAsync(string prompt, ushort maxLength = 1024, ushort maxContextLength = 8192, float temperature = 0.8f, float topPSampling = 0.925f, float repetitionPenalty = 1.175f, int attempts = 10)
        {
            try
            {
                if (attempts < 1)
                {
                    return "._.";
                }

                using var http = new HttpClient();

                http.Timeout = TimeSpan.FromMinutes(20);

                var endpoint = ConfigurationManager.GetNeuroApiEndpoint() + "generate";

                var request = new
                {
                    max_context_length = maxContextLength,
                    max_length = maxLength,
                    prompt = prompt,
                    quiet = false,
                    rep_pen = repetitionPenalty,
                    temperature = temperature,
                    top_p = topPSampling
                };

                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await http.PostAsync(endpoint, content);

                response.EnsureSuccessStatusCode();

                var data = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());

                string? text = data.results[0].text;

                if (string.IsNullOrEmpty(text))
                {
                    throw new LLMEmptyAnswerException(prompt, maxLength, maxContextLength, temperature, topPSampling, repetitionPenalty);
                }

                if (!AnswerValidation.Validate(text))
                {
                    throw new LLMAnswerValidationException(prompt, maxLength, maxContextLength, temperature, topPSampling, repetitionPenalty);
                }

                return AnswerFilering.Process(text);
            }
            catch (Exception ex)
            {
                ex.Log();
                return await GenerateAsync(prompt, maxLength, maxContextLength, temperature, topPSampling, repetitionPenalty, attempts - 1);
            }
        }
    }
}
