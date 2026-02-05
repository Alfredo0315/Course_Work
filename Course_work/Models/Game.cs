using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Course_Work.Models
{
    /// <summary>
    /// Модель игры
    /// </summary>
    [Table("Games")]
    public class Game
    {
        [Key]
        [Column("ID_Games")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_Games { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Jenre { get; set; }

        [MaxLength(100)]
        public string? Platform { get; set; }

        public int? Release_year { get; set; }

        // Навигационные свойства
        public virtual ICollection<GamesNews> GamesNews { get; set; } = new List<GamesNews>();
        public virtual ICollection<PlayersGames> PlayersGames { get; set; } = new List<PlayersGames>();
    }
}