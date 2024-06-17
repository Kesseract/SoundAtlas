using SoundAtlas.Models;
using SoundAtlas.ViewModels.VirtualInstrument.VirtualInstrument.Parameters;
using SoundAtlas.Views.VirtualInstrument.VirtualInstrument.Parameters;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SoundAtlas.Views.VirtualInstrument.VirtualInstrument.Parameters
{
    /// <summary>
    /// ParametersView.xaml の相互作用ロジック
    /// </summary>
    public partial class ParametersView : UserControl
    {
        public ParametersView()
        {
            InitializeComponent();
        }

        private void ExportCsvButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = new ParameterViewModel(0);
            if (viewModel != null)
            {
                viewModel.ExportCsv();
            }
        }

        private void ImportCsvButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = new ParameterViewModel(0);
            if (viewModel != null)
            {
                viewModel.ImportCsv();
            }
        }
    }
}
