using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Course_Work.Models
{
    /// <summary>
    /// Модель игрока
    /// </summary>
    [Table("Players")]
    public class Player
    {
        [Key]
        [Column("ID_Players")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_Players { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nickname { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Name { get; set; }

        [MaxLength(200)]
        public string? Surname { get; set; }

        [MaxLength(100)]
        public string? Country { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date_of_birth { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Prize_pool { get; set; } = 0;

        [Column("ID_Teams")]
        public int? ID_Teams { get; set; }

        // Навигационные свойства
        [ForeignKey("ID_Teams")]
        public virtual Team? Team { get; set; }

        public virtual ICollection<PlayersGames> PlayersGames { get; set; } = new List<PlayersGames>();
    }
}