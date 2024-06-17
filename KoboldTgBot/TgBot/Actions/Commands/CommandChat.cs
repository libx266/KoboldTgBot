using KoboldTgBot.Database;
using KoboldTgBot.Neuro;
using KoboldTgBot.Utils;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace KoboldTgBot.TgBot.Actions.Commands
{
    internal sealed class CommandChat : TgCommandBase
    {
        public CommandChat(ITelegramBotClient bot, Message message) : base(bot, message)
        {
        }

        protected override async Task WorkAsync()
        {
            if (!string.IsNullOrEmpty(_message.Text))
            {
                using var db = new DataContext();

                await db.Messages.AddAsync(new DbMessage
                {
                    Text = _message.Text,
                    UserId = _message.From!.Id,
                    ChatId = _message.Chat.Id,
                    TgId = _message.MessageId
                });

                await db.SaveChangesAsync();

                var prompt = await db.ConstructPropmptAsync(_message.Chat.Id, _message.From);

                string answer = "Произошла ошибка, извините пожалуйста!";

                var generation = Task.Run(async () => answer = await GenerationApi.GenerateAsync(prompt));

                var typing = Task.Run(async () =>
                {
                    while (!generation.IsCompleted)
                    {
                        await _bot.SendChatActionAsync(_message.Chat.Id, Telegram.Bot.Types.Enums.ChatAction.Typing);
                        await Task.Delay(TimeSpan.FromSeconds(2));
                    }
                });

                await Task.WhenAny(generation, typing);

                var sendedMessage = await _bot.SendTextMessageAsync(_message.Chat.Id, answer);

                await db.Messages.AddAsync(new DbMessage
                {
                    Text = sendedMessage.Text!,
                    UserId = -1L,
                    ChatId = sendedMessage.Chat.Id,
                    TgId = sendedMessage.MessageId
                });

                await db.SaveChangesAsync();
            }
        }
    }
}
