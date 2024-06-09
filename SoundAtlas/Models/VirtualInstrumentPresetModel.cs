using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoundAtlas.Models
{
    [Table("virtual_instrument_presets")]
    public class VirtualInstrumentPresetModel
    {
        [Key]
        [Column("virtual_instrument_presets_id")]
        public int VirtualInstrumentPresetId { get; set; }

        [Column("virtual_instruments_id")]
        [Required]
        public int VirtualInstrumentId { get; set; }

        [ForeignKey("VirtualInstrumentId")]
        public virtual VirtualInstrumentModel? VirtualInstrument { get; set; }

        [Column("instruments_id")]
        [Required]
        public int InstrumentId { get; set; }

        [ForeignKey("InstrumentId")]
        public virtual InstrumentModel? Instrument { get; set; }

        [Column("name")]
        [MaxLength(255)]
        [Required]
        public string Name { get; set; } = "";

        [Column("rate")]
        [Required]
        public int Rate { get; set; }

        [Column("melody_flg")]
        [Required]
        public bool MelodyFlg { get; set; }

        [Column("chord_flg")]
        [Required]
        public bool ChordFlg { get; set; }

        [Column("bass_flg")]
        [Required]
        public bool BassFlg { get; set; }

        [Column("chrod_rhythm_flg")]
        [Required]
        public bool ChordRhythmFlg { get; set; }

        [Column("percussion_flg")]
        [Required]
        public bool PercussionFlg { get; set; }
    }
}
