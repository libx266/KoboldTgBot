using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoboldTgBot.Database
{
    [Table("CurrentRoles")]
    public sealed class DbCurrentRole : BaseEntity
    {
        public long ChatId { get; set; }
        public int RoleId { get; set; }
    }
}
