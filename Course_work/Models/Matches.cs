using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Course_Work.Models
{
    [Table("Matches")]
    public class Match
    {
        [Key]
        [Column("ID_Matches")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_Matches { get; set; }

        [Required]
        public DateTime Match_date { get; set; }
        
        public TimeSpan Match_time { get; set; }

        [MaxLength(50)]
        public string? Score { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = "Запланирован";

        [Required]
        [Column("ID_Tournament")]
        public int ID_Tournament { get; set; }
        
        [ForeignKey("ID_Tournament")]
        public virtual Tournament Tournament { get; set; } = null!;
        
        public virtual ICollection<TeamsMatch> TeamsMatches { get; set; } = new List<TeamsMatch>();
    }
}