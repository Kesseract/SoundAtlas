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
    }
}
