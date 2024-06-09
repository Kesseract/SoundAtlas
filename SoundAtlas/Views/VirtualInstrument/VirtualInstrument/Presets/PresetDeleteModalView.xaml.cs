using System.Windows;
using SoundAtlas.ViewModels.VirtualInstrument.VirtualInstrument.Presets;

namespace SoundAtlas.Views.VirtualInstrument.VirtualInstrument.Presets
{
    /// <summary>
    /// PresetDeleteModalView.xaml の相互作用ロジック
    /// </summary>
    public partial class PresetDeleteModalView : Window
    {
        public PresetDeleteModalView()
        {
            InitializeComponent();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as PresetDeleteViewModel;
            if (viewModel != null)
            {
                viewModel.DeletePresets();  // ViewModelの削除メソッドを呼び出す
                DialogResult = true;
                Close();  // モーダルウィンドウを閉じる
            }
        }
    }
}
