using SoundAtlas.ViewModels.VirtualInstrument.Instrument.Instruments;
using System.Windows;

namespace SoundAtlas.Views.VirtualInstrument.Instrument.Instruments
{
    public partial class InstrumentUpdateModalView : Window
    {
        public InstrumentUpdateModalView(InstrumentUpdateViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel; // ViewModelをDataContextに設定
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is InstrumentUpdateViewModel viewModel)
            {
                viewModel.UpdateInstrumentDetail();
                MessageBox.Show("Instrument details updated successfully.");
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
