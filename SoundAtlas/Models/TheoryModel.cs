using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoundAtlas.Models
{
    [Table("theories")]
    public class TheoryModel
    {
        [Key]
        [Column("theories_id ")]
        public int TheoryId { get; set; }

        [Column("name")]
        [MaxLength(255)]
        [Required]
        public string Name { get; set; } = "";

        [Column("melody_flg")]
        [Required]
        public bool MelodyFlg { get; set; }

        [Column("chrod_flg")]
        [Required]
        public bool ChordFlg { get; set; }

        [Column("rhythm_flg")]
        [Required]
        public bool RhythmFlg { get; set; }
    }
}
