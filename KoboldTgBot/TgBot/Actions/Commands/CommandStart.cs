using KoboldTgBot.TgBot.Objects;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace KoboldTgBot.TgBot.Actions.Commands
{
    internal sealed class CommandStart : TgAction<MessageHandler>
    {
        public const string Name = "/start";

        public CommandStart(ITelegramBotClient bot, MessageHandler entity) : base(bot, entity)
        {
        }

        protected override async Task WorkAsync()
        {
            var keyboard = new ReplyKeyboardMarkup(new KeyboardButton[]
            {
                new KeyboardButton(CommandClear.Name),
                new KeyboardButton(CommandRegen.Name),
                new KeyboardButton(CommandEdit.Name),
                new KeyboardButton(CommandMultiMessage.Name),
                new KeyboardButton(CommandMore.Name),
                new KeyboardButton(CommandRole.Name)
            });

            keyboard.ResizeKeyboard = true;

            await _bot.SendTextMessageAsync(ChatId, Properties.Resources.StartMessage, replyMarkup: keyboard);
        }
            
    }
}
