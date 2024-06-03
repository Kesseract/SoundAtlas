using System.Windows;
using SoundAtlas.ViewModels.VirtualInstrument.Instrument.Instruments;

namespace SoundAtlas.Views.VirtualInstrument.Instrument.Instruments
{
    /// <summary>
    /// AddInstrumentModalView.xaml の相互作用ロジック
    /// </summary>
    public partial class InstrumentCreateModalView : Window
    {
        public InstrumentCreateModalView()
        {
            InitializeComponent();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = new InstrumentCreateViewModel()
            {
                Name = name.Text,
            };

            viewModel.AddInstrument();
            MessageBox.Show("Instrument create successfully.");
            DialogResult = true;
            Close();
        }
    }
}
