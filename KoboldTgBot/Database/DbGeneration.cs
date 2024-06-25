using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoboldTgBot.Database
{
    #nullable disable

    [Table("Generations")]
    public class DbGeneration : BaseEntity
    {
        public int PromptTokens { get; set; }
        public int CompletionTokens { get; set; }

        public long UserId { get; set; }

        [MaxLength(byte.MaxValue)]
        public string GenerationId { get; set; } 

        public string Prompt { get; set; } 

        [MaxLength(4096)]
        public string Answer { get; set; }

        [MaxLength(byte.MaxValue)]
        public string Model { get; set; }
    }
}
