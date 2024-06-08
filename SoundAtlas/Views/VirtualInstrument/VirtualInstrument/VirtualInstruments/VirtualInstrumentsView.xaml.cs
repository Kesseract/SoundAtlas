using SoundAtlas.Models;
using SoundAtlas.ViewModels.VirtualInstrument.VirtualInstrument.VirtualInstruments;
using SoundAtlas.Views.VirtualInstrument.VirtualInstrument.VirtualInstruments;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SoundAtlas.Views.VirtualInstrument.VirtualInstrument.VirtualInstruments
{
    /// <summary>
    /// VirtualInstrumentsView.xaml の相互作用ロジック
    /// </summary>
    public partial class VirtualInstrumentsView : UserControl
    {
        public ObservableCollection<InstrumentModel>? VirtualInstruments { get; private set; }
        public VirtualInstrumentsView()
        {
            InitializeComponent();
            var InstrumentViewModel = new VirtualInstrumentViewModel();
            this.DataContext = InstrumentViewModel;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // ここで単語追加用のモーダルウィンドウを開く
            VirtualInstrumentCreateModalView addModal = new VirtualInstrumentCreateModalView();
            var result = addModal.ShowDialog();
            if (result == true)
            {
                var viewModel = DataContext as VirtualInstrumentViewModel;
                if (viewModel != null)
                {
                    viewModel.LoadVirtualInstruments();  // viewModelを通してLoadVirtualInstrumentsを呼び出す
                }
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as VirtualInstrumentViewModel;
            if (viewModel != null)
            {
                viewModel.SearchText = SearchTextBox.Text;
                viewModel.SelectedSearchColumn = (SearchColumnComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
                viewModel.SearchVirtualInstruments();
            }
        }

        private void TextBlock_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var textBlock = sender as TextBlock;
            if (textBlock != null && e.ChangedButton == MouseButton.Left)
            {
                VirtualInstrumentItemViewModel? selectedVirtualInstrument = textBlock.DataContext as VirtualInstrumentItemViewModel;
                if (selectedVirtualInstrument != null)
                {
                    var detailViewModel = new VirtualInstrumentUpdateViewModel(selectedVirtualInstrument.VirtualInstrumentId);
                    var detailModal = new VirtualInstrumentUpdateModalView(detailViewModel);
                    var result = detailModal.ShowDialog();
                    if (result == true)
                    {
                        var viewModel = DataContext as VirtualInstrumentViewModel;
                        if (viewModel != null)
                        {
                            viewModel.LoadVirtualInstruments();  // viewModelを通してLoadVirtualInstrumentsを呼び出す
                        }
                    }
                }
            }
        }

        private void DeleteSelected_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as VirtualInstrumentViewModel;
            if (viewModel != null)
            {
                var selectedItems = viewModel.VirtualInstruments.Where(c => c.IsSelected).ToList();
                if (selectedItems.Any())
                {
                    var deleteViewModel = new VirtualInstrumentDeleteViewModel(selectedItems);
                    var deleteModal = new VirtualInstrumentDeleteModalView { DataContext = deleteViewModel };
                    var result = deleteModal.ShowDialog();
                    if (result == true)
                    {
                        viewModel.LoadVirtualInstruments();
                    }
                }
                else
                {
                    MessageBox.Show("No VirtualInstruments selected for deletion.", "No Selection", MessageBoxButton.OK);
                }
            }
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                var item = checkBox.DataContext as VirtualInstrumentItemViewModel;
                if (item != null)
                {
                    item.IsSelected = checkBox.IsChecked ?? false;
                }
            }
        }

        private void ExportCsvButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as VirtualInstrumentViewModel;
            if (viewModel != null)
            {
                viewModel.ExportCsv();
            }
        }

        private void ImportCsvButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as VirtualInstrumentViewModel;
            if (viewModel != null)
            {
                viewModel.ImportCsv();
            }
        }
    }
}
