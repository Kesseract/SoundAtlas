using System.Windows;
using System.Windows.Input;
using SoundAtlas.ViewModels.VirtualInstrument.VirtualInstrument.Presets;

namespace SoundAtlas.Views.VirtualInstrument.VirtualInstrument.Presets
{
    /// <summary>
    /// PresetCreateModalView.xaml の相互作用ロジック
    /// </summary>
    public partial class PresetCreateModalView : Window
    {
        public PresetCreateModalView()
        {
            InitializeComponent();
            DataContext = new PresetCreateViewModel();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(rate.Text, out int parsedRate))
            {

                if (DataContext is PresetCreateViewModel viewModel)
                {
                    viewModel.Name = name.Text;
                    viewModel.SelectedVirtualInstrumentId = (int)virtualInstrumentComboBox.SelectedValue;
                    viewModel.SelectedInstrumentId = (int)instrumentComboBox.SelectedValue;
                    viewModel.Rate = parsedRate;
                    viewModel.MelodyFlg = checkBoxMelodyFlg.IsChecked ?? false;
                    viewModel.ChordFlg = checkBoxChordFlg.IsChecked ?? false;
                    viewModel.BassFlg = checkBoxBassFlg.IsChecked ?? false;
                    viewModel.ChordRhythmFlg = checkBoxChordRhythmFlg.IsChecked ?? false;
                    viewModel.PercussionFlg = checkBoxPercussionFlg.IsChecked ?? false;

                    viewModel.AddPreset();
                    MessageBox.Show("Preset created successfully.");
                    DialogResult = true;
                    Close();
                }
            }
        }

        private void Rate_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _);  // 数値でないものを拒否する
        }
    }
}
