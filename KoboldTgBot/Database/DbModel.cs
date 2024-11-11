using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoboldTgBot.Database
{
    #nullable disable
    [Table("Models")]
    public class DbModel : BaseEntity
    {
        public string Name { get; set; }
        public decimal Prompt1kTokensCostRub { get; set; }
        public decimal Answer1kTokensCostRub { get; set; }
        public decimal Cache1kTokensCostRub { get; set; }
        public string Endpoint { get; set; }
    }
}
