using System.Text.RegularExpressions;

namespace KoboldTgBot.Utils
{
    internal static class AnswerFilering
    {
        private static string ReplaceBadSymbols(this string asnwer) =>
            asnwer.Replace("`", "");

        private static string ReplaceGender(this string answer)
        {
            string pattern = @"(?<=\s)Рад(?=\s|\s\s)";
            string replacement = "Рада";

            answer = Regex.Replace(answer, pattern, replacement);

            pattern = @"(?<=\s)рад(?=\s|\s\s)";
            replacement = "рада";

            return Regex.Replace(answer, pattern, replacement);
        }

        internal static string Process(string asnwer) => asnwer
          //  .ReplaceBadSymbols()
            .ReplaceGender();

            

    }
}
