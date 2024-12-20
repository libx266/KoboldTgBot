﻿using KoboldTgBot.Database;
using KoboldTgBot.Extensions.Database;
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
        private readonly StateMachineCreateRole _smRole = new();
        private readonly StateMachineMultiMessage _smMultiMessage = new();

        private readonly Queue<Func<Task>> _llama3 = new();

        internal TelegramBot(string token) =>
            _bot = new TelegramBotClient(_token = token);

        internal async void StartPooling()
        {
            _bot.StartReceiving(HandleUpdateAsync, async (bot, ex, cancel) => await Task.Run(() => ex.Log()));

            while (true)
            {
                if (_llama3.TryDequeue(out var task))
                {
                    await task();
                }
                else
                {
                    await Task.Delay(100);
                }
            }
        }

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
                else if (_smRole.IsEnable(message.Chat.Id, out var createRoleState))
                {
                    cmd = createRoleState switch
                    {
                        StateCreateRole.Title => factory.Create<CommandCreateRoleStoreTitle>(_smRole),
                        StateCreateRole.Name => factory.Create<CommandCreateRoleStoreName>(_smRole),
                        StateCreateRole.Gender => factory.Create<CommandCreateRoleStoreGender>(_smRole),
                        StateCreateRole.Charakter => factory.Create<CommandCreateRoleStoreCharakter>(_smRole),
                        StateCreateRole.Specialisation => factory.Create<CommandCreateRoleStoreSpecialisation>(_smRole),
                        StateCreateRole.Relation => factory.Create<CommandCreateRoleStoreRelation>(_smRole),
                        StateCreateRole.Style => factory.Create<CommandCreateRoleStoreStyle>(_smRole),
                        _ => cmd
                    };
                }
                else if (_smMultiMessage.IsEnable(message.Chat.Id, out var _))
                {
                    cmd = factory.Create<CommandMultiMessage>(_smMultiMessage);
                }
                else if (message.Text.StartsWith('/'))
                {
                    var cmdParites = message.Text.Split(':');

                    string cmdHead = cmdParites.First();
                    string? cmdBody = cmdParites.LastOrDefault();

                    cmd = cmdHead switch
                    {
                        CommandStart.Name => factory.Create<CommandStart>(),
                        CommandClear.Name => factory.Create<CommandClear>(),
                        CommandRole.Name => factory.Create<CommandRole>(_smRole),
                        CommandRegen.Name => factory.Create<CommandRegen>(),
                        CommandEdit.Name => factory.Create<CommandEdit>(_smEdit),
                        CommandMultiMessage.Name => factory.Create<CommandMultiMessage>(_smMultiMessage),
                        CommandMore.Name => factory.Create<CommandMore>(),
                        CommandBalance.Name => factory.Create<CommandBalance>(),
                        CommandDelete.Name => factory.Create<CommandDelete>(),
                        CommandRename.Name => factory.Create<CommandRename>(cmdBody),
                        CommandRuOnly.Name => factory.Create<CommandRuOnly>(),
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
                CallbackAcceptRole.Name => factory.Create<CallbackAcceptRole>(_smRole),
                CallbackCreateRole.Name => factory.Create<CallbackCreateRole>(_smRole),
                CallbackDeleteRole.Name => factory.Create<CallbackDeleteRole>(_smRole),
                CallbackSelectModel.Name => factory.Create<CallbackSelectModel>(),
                _ => default
            };

            await ExecuteAction(clb);
        }

        private async void HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken token)
        {
            var task = async () =>
            {
                if (update.Message is not null)
                {
                    await HandleCommandAsync(update.Message);
                }
                if (update.CallbackQuery is not null)
                {
                    await HandleCallbackAsync(update.CallbackQuery);
                }
            };

            using var db = new DataContext();

            if
            (
                await db.IsExternalModelEnable(update.Message?.From?.Id ?? update.CallbackQuery!.From.Id) || 
                update.Message?.Text == CommandBalance.Name ||
                update?.CallbackQuery?.Data?.Split('=')?.FirstOrDefault() == CallbackSelectModel.Name
            )
            {
                var _ = Task.Run(task);
            }
            else
            {
                _llama3.Enqueue(task);
            }
        }
    }
}
