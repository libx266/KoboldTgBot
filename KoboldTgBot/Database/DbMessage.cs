using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KoboldTgBot.Database
{
    [Table("Messages")]
    public sealed class DbMessage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [MaxLength(4096)]
        public string Text { get; set; }
        public long SenderId { get; set; }
        public long ChatId { get; set; }
        public long TgId { get; set; }
        public bool IsEdited { get; set; } = false;
        public bool InMemory { get; set; } = true;

        public DateTime SenderDate { get; set; } = DateTime.UtcNow;
    }
}
