using System.Windows;
using SoundAtlas.ViewModels.VirtualInstrument.Instrument.Instruments;

namespace SoundAtlas.Views.VirtualInstrument.Instrument.Instruments
{
    /// <summary>
    /// InstrumentDeleteModalView.xaml の相互作用ロジック
    /// </summary>
    public partial class InstrumentDeleteModalView : Window
    {
        public InstrumentDeleteModalView()
        {
            InitializeComponent();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as InstrumentDeleteViewModel;
            if (viewModel != null)
            {
                viewModel.DeleteInstruments();  // ViewModelの削除メソッドを呼び出す
                DialogResult = true;
                Close();  // モーダルウィンドウを閉じる
            }
        }
    }
}
