﻿using KoboldTgBot.Database;
using KoboldTgBot.Extensions.Database;
using KoboldTgBot.Extensions.Utils;
using KoboldTgBot.TgBot.Actions.Callbacks;
using KoboldTgBot.TgBot.Objects;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace KoboldTgBot.TgBot.Actions.Commands
{
    internal sealed class CommandRole : TgAction<MessageHandler>
    {
        public const string Name = "/role";

        public CommandRole(ITelegramBotClient bot, MessageHandler entity) : base(bot, entity)
        {
        }

        protected override async Task WorkAsync()
        {
            using var db = new DataContext();

            var buttons = new List<List<InlineKeyboardButton>>();

            int index = 0;

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

            (await db.GetRoleShortListAsync(UserId)).ForEach(r => AddButton(TgHelper.MakeInlineButton<CallbackRole>(r.Title, r.ID)));

            AddButton(TgHelper.MakeInlineButton<CallbackCreateRole>("Добавить"));

            var keyboard = new InlineKeyboardMarkup(buttons);

            await _bot.SendTextMessageAsync(ChatId, "Выберите роль персонажа:", replyMarkup: keyboard);
        }
    }
}
