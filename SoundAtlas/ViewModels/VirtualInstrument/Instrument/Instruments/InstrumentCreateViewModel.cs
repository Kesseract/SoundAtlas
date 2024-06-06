using SoundAtlas.Models;
using SoundAtlas.ViewModels.VirtualInstrument.Instrument.Categories;
using System.Collections.ObjectModel;

namespace SoundAtlas.ViewModels.VirtualInstrument.Instrument.Instruments
{
    public class InstrumentCreateViewModel
    {
        private readonly DatabaseService _databaseService;
        public string? Name { get; set; }
        public int SelectedCategoryId { get; set; }
        public ObservableCollection<InstrumentCategoryCreateViewModel> Categories { get; set; } = new ObservableCollection<InstrumentCategoryCreateViewModel>();
        

        public InstrumentCreateViewModel()
        {
            _databaseService = new DatabaseService();
            LoadCategories();
        }

        private void LoadCategories()
        {
            Categories.Clear();
            var categoryList = _databaseService.GetAllEntities<InstrumentCategoryModel>().Select(category => new InstrumentCategoryCreateViewModel
            {
                CategoryId = category.InstrumentCategoryId,
                // Classification1 から Classification4 までを適切に扱い、nullまたは空の値を無視
                ClassificationJoin = string.Join("-", new[] { category.Classification1, category.Classification2, category.Classification3, category.Classification4 }
                                                      .Where(c => !string.IsNullOrEmpty(c)))
            }).ToList();

            // コレクションにデータを追加
            foreach (var category in categoryList)
            {
                Categories.Add(category);
            }

            if (Categories.Any())
            {
                SelectedCategoryId = Categories[0].CategoryId;
            }
        }

        public void AddInstrument()
        {
            var instrument = new InstrumentModel
            {
                Name = Name ?? string.Empty,
                InstrumentCategoryId = SelectedCategoryId
            };

            _databaseService.AddEntity(instrument);
        }

        public class InstrumentCategoryCreateViewModel
        {
            public int CategoryId { get; set; }
            public string ClassificationJoin { get; set; } = "";
        }
    }
}
