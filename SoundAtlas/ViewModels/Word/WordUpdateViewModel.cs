using SoundAtlas.Models;

namespace SoundAtlas.ViewModels.Word
{
    public class WordUpdateViewModel
    {
        private readonly DatabaseService _databaseService;
        public WordUpdateItemViewModel? WordUpdate { get; private set; }


        public WordUpdateViewModel(int wordId)
        {
            _databaseService = new DatabaseService();
            LoadWordById(wordId);
        }

        private void LoadWordById(int wordId)
        {
            var word = _databaseService.GetEntityById<WordModel>(wordId);
            if (word != null)
            {
                WordUpdate = new WordUpdateItemViewModel
                {
                    WordId = word.WordId,
                    Name = word.Name,
                    Abstract = word.Abstract,
                    Detail = word.Detail
                };
            }
        }
        public void UpdateWordDetail()
        {
            if (WordUpdate != null)
            {
                _databaseService.UpdateEntity(new WordModel
                {
                    WordId = WordUpdate.WordId,
                    Name = WordUpdate.Name,
                    Abstract = WordUpdate.Abstract,
                    Detail = WordUpdate.Detail
                });
            }
        }
    }
    public class WordUpdateItemViewModel
    {
        public int WordId { get; set; }
        public string Name { get; set; } = "";
        public string? Abstract { get; set; }
        public string? Detail { get; set; }
    }
}
