using KoboldTgBot.Extensions.Utils;
using KoboldTgBot.TgBot.Actions;
using KoboldTgBot.TgBot.Actions.Callbacks;
using KoboldTgBot.TgBot.Actions.Commands;
using KoboldTgBot.TgBot.Objects;
using KoboldTgBot.TgBot.States;
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
        private readonly StateMachineMultiMessage _smMultiMessage = new();

        internal TelegramBot(string token) =>
            _bot = new TelegramBotClient(_token = token);

        internal void StartPooling() => _bot.StartReceiving(HandleUpdateAsync, async (bot, ex, cancel) => await Task.Run(() => ex.Log()));

        private async Task ExecuteAction<T>(TgAction<T>? action) where T : ActionEntity
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
               
                TgAction<MessageHandler> cmd = factory.Create<CommandChat>();

                if (_smEdit.IsEnable(message.Chat.Id, out var _))
                {
                    cmd = factory.Create<CommandEdit>(_smEdit);
                }
                else if (_smCreateRole.IsEnable(message.Chat.Id, out var createRoleState))
                {
                    cmd = createRoleState switch
                    {
                        StateCreateRole.Title => factory.Create<CommandCreateRoleStoreTitle>(_smCreateRole),
                        StateCreateRole.Name => factory.Create<CommandCreateRoleStoreName>(_smCreateRole),
                        StateCreateRole.Gender => factory.Create<CommandCreateRoleStoreGender>(_smCreateRole),
                        StateCreateRole.Charakter => factory.Create<CommandCreateRoleStoreCharakter>(_smCreateRole),
                        StateCreateRole.Specialisation => factory.Create<CommandCreateRoleStoreSpecialisation>(_smCreateRole),
                        StateCreateRole.Relation => factory.Create<CommandCreateRoleStoreRelation>(_smCreateRole),
                        StateCreateRole.Style => factory.Create<CommandCreateRoleStoreStyle>(_smCreateRole),
                        _ => cmd
                    };
                }
                else if (_smMultiMessage.IsEnable(message.Chat.Id, out var _))
                {
                    cmd = factory.Create<CommandMultiMessage>(_smMultiMessage);
                }
                else if (message.Text.StartsWith('/'))
                {
                    cmd = message.Text switch
                    {
                        CommandStart.Name => factory.Create<CommandStart>(),
                        CommandClear.Name => factory.Create<CommandClear>(),
                        CommandRole.Name => factory.Create<CommandRole>(),
                        CommandRegen.Name => factory.Create<CommandRegen>(),
                        CommandEdit.Name => factory.Create<CommandEdit>(_smEdit),
                        CommandMultiMessage.Name => factory.Create<CommandMultiMessage>(_smMultiMessage),
                        CommandMore.Name => factory.Create<CommandMore>(),
                        _ => factory.Create<CommandUnknown>()
                    };
                }

                await ExecuteAction(cmd);
            }
        }

        private async Task HandleCallbackAsync(CallbackQuery callback)
        {
            var factory = new TgCallbackFactory(_bot, callback);

            TgAction<CallbackHandler>? clb = callback.Data!.Split('=').FirstOrDefault() switch
            {
                CallbackRole.Name => factory.Create<CallbackRole>(),
                CallbackAcceptRole.Name => factory.Create<CallbackAcceptRole>(),
                CallbackCreateRole.Name => factory.Create<CallbackCreateRole>(_smCreateRole),
                CallbackDeleteRole.Name => factory.Create<CallbackDeleteRole>(),
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
