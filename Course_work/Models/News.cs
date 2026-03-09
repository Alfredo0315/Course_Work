using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Course_Work.Models
{
    [Table("News")]
    public class News
    {
        [Key]
        [Column("ID_News")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_News { get; set; }

        [Required]
        [MaxLength(500)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "date")]
        public DateTime Date_of_publication { get; set; }

        [Required]
        [Column(TypeName = "time")]
        public TimeSpan Time_of_publication { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? Description { get; set; }
        
        public virtual ICollection<GamesNews> GamesNews { get; set; } = new List<GamesNews>();
    }
}