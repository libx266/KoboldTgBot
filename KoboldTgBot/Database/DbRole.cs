using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KoboldTgBot.Database
{
    #nullable disable

    [Table("Roles")]
    public sealed class DbRole : BaseEntity
    {
        [MaxLength(64)]
        public string Title { get; set; }
        public long UserId { get; set; }

        [MaxLength(64)]
        public string Name { get; set; }
        [MaxLength(64)]
        public string Gender { get; set; }

        [MaxLength(1024)]
        public string Character { get; set; }
        [MaxLength(1024)]
        public string Specialisation { get; set; }
        [MaxLength(1024)]
        public string Relation { get; set; }
        [MaxLength(1024)]
        public string Style { get; set; }
    }
}
