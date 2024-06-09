using SoundAtlas.ViewModels.VirtualInstrument.VirtualInstrument.Presets;
using System.Windows;

namespace SoundAtlas.Views.VirtualInstrument.VirtualInstrument.Presets
{
    public partial class PresetUpdateModalView : Window
    {
        public PresetUpdateModalView(PresetUpdateViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel; // ViewModelをDataContextに設定
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is PresetUpdateViewModel viewModel)
            {
                viewModel.UpdatePresetDetail();
                MessageBox.Show("Preset details updated successfully.");
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
