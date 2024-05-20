using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoundAtlas.Models
{
    [Table("virtual_instrument_parameters")]
    public class VirtualInstrumentParameterModel
    {
        [Key]
        [Column("virtual_instrument_parameters_id ")]
        public int VirtualInstrumentParameterId { get; set; }

        [Column("virtual_instrument_presets_id")]
        public int VirtualInstrumentPresetId { get; set; }

        [ForeignKey("VirtualInstrumentPresetId")]
        public virtual VirtualInstrumentPresetModel VirtualInstrumentPreset { get; set; } = new VirtualInstrumentPresetModel();

        [Column("name")]
        [MaxLength(255)]
        [Required]
        public string Name { get; set; } = "";

        [Column("value")]
        [MaxLength(255)]
        [Required]
        public string Value { get; set; } = "";
    }
}
