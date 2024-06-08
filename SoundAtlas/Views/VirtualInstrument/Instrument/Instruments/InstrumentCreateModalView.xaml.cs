using System.Windows;
using SoundAtlas.ViewModels.VirtualInstrument.Instrument.Instruments;

namespace SoundAtlas.Views.VirtualInstrument.Instrument.Instruments
{
    /// <summary>
    /// InstrumentCreateModalView.xaml の相互作用ロジック
    /// </summary>
    public partial class InstrumentCreateModalView : Window
    {
        public InstrumentCreateModalView()
        {
            InitializeComponent();
            DataContext = new InstrumentCreateViewModel();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is InstrumentCreateViewModel viewModel)
            {
                viewModel.Name = name.Text;
                viewModel.SelectedCategoryId = (int)categoryComboBox.SelectedValue;

                viewModel.AddInstrument();
                MessageBox.Show("Instrument created successfully.");
                DialogResult = true;
                Close();
            }
        }
    }
}
