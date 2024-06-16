using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoboldTgBot.Utils
{
    internal static class AnswerValidation
    {
        private class AnswerValidator
        {
            private readonly string _answer;
            private readonly List<Func<string, bool>> _validators = new();

            internal AnswerValidator(string answer) => _answer = answer;

            internal AnswerValidator Check(Func<string, bool> predicate)
            {
                _validators.Add(predicate);
                return this;
            }

            internal bool Compile(bool mustBe) => mustBe ? _validators.All(p => p(_answer)) : !_validators.Any(p => p(_answer));
        }

        internal static bool Validate(string answer) => true;
            /*
            new AnswerValidator(answer)
            .Check(s => s.Contains("### Instruction:"))
            .Check(s => s.Contains("### Response:"))
            .Compile(false);
            */
            
    }
}
