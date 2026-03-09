using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Course_Work.Models
{
    [Table("Players_Games")]
    public class PlayersGames
    {
        [Column("ID_Players")]
        public int ID_Players { get; set; }

        [Column("ID_Games")]
        public int ID_Games { get; set; }
        
        [ForeignKey("ID_Players")]
        public virtual Player Player { get; set; } = null!;

        [ForeignKey("ID_Games")]
        public virtual Game Game { get; set; } = null!;
    }
}