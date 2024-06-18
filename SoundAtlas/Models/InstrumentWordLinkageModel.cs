using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoundAtlas.Models
{
    [Table("instrument_word_linkages")]
    public class InstrumentWordLinkageModel
    {
        [Key]
        [Column("instrument_word_linkages_id ")]
        public int InstrumentWordLinkageId { get; set; }

        [Column("virtual_instrument_presets_id")]
        [Required]
        public int VirtualInstrumentPresetId { get; set; }

        [ForeignKey("VirtualInstrumentPresetId")]
        public virtual VirtualInstrumentPresetModel VirtualInstrumentPreset { get; set; } = null!;

        [Column("words_id")]
        [Required]
        public int WordId { get; set;}

        [ForeignKey("WordId")]
        public virtual WordModel Word { get; set; } = null!;
    }
}
