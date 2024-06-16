using KoboldTgBot.Neuro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace KoboldTgBot.TgBot.Actions.Callbacks
{
    internal sealed class CallbackRole : TgCallbackBase
    {
        public CallbackRole(ITelegramBotClient bot, CallbackQuery callback) : base(bot, callback)
        {
        }

        protected override async Task WorkAsync()
        {
            var roles = _data as NeuroCharacterRoleManager;

            roles!.SetRole(_callback.Message.Chat.Id, Data);

            await _bot.SendTextMessageAsync(_callback.Message.Chat.Id, Data switch
            {
                NeuroCharacterRoleManager.Science => "Служу прогрессу!",
                NeuroCharacterRoleManager.ChitChat => "Ня!",
                NeuroCharacterRoleManager.Imperial => "Слава Империи!"
            });
            await _bot.DeleteMessageAsync(_callback.Message.Chat.Id, _callback.Message.MessageId);
        }
    }
}
