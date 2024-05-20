using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoundAtlas.Models
{
    [Table("virtual_instrument_details")]
    public class VirtualInstrumentDetailModel
    {
        [Key]
        [Column("virtual_instrument_details_id")]
        public int VirtualInstrumentDetailId { get; set; }

        [Column("virtual_instruments_id")]
        [Required]
        public int VirtualInstrumentId { get; set; }

        [ForeignKey("VirtualInstrumentId")]
        public virtual VirtualInstrumentModel VirtualInstrument { get; set; } = new VirtualInstrumentModel();

        [Column("site_url")]
        public string? SiteUrl { get; set; }

        [Column("version")]
        [MaxLength(255)]
        public string? Version { get; set; }

        [Column("last_updated")]
        public DateTime? LastUpdated { get; set; }

        [Column("image")]
        public string? Image { get; set; }

        [Column("memo")]
        public string? Memo { get; set; }
    }
}
