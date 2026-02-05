using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Course_Work.Models
{
    /// <summary>
    /// Модель связи игроков и игр (многие ко многим)
    /// </summary>
    [Table("Players_Games")]
    public class PlayersGames
    {
        [Key]
        [Column("ID_Players_Games")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_Players_Games { get; set; }

        [Required]
        [Column("ID_Players")]
        public int ID_Players { get; set; }

        [Required]
        [Column("ID_Games")]
        public int ID_Games { get; set; }

        // Навигационные свойства
        [ForeignKey("ID_Players")]
        public virtual Player Player { get; set; } = null!;

        [ForeignKey("ID_Games")]
        public virtual Game Game { get; set; } = null!;
    }
}