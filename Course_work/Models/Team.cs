using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Course_Work.Models
{
    /// <summary>
    /// Модель команды
    /// </summary>
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

        [Column("ID_Tournament")]
        public int? ID_Tournament { get; set; }

        // Навигационные свойства
        [ForeignKey("ID_Tournament")]
        public virtual Tournament? Tournament { get; set; }

        public virtual ICollection<Player> Players { get; set; } = new List<Player>();
    }
}