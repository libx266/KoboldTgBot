using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoboldTgBot.Database
{
    [Table("Cabinets")]
    public class DbCabinet : BaseEntity
    {
        public long UserId { get; set; }
        public decimal Balance { get; set; } = 0m;
        public int ModelType { get; set; } = 0;
    }
}
