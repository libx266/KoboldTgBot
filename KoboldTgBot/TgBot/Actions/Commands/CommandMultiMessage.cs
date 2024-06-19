using KoboldTgBot.Database;
using KoboldTgBot.Extensions.Database;
using KoboldTgBot.TgBot.Objects;
using KoboldTgBot.TgBot.States;
using Telegram.Bot;

namespace KoboldTgBot.TgBot.Actions.Commands
{
    internal sealed class CommandMultiMessage : TgStatedAction<MessageHandler, StateMultiMessage, StateMachineMultiMessage>
    {
        public const string Name = "/mult";

        public CommandMultiMessage(ITelegramBotClient bot, MessageHandler entity) : base(bot, entity)
        {
        }

        protected override async Task WorkAsync()
        {
            if (StateIsDisable())
            {
                var msg = await _bot.SendTextMessageAsync(ChatId, "Можете отправлять сообщения, по окончанию введите /stop");

                CreateState(StateMultiMessage.Store);

                AddMessageToDelete(MessageId);
                AddMessageToDelete(msg.MessageId);
            }
            else if (Text == "/stop")
            {
                AddMessageToDelete(MessageId);

                var factory = new TgCommandFactory(_bot, Entity.Get(m => m));
                var chat = factory.Create<CommandChat>(new object());
                await chat.ExecuteAsync();

                DisableState();

                await DeleteMessages();
            }
            else
            {
                using var db = new DataContext();

                await db.AddMessageAsync(Text, UserId, ChatId, MessageId);

                await db.SaveChangesAsync();
            }
        }
    }
}
