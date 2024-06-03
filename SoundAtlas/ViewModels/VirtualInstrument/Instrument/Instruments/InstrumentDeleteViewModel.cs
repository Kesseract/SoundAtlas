using Microsoft.Toolkit.Mvvm.Input;
using SoundAtlas.Models;
using System.Windows.Input;
using System.Windows;
using System.Collections.ObjectModel;

namespace SoundAtlas.ViewModels.VirtualInstrument.Instrument.Instruments
{
    public class InstrumentDeleteViewModel
    {
        private readonly DatabaseService _databaseService;
        public ICommand DeleteInstrumentsCommand { get; private set; }
        public ObservableCollection<InstrumentItemViewModel> SelectedItems { get; private set; }

        public InstrumentDeleteViewModel(IEnumerable<InstrumentItemViewModel> selectedItems)
        {
            _databaseService = new DatabaseService();
            DeleteInstrumentsCommand = new RelayCommand(DeleteInstruments);
            SelectedItems = new ObservableCollection<InstrumentItemViewModel>(selectedItems);
        }

        public void DeleteInstruments()
        {
            if (MessageBox.Show("Are you sure you want to delete the selected instruments?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var selectedInstruments = SelectedItems.Select(c => new InstrumentModel
                {
                    InstrumentId = c.InstrumentId,
                    Name = c.Name
                }).ToList();
                _databaseService.DeleteEntities(selectedInstruments);
                MessageBox.Show("Selected instruments have been deleted.", "Deletion Successful", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
