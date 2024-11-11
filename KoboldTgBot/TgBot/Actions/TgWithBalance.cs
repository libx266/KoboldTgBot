using KoboldTgBot.Database;
using KoboldTgBot.Extensions.Utils;
using KoboldTgBot.TgBot.Actions.Callbacks;
using KoboldTgBot.TgBot.Objects;
using KoboldTgBot.Utils;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace KoboldTgBot.TgBot.Actions
{
    internal abstract class TgWithBalance<T> : TgAction<T> where T : ActionEntity
    {
        internal TgWithBalance(ITelegramBotClient bot, T entity) : base(bot, entity)
        {
        }

        private static string ModelNameBuilder(string name, decimal promptCost, decimal answerCost) =>
            $"{name} (P: {promptCost}₽, A: {answerCost}₽)";


        protected async Task SendCabInfoAsync(DataContext db, DbCabinet cab, bool edit = false)
        {
            IEnumerable<string> models = new[] { ModelNameBuilder(ConfigurationManager.ModelName, 0m, 0m) };

            if (cab.Balance > 0m)
            {
                models = models.Concat((await db.Models.OrderBy(m => m.ID).ToListAsync()).Select(m => ModelNameBuilder(m.Name, m.Prompt1kTokensCostRub, m.Answer1kTokensCostRub)));
            }

            var buttons = models.Select(model => new[] { TgHelper.MakeInlineButton<CallbackSelectModel>(model, model.Split(' ').First()) });

            var keyboard = new InlineKeyboardMarkup(buttons.ToArray());

            var model = await db.Models.Where(m => m.ID == cab.ModelType).Select(m => m.Name).FirstOrDefaultAsync();

            string text = "Ваш баланс:  " + cab.Balance +
                "₽\nИспользуемая модель:  " + (model ?? ConfigurationManager.ModelName) +
                "\nВаш ID:  " + UserId +
                "\n\nВы можете выбрать модель из списка.\nЦена указана в формате: \nP - Цена за 1k токенов вашего запроса,\nA - Цена за 1k токенов ответа модели.";

            if (edit)
            {
                await _bot.EditMessageTextAsync(ChatId, MessageId, text, replyMarkup: keyboard);
            }
            else
            {
                await _bot.SendTextMessageAsync(ChatId, text, replyMarkup: keyboard);
            }
        }
    }
}
