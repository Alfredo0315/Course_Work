using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Course_Work.Models
{
    [Table("Tournament")]
    public class Tournament
    {
        [Key]
        [Column("ID_Tournament")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_Tournament { get; set; }

        [Required]
        [MaxLength(300)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "date")]
        public DateTime Start_date { get; set; }

        [Column(TypeName = "date")]
        public DateTime? End_date { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Prize_pool { get; set; }

        [MaxLength(200)]
        public string? Location_of_the_event { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }
        [Column("ID_Games")]
        public int? ID_Games { get; set; }

        [ForeignKey("ID_Games")]
        public virtual Game? Game { get; set; }
      
        public virtual ICollection<TeamsTournament> TeamsTournaments { get; set; } = new List<TeamsTournament>();
        public virtual ICollection<Match> Matches { get; set; } = new List<Match>();
    }
}