using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Course_Work.Models
{
    [Table("Teams_Tournament")]
    public class TeamsTournament
    {
        [Column("ID_Teams")]
        public int ID_Teams { get; set; }

        [Column("ID_Tournament")]
        public int ID_Tournament { get; set; }
        
        [ForeignKey("ID_Teams")]
        public virtual Team Team { get; set; } = null!;

        [ForeignKey("ID_Tournament")]
        public virtual Tournament Tournament { get; set; } = null!;
    }
}