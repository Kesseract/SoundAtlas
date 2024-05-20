using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoundAtlas.Models
{
    [Table("theory_word_linkages")]
    public class TheoryWordLinkageModel
    {
        [Key]
        [Column("theory_word_linkages_id ")]
        public int TheoryWordLinkageId { get; set; }

        [Column("theories_id")]
        [Required]
        public int TheoryId { get; set; }

        [ForeignKey("TheoryId")]
        public virtual TheoryModel Theory { get; set; } = new TheoryModel();

        [Column("words_id")]
        [Required]
        public int WordId { get; set; }

        [ForeignKey("WordId")]
        public virtual WordModel Word { get; set; } = new WordModel();
    }
}
