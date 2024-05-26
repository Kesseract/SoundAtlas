using SoundAtlas.Models;
using SoundAtlas.ViewModels.Theory;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SoundAtlas.Views.Theory
{
    /// <summary>
    /// TheoriesView.xaml の相互作用ロジック
    /// </summary>
    public partial class TheoriesView : UserControl
    {
        public ObservableCollection<TheoryModel>? Theories { get; private set; }
        public TheoriesView()
        {
            InitializeComponent();
            var theoryViewModel = new TheoryViewModel();
            this.DataContext = theoryViewModel;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // ここで単語追加用のモーダルウィンドウを開く
            TheoryCreateModalView addModal = new TheoryCreateModalView();
            var result = addModal.ShowDialog();
            if (result == true)
            {
                var viewModel = DataContext as TheoryViewModel;
                if (viewModel != null)
                {
                    viewModel.LoadTheories();  // viewModelを通してLoadTheoriesを呼び出す
                }
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as TheoryViewModel;
            if (viewModel != null)
            {
                viewModel.SearchText = SearchTextBox.Text;
                viewModel.SelectedSearchColumn = (SearchColumnComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
                viewModel.SearchTheories();
            }
        }

        private void TheoryTextBlock_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var textBlock = sender as TextBlock;
            if (textBlock != null && e.ChangedButton == MouseButton.Left)
            {
                TheoryItemViewModel? selectedTheory = textBlock.DataContext as TheoryItemViewModel;
                if (selectedTheory != null)
                {
                    var detailViewModel = new TheoryUpdateViewModel(selectedTheory.TheoryId);
                    var detailModal = new TheoryUpdateModalView(detailViewModel);
                    var result = detailModal.ShowDialog();
                    if (result == true)
                    {
                        var viewModel = DataContext as TheoryViewModel;
                        if (viewModel != null)
                        {
                            viewModel.LoadTheories();  // viewModelを通してLoadWordsを呼び出す
                        }
                    }
                }
            }
        }

        private void DeleteSelected_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as TheoryViewModel;
            if (viewModel != null)
            {
                var selectedItems = viewModel.Theories.Where(w => w.IsSelected).ToList();
                if (selectedItems.Any())
                {
                    var deleteViewModel = new TheoryDeleteViewModel(selectedItems);
                    var deleteModal = new TheoryDeleteModalView { DataContext = deleteViewModel };
                    var result = deleteModal.ShowDialog();
                    if (result == true)
                    {
                        viewModel.LoadTheories();
                    }
                }
                else
                {
                    MessageBox.Show("No Theories selected for deletion.", "No Selection", MessageBoxButton.OK);
                }
            }
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                var item = checkBox.DataContext as TheoryItemViewModel;
                if (item != null)
                {
                    item.IsSelected = checkBox.IsChecked ?? false;
                }
            }
        }

        private void ExportCsvButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as TheoryViewModel;
            if (viewModel != null)
            {
                viewModel.ExportCsv();
            }
        }

        private void ImportCsvButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as TheoryViewModel;
            if (viewModel != null)
            {
                viewModel.ImportCsv();
            }
        }

        private void OnAddWord_Click(object sender, RoutedEventArgs e)
        {
            // senderからButtonを特定し、必要に応じてDataContextを取得
            Console.WriteLine(sender);
            var button = sender as Button;
            if (button != null)
            {
                var theory = button.DataContext as TheoryItemViewModel;
                Console.WriteLine(theory);
                if (theory != null)
                {
                    // ここでダイアログを表示し、ユーザーに語彙を選択させる
                    // 選択された語彙をデータベースにリンクする処理など
                    Console.WriteLine(theory.TheoryId);
                    Console.WriteLine(theory.Name);
                }
            }
        }
    }
}
