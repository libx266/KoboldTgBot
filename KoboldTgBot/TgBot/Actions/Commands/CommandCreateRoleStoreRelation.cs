using KoboldTgBot.TgBot.Objects;
using Telegram.Bot;

namespace KoboldTgBot.TgBot.Actions.Commands
{
    internal sealed class CommandCreateRoleStoreRelation : CommandCreateRoleStoreBase
    {
        public CommandCreateRoleStoreRelation(ITelegramBotClient bot, MessageHandler message) : base(bot, message)
        {
        }

        protected override async Task WorkAsync() =>
            await Store((role, msg) => role.Relation = msg, "Опишите стиль ведения диалога, которого персонаж должен приедерживаться", States.StateCreateRole.Style);
    }
}
