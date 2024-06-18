using KoboldTgBot.Database;
using KoboldTgBot.Extensions;
using KoboldTgBot.Neuro;
using KoboldTgBot.TgBot.Objects;
using Telegram.Bot;

namespace KoboldTgBot.TgBot.Actions.Commands
{
    internal abstract class CommandWithGenerationBase : TgAction<MessageHandler>
    {
        public CommandWithGenerationBase(ITelegramBotClient bot, MessageHandler entity) : base(bot, entity)
        {
        }

        protected async Task<string> GenerateAsync(DataContext db)
        {
            var prompt = await db.ConstructPropmptAsync(ChatId, Entity.Get(m => m.From));

            string answer = "._.";

            var generation = Task.Run(async () => answer = await GenerationApi.GenerateAsync(prompt));

            var typing = Task.Run(async () =>
            {
                while (!generation.IsCompleted)
                {
                    await _bot.SendChatActionAsync(ChatId, Telegram.Bot.Types.Enums.ChatAction.Typing);
                    await Task.Delay(TimeSpan.FromSeconds(2));
                }
            });

            await Task.WhenAny(generation, typing);

            return answer;
        }
    }
}
