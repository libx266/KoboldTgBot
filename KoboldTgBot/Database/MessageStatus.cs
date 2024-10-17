using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoboldTgBot.Database
{
    public enum MessageStatus
    {
        Actual = 0,
        Clear = 1,
        Regenerated = 2,
        Edited = 4,
        Deleted = 8
    }
}
