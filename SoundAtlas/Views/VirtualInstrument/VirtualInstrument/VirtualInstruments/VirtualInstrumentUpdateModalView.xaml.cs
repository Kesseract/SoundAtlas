using SoundAtlas.ViewModels.VirtualInstrument.VirtualInstrument.VirtualInstruments;
using System.Windows;

namespace SoundAtlas.Views.VirtualInstrument.VirtualInstrument.VirtualInstruments
{
    public partial class VirtualInstrumentUpdateModalView : Window
    {
        public VirtualInstrumentUpdateModalView(VirtualInstrumentUpdateViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel; // ここでVirtualInstrumentModelオブジェクトをDataContextに設定
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is VirtualInstrumentUpdateViewModel viewModel)
            {
                viewModel.UpdateVirtualInstrumentDetail();
                MessageBox.Show("VirtualInstrument details updated successfully.");
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
