using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Course_Work.Models
{
    [Table("Teams_Matches")]
    public class TeamsMatch
    {
        [Column("ID_Teams")]
        public int ID_Teams { get; set; }

        [Column("ID_Matches")]
        public int ID_Matches { get; set; }

        [MaxLength(20)]
        public string? Team_Side { get; set; }
        
        [ForeignKey("ID_Teams")]
        public virtual Team Team { get; set; } = null!;

        [ForeignKey("ID_Matches")]
        public virtual Match Match { get; set; } = null!;
    }
}