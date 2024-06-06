using SoundAtlas.Models;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using static SoundAtlas.ViewModels.VirtualInstrument.Instrument.Instruments.InstrumentCreateViewModel;

namespace SoundAtlas.ViewModels.VirtualInstrument.Instrument.Instruments
{
    public class InstrumentUpdateViewModel
    {
        private readonly DatabaseService _databaseService;
        public InstrumentUpdateItemViewModel? InstrumentUpdate { get; private set; }
        public ObservableCollection<InstrumentCategoryCreateViewModel> Categories { get; private set; }

        public InstrumentUpdateViewModel(int instrumentId)
        {
            _databaseService = new DatabaseService();
            Categories = new ObservableCollection<InstrumentCategoryCreateViewModel>();
            LoadCategories();
            LoadInstrumentById(instrumentId);
        }

        private void LoadCategories()
        {
            Categories.Clear();
            var categoryList = _databaseService.GetAllEntities<InstrumentCategoryModel>().Select(category => new InstrumentCategoryCreateViewModel
            {
                CategoryId = category.InstrumentCategoryId,
                ClassificationJoin = string.Join("-", new[] { category.Classification1, category.Classification2, category.Classification3, category.Classification4 }
                                                          .Where(c => !string.IsNullOrEmpty(c)))
            }).ToList();

            foreach (var category in categoryList)
            {
                Categories.Add(category);
            }
        }

        private void LoadInstrumentById(int instrumentId)
        {
            var instrument = _databaseService.GetEntityById<InstrumentModel>(instrumentId);
            if (instrument != null)
            {
                InstrumentUpdate = new InstrumentUpdateItemViewModel
                {
                    InstrumentId = instrument.InstrumentId,
                    Name = instrument.Name,
                    SelectedCategoryId = instrument.InstrumentCategoryId  // ここでカテゴリIDを設定
                };
            }
        }

        public void UpdateInstrumentDetail()
        {
            if (InstrumentUpdate != null)
            {
                var updatedInstrument = new InstrumentModel
                {
                    InstrumentId = InstrumentUpdate.InstrumentId,
                    Name = InstrumentUpdate.Name,
                    InstrumentCategoryId = InstrumentUpdate.SelectedCategoryId
                };
                _databaseService.UpdateEntity(updatedInstrument);
            }
        }
    }
    public class InstrumentUpdateItemViewModel
    {
        public int InstrumentId { get; set; }
        public string Name { get; set; } = "";
        public int SelectedCategoryId { get; set; }
    }
}
