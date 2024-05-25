using SoundAtlas.ViewModels.Theory;
using System.Windows;

namespace SoundAtlas.Views
{
    /// <summary>
    /// WordDeleteModalView.xaml の相互作用ロジック
    /// </summary>
    public partial class TheoryDeleteModalView : Window
    {
        public TheoryDeleteModalView()
        {
            InitializeComponent();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as TheoryDeleteViewModel;
            if (viewModel != null)
            {
                viewModel.DeleteTheories();  // ViewModelの削除メソッドを呼び出す
                DialogResult = true;
                Close();  // モーダルウィンドウを閉じる
            }
        }
    }
}
