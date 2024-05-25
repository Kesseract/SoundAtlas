using SoundAtlas.Models;

namespace SoundAtlas.ViewModels.Theory
{
    public class TheoryCreateViewModel
    {
        private readonly DatabaseService _databaseService;
        public string? Theory { get; set; }
        public bool MelodyFlg { get; set; } = false;
        public bool ChordFlg { get; set; } = false;
        public bool RhythmFlg { get; set; } = false;
        public string? Abstract { get; set; }
        public string? Detail { get; set; }

        public TheoryCreateViewModel()
        {
            _databaseService = new DatabaseService();
        }

        public void AddTheory()
        {
            var theory = new TheoryModel
            {
                Name = Theory ?? string.Empty,
                MelodyFlg = MelodyFlg,
                ChordFlg = ChordFlg,
                RhythmFlg = RhythmFlg,
                Abstract = Abstract ?? string.Empty,
                Detail = Detail ?? string.Empty
            };

            _databaseService.AddEntity(theory);
        }
    }
}
