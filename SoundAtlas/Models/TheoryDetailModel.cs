using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoundAtlas.Models
{
    [Table("theory_details")]
    public class TheoryDetailModel
    {
        [Key]
        [Column("theory_details_id ")]
        public int TheoryDetailId { get; set; }

        [Column("theories_id")]
        [Required]
        public int TheoryId { get; set; }

        [ForeignKey("TheoryId")]
        public virtual TheoryModel Theory { get; set; } = new TheoryModel();

        [Column("memo")]
        public string? Memo { get; set; }
    }
}
