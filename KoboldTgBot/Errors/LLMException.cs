namespace KoboldTgBot.Errors
{
    public abstract class LLMException : Exception
    {
        public string Propmpt { get; }

        public int MaxLength { get; }

        public float Temperature { get; }

        public float TopPSampling { get; }

        public float RepetitionPenalty { get; }

        public LLMException(string message, string propmpt, int maxLength, float temperature, float topPSampling, float repetitionPenalty) : base(message)
        {
            Propmpt = propmpt;
            MaxLength = maxLength;
            Temperature = temperature;
            TopPSampling = topPSampling;
            RepetitionPenalty = repetitionPenalty;
        }
    }
}
