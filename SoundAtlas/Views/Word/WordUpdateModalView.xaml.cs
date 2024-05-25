using SoundAtlas.ViewModels.Word;
using System.Windows;

namespace SoundAtlas.Views.Word
{
    public partial class WordUpdateModalView : Window
    {
        public WordUpdateModalView(WordUpdateViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel; // ここでWordModelオブジェクトをDataContextに設定
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is WordUpdateViewModel viewModel)
            {
                viewModel.UpdateWordDetail();
                MessageBox.Show("Word details updated successfully.");
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
