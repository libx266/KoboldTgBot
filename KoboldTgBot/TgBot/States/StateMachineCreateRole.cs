using KoboldTgBot.Database;

namespace KoboldTgBot.TgBot.States
{
    internal sealed class StateMachineCreateRole : StateMachineBase<StateCreateRole>
    {
        internal readonly Dictionary<long, DbRole> Role = new();
    }
}
