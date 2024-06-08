using System.Windows;
using SoundAtlas.ViewModels.VirtualInstrument.VirtualInstrument.VirtualInstruments;

namespace SoundAtlas.Views.VirtualInstrument.VirtualInstrument.VirtualInstruments
{
    /// <summary>
    /// VirtualInstrumentCreateModalView.xaml の相互作用ロジック
    /// </summary>
    public partial class VirtualInstrumentCreateModalView : Window
    {
        public VirtualInstrumentCreateModalView()
        {
            InitializeComponent();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = new VirtualInstrumentCreateViewModel()
            {
                Name = name.Text,
                SiteUrl = siteUrl.Text,
                Version = version.Text,
                LastUpdated = lastUpdated.SelectedDate,
                Image = image.Text,
                Memo = memo.Text
            };

            viewModel.AddVirtualInstrument();
            MessageBox.Show("VirtualInstrument create successfully.");
            DialogResult = true;
            Close();
        }
    }
}
