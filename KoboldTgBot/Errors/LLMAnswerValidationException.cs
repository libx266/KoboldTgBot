﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoboldTgBot.Errors
{
    public class LLMAnswerValidationException : LLMException
    {
        public LLMAnswerValidationException(string propmpt, ushort maxLength, ushort contextSize, float temperature, float topPSampling, float repetitionPenalty) : base("LLM asnwer not valid", propmpt, maxLength, contextSize, temperature, topPSampling, repetitionPenalty)
        {
        }
    }
}
