using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DatabaseService;

namespace SoundAtlas.Models
{
    [Table("words")]
    public class WordModel
    {
        [Key]
        [Column("words_id")]
        public int WordId { get; set; }

        [Column("name")]
        [MaxLength(255)]
        [Required]
        public string Name { get; set; } = "";

        [Column("abstract")]
        public string? Abstract { get; set; }

        [Column("detail")]
        public string? Detail { get; set; }
    }
}
