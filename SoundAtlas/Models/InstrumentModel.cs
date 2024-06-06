using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoundAtlas.Models
{
    [Table("instruments")]
    public class InstrumentModel
    {
        [Key]
        [Column("instruments_id")]
        public int InstrumentId { get; set; }

        [Column("name")]
        [MaxLength(255)]
        [Required]
        public string Name { get; set; } = "";

        [Column("instrument_categories_id")]
        [Required]
        public int InstrumentCategoryId { get; set; }

        [ForeignKey("InstrumentCategoryId")]
        public virtual InstrumentCategoryModel InstrumentCategory { get; set; }
    }

}
