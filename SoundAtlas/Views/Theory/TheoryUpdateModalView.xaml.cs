using SoundAtlas.ViewModels.Theory;
using System.Windows;

namespace SoundAtlas.Views.Theory
{
    public partial class TheoryUpdateModalView : Window
    {
        public TheoryUpdateModalView(TheoryUpdateViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel; // ここでTheoryModelオブジェクトをDataContextに設定
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is TheoryUpdateViewModel viewModel)
            {
                viewModel.UpdateTheoryDetail();
                MessageBox.Show("Theory details updated successfully.");
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
