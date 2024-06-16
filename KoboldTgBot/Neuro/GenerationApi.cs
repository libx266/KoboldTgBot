using KoboldTgBot.Errors;
using KoboldTgBot.Utils;
using Newtonsoft.Json;
using System.Text;

namespace KoboldTgBot.Neuro
{
    internal static class GenerationApi
    {
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
                    stop = new[] { "### Instruction:", "### Response:" } 
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
