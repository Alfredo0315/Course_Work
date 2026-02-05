using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Course_Work.Models
{
    /// <summary>
    /// Модель связи игр и новостей (многие ко многим)
    /// </summary>
    [Table("Games_News")]
    public class GamesNews
    {
        [Key]
        [Column("ID_Games_News")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_Games_News { get; set; }

        [Required]
        [Column("ID_Games")]
        public int ID_Games { get; set; }

        [Required]
        [Column("ID_News")]
        public int ID_News { get; set; }

        // Навигационные свойства
        [ForeignKey("ID_Games")]
        public virtual Game Game { get; set; } = null!;

        [ForeignKey("ID_News")]
        public virtual News News { get; set; } = null!;
    }
}