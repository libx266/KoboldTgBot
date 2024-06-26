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
        public decimal Balance { get; set; } = 0;
        public decimal PromptTokenPrice { get; set; } = 1.44m;
        public decimal CompletionTokenPrice { get; set; } = 4.32m;
        public bool IsGpt4o { get; set; } = false;
    }
}
