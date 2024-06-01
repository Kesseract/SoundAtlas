using SoundAtlas.Models;
using SoundAtlas.ViewModels.VirtualInstrument.Instrument.Categories;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SoundAtlas.Views.VirtualInstrument.Instrument.Categories
{
    /// <summary>
    /// CategoriesView.xaml の相互作用ロジック
    /// </summary>
    public partial class CategoriesView : UserControl
    {
        public ObservableCollection<InstrumentCategoryModel>? Categories { get; private set; }
        public CategoriesView()
        {
            InitializeComponent();
            var categoryViewModel = new CategoryViewModel();
            this.DataContext = categoryViewModel;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // ここで単語追加用のモーダルウィンドウを開く
            CategoryCreateModalView addModal = new CategoryCreateModalView();
            var result = addModal.ShowDialog();
            if (result == true)
            {
                var viewModel = DataContext as CategoryViewModel;
                if (viewModel != null)
                {
                    viewModel.LoadCategories();  // viewModelを通してLoadCategoriesを呼び出す
                }
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as CategoryViewModel;
            if (viewModel != null)
            {
                viewModel.SearchText = SearchTextBox.Text;
                viewModel.SelectedSearchColumn = (SearchColumnComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
                viewModel.SearchCategories();
            }
        }

        private void TextBlock_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var textBlock = sender as TextBlock;
            if (textBlock != null && e.ChangedButton == MouseButton.Left)
            {
                CategoryItemViewModel? selectedCategory = textBlock.DataContext as CategoryItemViewModel;
                if (selectedCategory != null)
                {
                    var detailViewModel = new CategoryUpdateViewModel(selectedCategory.CategoryId);
                    var detailModal = new CategoryUpdateModalView(detailViewModel);
                    var result = detailModal.ShowDialog();
                    if (result == true)
                    {
                        var viewModel = DataContext as CategoryViewModel;
                        if (viewModel != null)
                        {
                            viewModel.LoadCategories();  // viewModelを通してLoadCategoriesを呼び出す
                        }
                    }
                }
            }
        }

        private void DeleteSelected_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as CategoryViewModel;
            if (viewModel != null)
            {
                var selectedItems = viewModel.Categories.Where(c => c.IsSelected).ToList();
                if (selectedItems.Any())
                {
                    var deleteViewModel = new CategoryDeleteViewModel(selectedItems);
                    var deleteModal = new CategoryDeleteModalView { DataContext = deleteViewModel };
                    var result = deleteModal.ShowDialog();
                    if (result == true)
                    {
                        viewModel.LoadCategories();
                    }
                }
                else
                {
                    MessageBox.Show("No categories selected for deletion.", "No Selection", MessageBoxButton.OK);
                }
            }
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                var item = checkBox.DataContext as CategoryItemViewModel;
                if (item != null)
                {
                    item.IsSelected = checkBox.IsChecked ?? false;
                }
            }
        }

        private void ExportCsvButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as CategoryViewModel;
            if (viewModel != null)
            {
                viewModel.ExportCsv();
            }
        }

        private void ImportCsvButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as CategoryViewModel;
            if (viewModel != null)
            {
                viewModel.ImportCsv();
            }
        }
    }
}
