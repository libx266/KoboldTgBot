using KoboldTgBot.TgBot.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace KoboldTgBot.TgBot.Actions.Commands
{
    internal sealed class CommandGpt4o : TgAction<MessageHandler>
    {
        public CommandGpt4o(ITelegramBotClient bot, MessageHandler entity) : base(bot, entity)
        {
        }

        protected override async Task WorkAsync()
        {
            bool gpt4o = !_gpt4o;

            await _bot.SendTextMessageAsync(ChatId, "Переключено на " + new[] {"LLama 3 70B", "gpt4o" }[Convert.ToInt32(gpt4o)]);

            (_data as Action<bool>)!(gpt4o);
        }
    }
}
