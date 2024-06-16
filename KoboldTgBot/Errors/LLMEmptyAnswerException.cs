namespace KoboldTgBot.Errors
{
    public sealed class LLMEmptyAnswerException : LLMException
    {
        public LLMEmptyAnswerException(string propmpt, ushort maxLength, ushort contextSize, float temperature, float topPSampling, float repetitionPenalty) : base("Empty asnwer from LLM", propmpt, maxLength, contextSize, temperature, topPSampling, repetitionPenalty)
        {
        }
    }
}
