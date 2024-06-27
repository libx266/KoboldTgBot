using KoboldTgBot.Database;
using KoboldTgBot.Extensions.Utils;
using KoboldTgBot.TgBot.Actions.Callbacks;
using KoboldTgBot.TgBot.Objects;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace KoboldTgBot.TgBot.Actions
{
    internal abstract class TgWithBalance<T> : TgAction<T> where T : ActionEntity
    {
        internal TgWithBalance(ITelegramBotClient bot, T entity) : base(bot, entity)
        {
        }

        protected async Task SendCabInfoAsync(DataContext db, DbCabinet cab, bool edit = false)
        {
            bool gpt4oEnable = !cab.IsGpt4o && cab.Balance > 0;

            string model = gpt4oEnable ? CallbackSelectModel.Gpt4o : CallbackSelectModel.LLama3;

            var keyboard = new InlineKeyboardMarkup(TgHelper.MakeInlineButton<CallbackSelectModel>("Переключить на " + model, model));

            if (edit)
            {
                await _bot.EditMessageTextAsync(ChatId, MessageId, "Ваш баланс:  " + cab.Balance, replyMarkup: keyboard);
            }
            else
            {
                await _bot.SendTextMessageAsync(ChatId, "Ваш баланс:  " + cab.Balance, replyMarkup: keyboard);
            }
        }
    }
}
