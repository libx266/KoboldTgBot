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
        public int? PromptTokens { get; set; } = null;
        public int? CompletionTokens { get; set; } = null;

        public long UserId { get; set; }

        [MaxLength(byte.MaxValue)]
        public string? GenerationId { get; set; } = null;

        public string Prompt { get; set; } 

        [MaxLength(4096)]
        public string Answer { get; set; }

        [MaxLength(byte.MaxValue)]
        public string Model { get; set; }

        public float? Temperature { get; set; } = null;
        public float? RepetitionPenalty { get; set; } = null;
        public float? TopPSampling { get; set; } = null;

        public bool IsLocal { get; set; }
    }
}
