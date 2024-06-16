using KoboldTgBot.TgBot.Actions;
using KoboldTgBot.TgBot.Actions.Commands;
using KoboldTgBot.TgBot.States;
using KoboldTgBot.Utils;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace KoboldTgBot.TgBot
{
    internal sealed class TelegramBot
    {
        private readonly ITelegramBotClient _bot;
        private readonly string _token;
        private readonly StateMachineEdit _smEdit = new();

        internal TelegramBot(string token) =>
            _bot = new TelegramBotClient(_token = token);

        internal void StartPooling() => _bot.StartReceiving(HandleUpdateAsync, async (bot, ex, cancel) => await Task.Run(() => ex.Log()));

        private async Task ExecuteAction(TgActionBase action)
        {
            var result = await action.ExecuteAsync();

            if (!result.Status && result.Error is not null)
            {
                result.Error.Log();
            }
        }

        private async Task HandleCommandAsync(Message message)
        {
            if (!string.IsNullOrEmpty(message.Text))
            {
                var factory = new TgCommandFactory(_bot, message);
               
                TgCommandBase cmd = factory.CreateComand<CommandChat>();

                if (_smEdit.IsEnable(message.Chat.Id, out var state))
                {
                    cmd = state switch
                    {
                        StateEdit.Process => new CommandEditProcess(_bot, message, _smEdit)
                    };
                }

                if (message.Text.StartsWith('/'))
                {
                    cmd = message.Text switch
                    {
                        "/start" => factory.CreateComand<CommandStart>(),
                        "/clear" => factory.CreateComand<CommandClear>(),
                        "/regen" => factory.CreateComand<CommandRegen>(),
                        "/edit" => new CommandEditPrepare(_bot, message, _smEdit),
                        _ => factory.CreateComand<CommandUnknown>()
                    };
                }

                await ExecuteAction(cmd);
            }
        }

        private async Task HandleCallbackAsync(CallbackQuery callback)
        {
            var parts = callback.Data.Split('=');

            throw new NotImplementedException();
        }

        private async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken token)
        {
            if (update.Message is not null)
            {
                await HandleCommandAsync(update.Message);
            }
            if (update.CallbackQuery is not null)
            {
                await HandleCallbackAsync(update.CallbackQuery);
            }
        }
    }
}
