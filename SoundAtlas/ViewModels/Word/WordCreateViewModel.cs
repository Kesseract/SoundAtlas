using SoundAtlas.Models;

namespace SoundAtlas.ViewModels.Word
{
    public class WordCreateViewModel
    {
        private readonly DatabaseService _databaseService;
        public string? Word { get; set; }
        public string? Abstract { get; set; }
        public string? Detail { get; set; }

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
                Detail = Detail ?? string.Empty
            };

            _databaseService.AddEntity(word);
        }
    }
}
