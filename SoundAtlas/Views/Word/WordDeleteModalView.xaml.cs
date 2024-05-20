using SoundAtlas.ViewModels.Word;
using System.Windows;

namespace SoundAtlas.Views
{
    /// <summary>
    /// WordDeleteModalView.xaml の相互作用ロジック
    /// </summary>
    public partial class WordDeleteModalView : Window
    {
        public WordDeleteModalView()
        {
            InitializeComponent();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as WordDeleteViewModel;
            if (viewModel != null)
            {
                viewModel.DeleteWords();  // ViewModelの削除メソッドを呼び出す
                DialogResult = true;
                Close();  // モーダルウィンドウを閉じる
            }
        }
    }
}
