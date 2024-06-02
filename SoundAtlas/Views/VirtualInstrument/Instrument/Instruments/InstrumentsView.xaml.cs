using SoundAtlas.Models;
using SoundAtlas.ViewModels.VirtualInstrument.Instrument.Instruments;
using SoundAtlas.Views.VirtualInstrument.Instrument.Instruments;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SoundAtlas.Views.VirtualInstrument.Instrument.Instruments
{
    /// <summary>
    /// InstrumentsView.xaml の相互作用ロジック
    /// </summary>
    public partial class InstrumentsView : UserControl
    {
        public ObservableCollection<InstrumentModel>? Instruments { get; private set; }
        public InstrumentsView()
        {
            InitializeComponent();
            var InstrumentViewModel = new InstrumentViewModel();
            this.DataContext = InstrumentViewModel;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // ここで単語追加用のモーダルウィンドウを開く
            InstrumentCreateModalView addModal = new InstrumentCreateModalView();
            var result = addModal.ShowDialog();
            if (result == true)
            {
                var viewModel = DataContext as InstrumentViewModel;
                if (viewModel != null)
                {
                    viewModel.LoadInstruments();  // viewModelを通してLoadInstrumentsを呼び出す
                }
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as InstrumentViewModel;
            if (viewModel != null)
            {
                viewModel.SearchText = SearchTextBox.Text;
                viewModel.SelectedSearchColumn = (SearchColumnComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
                viewModel.SearchInstruments();
            }
        }

        private void TextBlock_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var textBlock = sender as TextBlock;
            if (textBlock != null && e.ChangedButton == MouseButton.Left)
            {
                InstrumentItemViewModel? selectedInstrument = textBlock.DataContext as InstrumentItemViewModel;
                if (selectedInstrument != null)
                {
                    var detailViewModel = new InstrumentUpdateViewModel(selectedInstrument.InstrumentId);
                    var detailModal = new InstrumentUpdateModalView(detailViewModel);
                    var result = detailModal.ShowDialog();
                    if (result == true)
                    {
                        var viewModel = DataContext as InstrumentViewModel;
                        if (viewModel != null)
                        {
                            viewModel.LoadInstruments();  // viewModelを通してLoadInstrumentsを呼び出す
                        }
                    }
                }
            }
        }

        private void DeleteSelected_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as InstrumentViewModel;
            if (viewModel != null)
            {
                var selectedItems = viewModel.Instruments.Where(c => c.IsSelected).ToList();
                if (selectedItems.Any())
                {
                    var deleteViewModel = new InstrumentDeleteViewModel(selectedItems);
                    var deleteModal = new InstrumentDeleteModalView { DataContext = deleteViewModel };
                    var result = deleteModal.ShowDialog();
                    if (result == true)
                    {
                        viewModel.LoadInstruments();
                    }
                }
                else
                {
                    MessageBox.Show("No Instruments selected for deletion.", "No Selection", MessageBoxButton.OK);
                }
            }
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                var item = checkBox.DataContext as InstrumentItemViewModel;
                if (item != null)
                {
                    item.IsSelected = checkBox.IsChecked ?? false;
                }
            }
        }

        private void ExportCsvButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as InstrumentViewModel;
            if (viewModel != null)
            {
                viewModel.ExportCsv();
            }
        }

        private void ImportCsvButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as InstrumentViewModel;
            if (viewModel != null)
            {
                viewModel.ImportCsv();
            }
        }
    }
}
