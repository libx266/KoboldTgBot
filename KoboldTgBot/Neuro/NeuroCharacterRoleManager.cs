using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoboldTgBot.Neuro
{
    internal sealed class NeuroCharacterRoleManager
    {
        private readonly Dictionary<long, string> _roles = new();

        internal const string Science = "science";
        internal const string ChitChat = "chitchat";
        internal const string Imperial = "imperial";

        internal void SetRole(long chatId, string role)
        {
            if (!new[] {Science, ChitChat, Imperial}.Contains(role))
            {
                throw new ArgumentException("Invalid role");
            }
            _roles[chatId] = role;
        }

        internal string GetPrompt(long chatId)
        {
            if (_roles.TryGetValue(chatId, out var role))
            {
                return role switch
                {
                    Science => Properties.Resources.NeuroCharacterSciencePrompt,
                    ChitChat => Properties.Resources.NeuroCharacterChitChatPrompt,
                    Imperial => Properties.Resources.NeuroCharacterImperialPrompt,
                    _ => Properties.Resources.NeuroCharacterSciencePrompt
                };
            }

            return Properties.Resources.NeuroCharacterSciencePrompt;
        }
    }
}
