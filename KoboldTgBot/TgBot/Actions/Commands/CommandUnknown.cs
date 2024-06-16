using Telegram.Bot;
using Telegram.Bot.Types;

namespace KoboldTgBot.TgBot.Actions.Commands
{
    internal sealed class CommandUnknown : TgCommandBase
    {
        public CommandUnknown(ITelegramBotClient bot, Message message) : base(bot, message)
        {
        }

        protected override async Task WorkAsync() =>
           await _bot.SendTextMessageAsync(_message.Chat.Id, Properties.Resources.UnknownCommandMessage);
    }
}
