using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoboldTgBot.Database
{
    [Table("Roles")]
    public sealed class DbRole : BaseEntity
    {
        [MaxLength(64)]
        public string Name { get; set; } = null!;

        public long UserId { get; set; }

        [MaxLength(1024)]
        public string Description { get; set; } = null!;
    }
}
