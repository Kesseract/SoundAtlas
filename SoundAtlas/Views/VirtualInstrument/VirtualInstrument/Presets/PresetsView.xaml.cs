using SoundAtlas.Models;
using SoundAtlas.ViewModels.VirtualInstrument.VirtualInstrument.Presets;
using SoundAtlas.ViewModels.VirtualInstrument.VirtualInstrument.Parameters;
using SoundAtlas.Views.VirtualInstrument.VirtualInstrument.Parameters;
using SoundAtlas.Views.VirtualInstrument.VirtualInstrument.Presets;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SoundAtlas.Views.VirtualInstrument.VirtualInstrument.Presets
{
    /// <summary>
    /// PresetsView.xaml の相互作用ロジック
    /// </summary>
    public partial class PresetsView : UserControl
    {
        public ObservableCollection<VirtualInstrumentPresetModel>? Presets { get; private set; }
        public PresetsView()
        {
            InitializeComponent();
            var PresetViewModel = new PresetViewModel();
            this.DataContext = PresetViewModel;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // ここで単語追加用のモーダルウィンドウを開く
            PresetCreateModalView addModal = new PresetCreateModalView();
            var result = addModal.ShowDialog();
            if (result == true)
            {
                var viewModel = DataContext as PresetViewModel;
                if (viewModel != null)
                {
                    viewModel.LoadPresets();  // viewModelを通してLoadPresetsを呼び出す
                }
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as PresetViewModel;
            if (viewModel != null)
            {
                viewModel.SearchText = SearchTextBox.Text;
                viewModel.SelectedSearchColumn = (SearchColumnComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
                viewModel.SearchPresets();
            }
        }

        private void TextBlock_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var textBlock = sender as TextBlock;
            if (textBlock != null && e.ChangedButton == MouseButton.Left)
            {
                PresetItemViewModel? selectedPreset = textBlock.DataContext as PresetItemViewModel;
                if (selectedPreset != null)
                {
                    var detailViewModel = new PresetUpdateViewModel(selectedPreset.PresetId);
                    var detailModal = new PresetUpdateModalView(detailViewModel);
                    var result = detailModal.ShowDialog();
                    if (result == true)
                    {
                        var viewModel = DataContext as PresetViewModel;
                        if (viewModel != null)
                        {
                            viewModel.LoadPresets();  // viewModelを通してLoadPresetsを呼び出す
                        }
                    }
                }
            }
        }

        private void Parameter_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var textBlock = sender as TextBlock;
            if (textBlock != null && e.ChangedButton == MouseButton.Left)
            {
                PresetItemViewModel? selectedPreset = textBlock.DataContext as PresetItemViewModel;
                if (selectedPreset != null)
                {
                    var parameterEditViewModel = new ParameterViewModel();
                    var parameterEditModal = new ParameterEditModalView(selectedPreset.PresetId);
                    var result = parameterEditModal.ShowDialog();
                }
            }
        }

        private void DeleteSelected_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as PresetViewModel;
            if (viewModel != null)
            {
                var selectedItems = viewModel.Presets.Where(c => c.IsSelected).ToList();
                if (selectedItems.Any())
                {
                    var deleteViewModel = new PresetDeleteViewModel(selectedItems);
                    var deleteModal = new PresetDeleteModalView { DataContext = deleteViewModel };
                    var result = deleteModal.ShowDialog();
                    if (result == true)
                    {
                        viewModel.LoadPresets();
                    }
                }
                else
                {
                    MessageBox.Show("No Presets selected for deletion.", "No Selection", MessageBoxButton.OK);
                }
            }
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                var item = checkBox.DataContext as PresetItemViewModel;
                if (item != null)
                {
                    item.IsSelected = checkBox.IsChecked ?? false;
                }
            }
        }

        private void ExportCsvButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as PresetViewModel;
            if (viewModel != null)
            {
                viewModel.ExportCsv();
            }
        }

        private void ImportCsvButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as PresetViewModel;
            if (viewModel != null)
            {
                viewModel.ImportCsv();
            }
        }
    }
}
