using SoundAtlas.Models;

namespace SoundAtlas.ViewModels.Word
{
    public class WordCreateViewModel
    {
        private readonly DatabaseService _databaseService;
        public string? Word { get; set; }
        public string? Abstract { get; set; }
        public string? Details { get; set; }

        public WordCreateViewModel()
        {
            _databaseService = new DatabaseService();
        }

        public void AddWord()
        {
            var word = new WordModel
            {
                Name = Word ?? string.Empty,
                Abstract = Abstract ?? string.Empty,
                Detail = Details ?? string.Empty
            };

            _databaseService.AddEntity(word);
        }
    }
}
