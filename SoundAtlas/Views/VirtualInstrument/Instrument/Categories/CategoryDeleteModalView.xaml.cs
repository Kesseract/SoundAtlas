using System.Windows;
using SoundAtlas.ViewModels.VirtualInstrument.Instrument.Categories;

namespace SoundAtlas.Views.VirtualInstrument.Instrument.Categories
{
    /// <summary>
    /// CategoryDeleteModalView.xaml の相互作用ロジック
    /// </summary>
    public partial class CategoryDeleteModalView : Window
    {
        public CategoryDeleteModalView()
        {
            InitializeComponent();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as CategoryDeleteViewModel;
            if (viewModel != null)
            {
                viewModel.DeleteCategories();  // ViewModelの削除メソッドを呼び出す
                DialogResult = true;
                Close();  // モーダルウィンドウを閉じる
            }
        }
    }
}
