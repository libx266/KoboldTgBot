using KoboldTgBot.Database;
using KoboldTgBot.TgBot.States;
using KoboldTgBot.Utils;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace KoboldTgBot.TgBot.Actions.Commands
{
    internal sealed class CommandCreateRoleStoreStyle : TgCommandBase
    {
        public CommandCreateRoleStoreStyle(ITelegramBotClient bot, Message message) : base(bot, message)
        {
        }

        protected override async Task WorkAsync()
        {
            var smCreateRole = _data as StateMachineCreateRole;

            var role = smCreateRole!.Role[_message.From!.Id];
            smCreateRole.DisableState(_message.Chat.Id);
            smCreateRole.Role.Remove(_message.From.Id);

            using var db = new DataContext();

            role.UserId = _message.From.Id;
            role.Style = _message.Text!;
            role.InsertDate = DateTime.UtcNow;

            await db.Roles.AddAsync(role);
            await db.SaveChangesAsync();

            smCreateRole.AddMessageToDelete(_message.Chat.Id, _message.MessageId);

            foreach (int m in smCreateRole.GetMessagesToDelete(_message.Chat.Id))
            {
                try
                {
                    await _bot.DeleteMessageAsync(_message.Chat.Id, m);
                }
                catch (Exception ex) 
                {
                    ex.Log();
                }
            }

            await _bot.SendTextMessageAsync(_message.Chat.Id, "Создана роль:  " + role.Title);
        }
    }
}
