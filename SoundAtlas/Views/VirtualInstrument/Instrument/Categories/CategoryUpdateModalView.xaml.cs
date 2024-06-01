using SoundAtlas.ViewModels.VirtualInstrument.Instrument.Categories;
using System.Windows;

namespace SoundAtlas.Views.VirtualInstrument.Instrument.Categories
{
    public partial class CategoryUpdateModalView : Window
    {
        public CategoryUpdateModalView(CategoryUpdateViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel; // ここでCategoryModelオブジェクトをDataContextに設定
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is CategoryUpdateViewModel viewModel)
            {
                viewModel.UpdateCategoryDetail();
                MessageBox.Show("Category details updated successfully.");
                DialogResult = true;
                Close();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close(); // モーダルを閉じる
        }
    }
}
