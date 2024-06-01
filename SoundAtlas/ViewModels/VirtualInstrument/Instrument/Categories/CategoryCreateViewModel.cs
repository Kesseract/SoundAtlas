using SoundAtlas.Models;

namespace SoundAtlas.ViewModels.VirtualInstrument.Instrument.Categories
{
    public class CategoryCreateViewModel
    {
        private readonly DatabaseService _databaseService;
        public string? Classification1 { get; set; }
        public string? Classification2 { get; set; }
        public string? Classification3 { get; set; }
        public string? Classification4 { get; set; }

        public CategoryCreateViewModel()
        {
            _databaseService = new DatabaseService();
        }

        public void AddCategory()
        {
            var category = new InstrumentCategoryModel
            {
                Classification1 = Classification1 ?? string.Empty,
                Classification2 = Classification2 ?? string.Empty,
                Classification3 = Classification3 ?? string.Empty,
                Classification4 = Classification4 ?? string.Empty,
            };

            _databaseService.AddEntity(category);
        }
    }
}
