using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Course_Work.Models
{
    [Table("Games_News")]
    public class GamesNews
    {
        [Column("ID_Games")]
        public int ID_Games { get; set; }

        [Column("ID_News")]
        public int ID_News { get; set; }
        
        [ForeignKey("ID_Games")]
        public virtual Game Game { get; set; } = null!;

        [ForeignKey("ID_News")]
        public virtual News News { get; set; } = null!;
    }
}