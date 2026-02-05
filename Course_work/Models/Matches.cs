using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Course_Work.Models
{
    /// <summary>
    /// Модель матча
    /// </summary>
    [Table("Matches")]
    public class Match
    {
        [Key]
        [Column("ID_Matches")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_Matches { get; set; }

        [Required]
        public DateTime Match_date { get; set; }

        [MaxLength(50)]
        public string? Score { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = "Запланирован";

        [Required]
        [Column("ID_Tournament")]
        public int ID_Tournament { get; set; }

        // Навигационные свойства
        [ForeignKey("ID_Tournament")]
        public virtual Tournament Tournament { get; set; } = null!;
    }
}