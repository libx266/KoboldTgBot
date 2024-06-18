using System.ComponentModel.DataAnnotations.Schema;

namespace KoboldTgBot.Database
{
    [Table("CurrentRoles")]
    public sealed class DbCurrentRole : BaseEntity
    {
        public long ChatId { get; set; }
        public int RoleId { get; set; }
    }
}
