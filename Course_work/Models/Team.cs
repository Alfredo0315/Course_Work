using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Course_Work.Models
{
    [Table("Teams")]
    public class Team
    {
        [Key]
        [Column("ID_Teams")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_Teams { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        public int? Founded_year { get; set; }

        [MaxLength(100)]
        public string? Country { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Prize_pool { get; set; } = 0;
        
        public virtual ICollection<Player> Players { get; set; } = new List<Player>();
        public virtual ICollection<TeamsTournament> TeamsTournaments { get; set; } = new List<TeamsTournament>();
        public virtual ICollection<TeamsMatch> TeamsMatches { get; set; } = new List<TeamsMatch>();
    }
}