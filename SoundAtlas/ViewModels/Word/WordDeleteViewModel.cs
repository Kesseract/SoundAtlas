using Microsoft.Toolkit.Mvvm.Input;
using SoundAtlas.Models;
using System.Windows.Input;
using System.Windows;
using System.Collections.ObjectModel;

namespace SoundAtlas.ViewModels.Word
{
    public class WordDeleteViewModel
    {
        private readonly DatabaseService _databaseService;
        public ICommand DeleteWordsCommand { get; private set; }
        public ObservableCollection<WordItemViewModel> SelectedItems { get; private set; }

        public WordDeleteViewModel(IEnumerable<WordItemViewModel> selectedItems)
        {
            _databaseService = new DatabaseService();
            DeleteWordsCommand = new RelayCommand(DeleteWords);
            SelectedItems = new ObservableCollection<WordItemViewModel>(selectedItems);
        }

        public void DeleteWords()
        {
            if (MessageBox.Show("Are you sure you want to delete the selected words?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var selectedWords = SelectedItems.Select(w => new WordModel
                {
                    WordId = w.WordId,
                    Name = w.Name,
                    Abstract = w.Abstract
                }).ToList();
                _databaseService.DeleteEntities(selectedWords);
                MessageBox.Show("Selected words have been deleted.", "Deletion Successful", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
