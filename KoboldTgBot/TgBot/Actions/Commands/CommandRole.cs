using KoboldTgBot.Database;
using KoboldTgBot.Extensions.Database;
using KoboldTgBot.Extensions.Utils;
using KoboldTgBot.TgBot.Actions.Callbacks;
using KoboldTgBot.TgBot.Objects;
using KoboldTgBot.TgBot.States;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace KoboldTgBot.TgBot.Actions.Commands
{
    internal sealed class CommandRole : TgStatedAction<MessageHandler, StateCreateRole, StateMachineCreateRole>
    {
        public const string Name = "/role";
        private const int RolesPerPage = 10;

        public CommandRole(ITelegramBotClient bot, MessageHandler entity) : base(bot, entity)
        {
        }

        protected override async Task WorkAsync()
        {
            using var db = new DataContext();

            var roles = await db.GetRoleShortListAsync(UserId);
            var totalPages = (int)Math.Ceiling((double)roles.Count / (double)RolesPerPage);

            for (var page = 1; page <= totalPages; page++)
            {
                var buttons = new List<List<InlineKeyboardButton>>();
                var index = 0;

                Action<InlineKeyboardButton> AddButton = b =>
                {
                    if (Convert.ToBoolean(index % 2))
                    {
                        buttons.Last().Add(b);
                    }
                    else
                    {
                        buttons.Add(new List<InlineKeyboardButton>() { b });
                    }

                    index++;
                };

                var startIndex = (page - 1) * RolesPerPage;
                var endIndex = Math.Min(startIndex + RolesPerPage, roles.Count);

                for (var i = startIndex; i < endIndex; i++)
                {
                    var role = roles[i];
                    AddButton(TgHelper.MakeInlineButton<CallbackRole>(role.Title, role.ID));
                }

                if (page == totalPages)
                {
                    AddButton(TgHelper.MakeInlineButton<CallbackCreateRole>("Добавить"));
                }

                var keyboard = new InlineKeyboardMarkup(buttons);

                AddMessageToDelete((await _bot.SendTextMessageAsync(ChatId, $"Выберите роль персонажа (страница {page} из {totalPages}):", replyMarkup: keyboard)).MessageId);
            }
        }
    }
}
