using Microsoft.Toolkit.Mvvm.Input;
using SoundAtlas.Models;
using System.Windows.Input;
using System.Windows;
using System.Collections.ObjectModel;

namespace SoundAtlas.ViewModels.VirtualInstrument.Instrument.Categories
{
    public class CategoryDeleteViewModel
    {
        private readonly DatabaseService _databaseService;
        public ICommand DeleteCategoriesCommand { get; private set; }
        public ObservableCollection<CategoryItemViewModel> SelectedItems { get; private set; }

        public CategoryDeleteViewModel(IEnumerable<CategoryItemViewModel> selectedItems)
        {
            _databaseService = new DatabaseService();
            DeleteCategoriesCommand = new RelayCommand(DeleteCategories);
            SelectedItems = new ObservableCollection<CategoryItemViewModel>(selectedItems);
        }

        public void DeleteCategories()
        {
            if (MessageBox.Show("Are you sure you want to delete the selected categories?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var selectedCategories = SelectedItems.Select(c => new InstrumentCategoryModel
                {
                    InstrumentCategoryId = c.CategoryId,
                    Classification1 = c.Classification1,
                    Classification2 = c.Classification2,
                    Classification3 = c.Classification3,
                    Classification4 = c.Classification4,
                }).ToList();
                _databaseService.DeleteEntities(selectedCategories);
                MessageBox.Show("Selected categories have been deleted.", "Deletion Successful", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
