using KoboldTgBot.Neuro;
using KoboldTgBot.TgBot.Actions;
using KoboldTgBot.TgBot.Actions.Callbacks;
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
        private readonly StateMachineCreateRole _smCreateRole = new();

        internal TelegramBot(string token) =>
            _bot = new TelegramBotClient(_token = token);

        internal void StartPooling() => _bot.StartReceiving(HandleUpdateAsync, async (bot, ex, cancel) => await Task.Run(() => ex.Log()));

        private async Task ExecuteAction(TgActionBase? action)
        {
            if (action is null)
                return;

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

                if (_smEdit.IsEnable(message.Chat.Id, out var editState))
                {
                    cmd = editState switch
                    {
                        StateEdit.Process => factory.CreateComand<CommandEditProcess>(_smEdit),
                        _ => cmd
                    };
                }
                else if (_smCreateRole.IsEnable(message.Chat.Id, out var createRoleState))
                {
                    cmd = createRoleState switch
                    {
                        StateCreateRole.Name => factory.CreateComand<CommandCreateRoleStoreName>(_smCreateRole),
                        StateCreateRole.Description => factory.CreateComand<CommandCreateRoleStoreDescription>(_smCreateRole),
                        _ => cmd
                    };
                }
                else if (message.Text.StartsWith('/'))
                {
                    cmd = message.Text switch
                    {
                        "/start" => factory.CreateComand<CommandStart>(),
                        "/clear" => factory.CreateComand<CommandClear>(),
                        "/role" => factory.CreateComand<CommandRole>(),
                        "/regen" => factory.CreateComand<CommandRegen>(),
                        "/edit" => factory.CreateComand<CommandEditPrepare>(_smEdit),
                        _ => factory.CreateComand<CommandUnknown>()
                    };
                }

                await ExecuteAction(cmd);
            }
        }

        private async Task HandleCallbackAsync(CallbackQuery callback)
        {
            var factory = new TgCallbackFactory(_bot, callback);

            TgCallbackBase? clb = callback.Data!.Split('=').FirstOrDefault() switch
            {
                "role" => factory.CreateCallback<CallbackRole>(),
                "accept_role" => factory.CreateCallback<CallbackAcceptRole>(),
                "create_role" => factory.CreateCallback<CallbackCreateRole>(_smCreateRole),
                "delete_role" => factory.CreateCallback<CallbackDeleteRole>(),
                _ => default
            };

            await ExecuteAction(clb);
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
