using SoundAtlas.Models;

namespace SoundAtlas.ViewModels.VirtualInstrument.Instrument.Categories
{
    public class CategoryUpdateViewModel
    {
        private readonly DatabaseService _databaseService;
        public CategoryUpdateItemViewModel? CategoryUpdate { get; private set; }


        public CategoryUpdateViewModel(int categoryId)
        {
            _databaseService = new DatabaseService();
            LoadCategoryById(categoryId);
        }

        private void LoadCategoryById(int categoryId)
        {
            var category = _databaseService.GetEntityById<InstrumentCategoryModel>(categoryId);
            if (category != null)
            {
                CategoryUpdate = new CategoryUpdateItemViewModel
                {
                    CategoryId = category.InstrumentCategoryId,
                    Classification1 = category.Classification1,
                    Classification2 = category.Classification2,
                    Classification3 = category.Classification3,
                    Classification4 = category.Classification4,
                };
            }
        }
        public void UpdateCategoryDetail()
        {
            if (CategoryUpdate != null)
            {
                _databaseService.UpdateEntity(new InstrumentCategoryModel
                {
                    InstrumentCategoryId = CategoryUpdate.CategoryId,
                    Classification1 = CategoryUpdate.Classification1,
                    Classification2 = CategoryUpdate.Classification2,
                    Classification3 = CategoryUpdate.Classification3,
                    Classification4 = CategoryUpdate.Classification4,
                });
            }
        }
    }
    public class CategoryUpdateItemViewModel
    {
        public int CategoryId { get; set; }
        public string Classification1 { get; set; } = "";
        public string? Classification2 { get; set; }
        public string? Classification3 { get; set; }
        public string? Classification4 { get; set; }
    }
}
