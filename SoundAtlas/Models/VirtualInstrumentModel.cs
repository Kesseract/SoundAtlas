using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoundAtlas.Models
{
    [Table("virtual_instruments")]
    public class VirtualInstrumentModel
    {
        [Key]
        [Column("virtual_instrument_id")]
        public int VirtualInstrumentId { get; set; }

        [Column("name")]
        [MaxLength(255)]
        [Required]
        public string Name { get; set; } = "";

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
