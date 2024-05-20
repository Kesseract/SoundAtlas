using Microsoft.Win32;
using SoundAtlas.Models;
using SoundAtlas.ViewModels.Word;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SoundAtlas.Views
{
    /// <summary>
    /// WordsView.xaml の相互作用ロジック
    /// </summary>
    public partial class WordsView : UserControl
    {
        public ObservableCollection<WordModel>? Words { get; private set; }
        public WordsView()
        {
            InitializeComponent();
            var wordViewModel = new WordViewModel();
            this.DataContext = wordViewModel;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // ここで単語追加用のモーダルウィンドウを開く
            WordCreateModalView addModal = new WordCreateModalView();
            var result = addModal.ShowDialog();
            if (result == true)
            {
                var viewModel = DataContext as WordViewModel;
                if (viewModel != null)
                {
                    viewModel.LoadWords();  // viewModelを通してLoadWordsを呼び出す
                }
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as WordViewModel;
            if (viewModel != null)
            {
                viewModel.SearchText = SearchTextBox.Text;
                viewModel.SelectedSearchColumn = (SearchColumnComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
                viewModel.SearchWords();
            }
        }

        private void WordTextBlock_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var textBlock = sender as TextBlock;
            if (textBlock != null && e.ChangedButton == MouseButton.Left)
            {
                WordItemViewModel? selectedWord = textBlock.DataContext as WordItemViewModel;
                if (selectedWord != null)
                {
                    var detailViewModel = new WordUpdateViewModel(selectedWord.WordId);
                    var detailModal = new WordUpdateModalView(detailViewModel);
                    var result = detailModal.ShowDialog();
                    if (result == true)
                    {
                        var viewModel = DataContext as WordViewModel;
                        if (viewModel != null)
                        {
                            viewModel.LoadWords();  // viewModelを通してLoadWordsを呼び出す
                        }
                    }
                }
            }
        }

        private void DeleteSelectedWords_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as WordViewModel;
            if (viewModel != null)
            {
                var selectedItems = viewModel.Words.Where(w => w.IsSelected).ToList();
                if (selectedItems.Any())
                {
                    var deleteViewModel = new WordDeleteViewModel(selectedItems);
                    var deleteModal = new WordDeleteModalView { DataContext = deleteViewModel };
                    var result = deleteModal.ShowDialog();
                    if (result == true)
                    {
                        viewModel.LoadWords();
                    }
                }
                else
                {
                    MessageBox.Show("No words selected for deletion.", "No Selection", MessageBoxButton.OK);
                }
            }
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                var item = checkBox.DataContext as WordItemViewModel;
                if (item != null)
                {
                    item.IsSelected = checkBox.IsChecked ?? false;
                }
            }
        }

        private void ExportCsvButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as WordViewModel;
            if (viewModel != null)
            {
                viewModel.ExportCsv();
            }
        }

        private void ImportCsvButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("CSVファイルのインポートが完了しました。", "エクスポート成功", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
