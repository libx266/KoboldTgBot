using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoboldTgBot.Errors
{
    public class LLMEmptyAnswerException : Exception
    {
        public string Propmpt { get; }

        public ushort MaxLength { get; }

        public ushort ContextSize { get; }

        public float Temperature { get; }

        public float TopPSampling { get; }

        public float RepetitionPenalty { get; }

        public LLMEmptyAnswerException(string propmpt, ushort maxLength, ushort contextSize, float temperature, float topPSampling, float repetitionPenalty) : base("Empty asnwer from LLM")
        {
            Propmpt = propmpt;
            MaxLength = maxLength;
            ContextSize = contextSize;
            Temperature = temperature;
            TopPSampling = topPSampling;
            RepetitionPenalty = repetitionPenalty;
        }
    }
}
