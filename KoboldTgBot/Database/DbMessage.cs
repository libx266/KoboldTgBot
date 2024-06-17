using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KoboldTgBot.Database
{
    [Table("Messages")]
    public sealed class DbMessage : BaseEntity
    {
        [MaxLength(4096)]
        public string Text { get; set; } = null!;
        public long UserId { get; set; }
        public long ChatId { get; set; }
        public int TgId { get; set; }
        public bool IsEdited { get; set; } = false;
        public bool InMemory { get; set; } = true;
    }
}
