using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoundAtlas.Models
{
    [Table("instrument_categories")]
    public class InstrumentCategoryModel
    {
        [Key]
        [Column("instrument_categories_id")]
        public int InstrumentCategoryId { get; set; }

        [Column("classification1")]
        [MaxLength(255)]
        [Required]
        public string Classification1 { get; set; } = "";

        [Column("classification2")]
        [MaxLength(255)]
        public string? Classification2 { get; set; }

        [Column("classification3")]
        [MaxLength(255)]
        public string? Classification3 { get; set; }

        [Column("classification4")]
        [MaxLength(255)]
        public string? Classification4 { get; set; }
    }
}
