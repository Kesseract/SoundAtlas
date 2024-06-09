using Microsoft.Toolkit.Mvvm.Input;
using SoundAtlas.Models;
using System.Windows.Input;
using System.Windows;
using System.Collections.ObjectModel;

namespace SoundAtlas.ViewModels.VirtualInstrument.VirtualInstrument.Presets
{
    public class PresetDeleteViewModel
    {
        private readonly DatabaseService _databaseService;
        public ICommand DeletePresetsCommand { get; private set; }
        public ObservableCollection<PresetItemViewModel> SelectedItems { get; private set; }

        public PresetDeleteViewModel(IEnumerable<PresetItemViewModel> selectedItems)
        {
            _databaseService = new DatabaseService();
            DeletePresetsCommand = new RelayCommand(DeletePresets);
            SelectedItems = new ObservableCollection<PresetItemViewModel>(selectedItems);
        }

        public void DeletePresets()
        {
            if (MessageBox.Show("Are you sure you want to delete the selected presets?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var selectedPresets = SelectedItems.Select(p => new VirtualInstrumentPresetModel
                {
                    VirtualInstrumentPresetId = p.PresetId,
                    Rate = p.Rate,
                    MelodyFlg = p.MelodyFlg,
                    ChordFlg = p.ChordFlg,
                    BassFlg = p.BassFlg,
                    ChordRhythmFlg = p.ChordRhythmFlg,
                    PercussionFlg = p.PercussionFlg,
                }).ToList();
                _databaseService.DeleteEntities(selectedPresets);
                MessageBox.Show("Selected presets have been deleted.", "Deletion Successful", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
