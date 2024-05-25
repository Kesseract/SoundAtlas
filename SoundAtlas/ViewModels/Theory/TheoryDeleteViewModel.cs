using Microsoft.Toolkit.Mvvm.Input;
using SoundAtlas.Models;
using System.Windows.Input;
using System.Windows;
using System.Collections.ObjectModel;

namespace SoundAtlas.ViewModels.Theory
{
    public class TheoryDeleteViewModel
    {
        private readonly DatabaseService _databaseService;
        public ICommand DeleteTheoriesCommand { get; private set; }
        public ObservableCollection<TheoryItemViewModel> SelectedItems { get; private set; }

        public TheoryDeleteViewModel(IEnumerable<TheoryItemViewModel> selectedItems)
        {
            _databaseService = new DatabaseService();
            DeleteTheoriesCommand = new RelayCommand(DeleteTheories);
            SelectedItems = new ObservableCollection<TheoryItemViewModel>(selectedItems);
        }

        public void DeleteTheories()
        {
            if (MessageBox.Show("Are you sure you want to delete the selected theories?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var selectedTheories = SelectedItems.Select(t => new TheoryModel
                {
                    TheoryId = t.TheoryId,
                    Name = t.Name,
                    MelodyFlg = t.MelodyFlg,
                    ChordFlg = t.ChordFlg,
                    RhythmFlg = t.RhythmFlg,
                    Abstract = t.Abstract
                }).ToList();
                _databaseService.DeleteEntities(selectedTheories);
                MessageBox.Show("Selected theories have been deleted.", "Deletion Successful", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
