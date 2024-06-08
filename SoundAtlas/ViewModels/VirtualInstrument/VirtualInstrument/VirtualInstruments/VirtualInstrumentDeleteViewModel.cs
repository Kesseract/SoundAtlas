using Microsoft.Toolkit.Mvvm.Input;
using SoundAtlas.Models;
using System.Windows.Input;
using System.Windows;
using System.Collections.ObjectModel;

namespace SoundAtlas.ViewModels.VirtualInstrument.VirtualInstrument.VirtualInstruments
{
    public class VirtualInstrumentDeleteViewModel
    {
        private readonly DatabaseService _databaseService;
        public ICommand DeleteVirtualInstrumentsCommand { get; private set; }
        public ObservableCollection<VirtualInstrumentItemViewModel> SelectedItems { get; private set; }

        public VirtualInstrumentDeleteViewModel(IEnumerable<VirtualInstrumentItemViewModel> selectedItems)
        {
            _databaseService = new DatabaseService();
            DeleteVirtualInstrumentsCommand = new RelayCommand(DeleteVirtualInstruments);
            SelectedItems = new ObservableCollection<VirtualInstrumentItemViewModel>(selectedItems);
        }

        public void DeleteVirtualInstruments()
        {
            if (MessageBox.Show("Are you sure you want to delete the selected virtualinstruments?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var selectedVirtualInstruments = SelectedItems.Select(v => new VirtualInstrumentModel
                {
                    VirtualInstrumentId = v.VirtualInstrumentId,
                    Name = v.Name,
                    SiteUrl = v.SiteUrl,
                    Version = v.Version,
                    LastUpdated = v.LastUpdated,
                    Image = v.Image,
                    Memo = v.Memo,
                }).ToList();
                _databaseService.DeleteEntities(selectedVirtualInstruments);
                MessageBox.Show("Selected virtualinstruments have been deleted.", "Deletion Successful", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
