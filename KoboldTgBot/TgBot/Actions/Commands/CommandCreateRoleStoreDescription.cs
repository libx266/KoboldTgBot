using KoboldTgBot.Database;
using KoboldTgBot.TgBot.States;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace KoboldTgBot.TgBot.Actions.Commands
{
    internal sealed class CommandCreateRoleStoreDescription : TgCommandBase
    {
        public CommandCreateRoleStoreDescription(ITelegramBotClient bot, Message message) : base(bot, message)
        {
        }

        protected override async Task WorkAsync()
        {
            var smCreateRole = _data as StateMachineCreateRole;

            string name = smCreateRole!.Name[_message.From!.Id];

            smCreateRole.AddMessageToDelete(_message.Chat.Id, _message.MessageId);
            smCreateRole.DisableState(_message.Chat.Id);
            smCreateRole.Name.Remove(_message.From.Id);

            using var db = new DataContext();

            await db.Roles.AddAsync(new DbRole
            {
                UserId = _message.From!.Id,
                Name = name,
                Description = _message.Text!
            });

            await db.SaveChangesAsync();

            foreach(int m in smCreateRole.GetMessagesToDelete(_message.Chat.Id))
            {
                await _bot.DeleteMessageAsync(_message.Chat.Id, m);
            }

            await _bot.SendTextMessageAsync(_message.Chat.Id, "Создана роль:  " + name);

        }
    }
}
