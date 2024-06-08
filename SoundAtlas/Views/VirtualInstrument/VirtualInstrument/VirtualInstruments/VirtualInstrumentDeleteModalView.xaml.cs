using System.Windows;
using SoundAtlas.ViewModels.VirtualInstrument.VirtualInstrument.VirtualInstruments;

namespace SoundAtlas.Views.VirtualInstrument.VirtualInstrument.VirtualInstruments
{
    /// <summary>
    /// VirtualInstrumentDeleteModalView.xaml の相互作用ロジック
    /// </summary>
    public partial class VirtualInstrumentDeleteModalView : Window
    {
        public VirtualInstrumentDeleteModalView()
        {
            InitializeComponent();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as VirtualInstrumentDeleteViewModel;
            if (viewModel != null)
            {
                viewModel.DeleteVirtualInstruments();  // ViewModelの削除メソッドを呼び出す
                DialogResult = true;
                Close();  // モーダルウィンドウを閉じる
            }
        }
    }
}
