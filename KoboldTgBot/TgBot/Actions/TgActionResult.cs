using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoboldTgBot.TgBot.Actions
{
    internal record TgActionResult(bool status, Exception? error = default, object? data = default);
}
