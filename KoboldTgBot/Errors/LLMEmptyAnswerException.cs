namespace KoboldTgBot.Errors
{
    public sealed class LLMEmptyAnswerException : LLMException
    {
        public LLMEmptyAnswerException(string propmpt, int maxLength, float temperature, float topPSampling, float repetitionPenalty) : base("Empty asnwer from LLM", propmpt, maxLength, temperature, topPSampling, repetitionPenalty)
        {
        }
    }
}
