using SoundAtlas.Models;

namespace SoundAtlas.ViewModels.Theory
{
    public class TheoryUpdateViewModel
    {
        private readonly DatabaseService _databaseService;
        public TheoryUpdateItemViewModel? TheoryUpdate { get; private set; }


        public TheoryUpdateViewModel(int theoryId)
        {
            _databaseService = new DatabaseService();
            LoadTheoryById(theoryId);
        }

        private void LoadTheoryById(int theoryId)
        {
            var theory = _databaseService.GetEntityById<TheoryModel>(theoryId);
            if (theory != null)
            {
                TheoryUpdate = new TheoryUpdateItemViewModel
                {
                    TheoryId = theory.TheoryId,
                    Name = theory.Name,
                    MelodyFlg = theory.MelodyFlg,
                    ChordFlg = theory.ChordFlg,
                    RhythmFlg = theory.RhythmFlg,
                    Abstract = theory.Abstract,
                    Detail = theory.Detail
                };
            }
        }
        public void UpdateTheoryDetail()
        {
            if (TheoryUpdate != null)
            {
                _databaseService.UpdateEntity(new TheoryModel
                {
                    TheoryId = TheoryUpdate.TheoryId,
                    Name = TheoryUpdate.Name,
                    MelodyFlg = TheoryUpdate.MelodyFlg,
                    ChordFlg = TheoryUpdate.ChordFlg,
                    RhythmFlg = TheoryUpdate.RhythmFlg,
                    Abstract = TheoryUpdate.Abstract,
                    Detail = TheoryUpdate.Detail
                });
            }
        }
    }
    public class TheoryUpdateItemViewModel
    {
        public int TheoryId { get; set; }
        public string Name { get; set; } = "";
        public bool MelodyFlg { get; set; } = false;
        public bool ChordFlg { get; set; } = false;
        public bool RhythmFlg { get; set; } = false;
        public string? Abstract { get; set; }
        public string? Detail { get; set; }
    }
}
