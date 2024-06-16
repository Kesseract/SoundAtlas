using SoundAtlas.Models;
using SoundAtlas.ViewModels.VirtualInstrument.VirtualInstrument.Parameters;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SoundAtlas.Views.VirtualInstrument.VirtualInstrument.Parameters
{
    /// <summary>
    /// ParameterEditModalView.xaml の相互作用ロジック
    /// </summary>
    public partial class ParameterEditModalView : Window
    {
        private ParameterViewModel _viewModel;

        public ParameterEditModalView(int presetId)
        {
            InitializeComponent();
            _viewModel = new ParameterViewModel(presetId);
            DataContext = _viewModel;
        }

        private void AddParameter(object sender, RoutedEventArgs e)
        {
            // TextBoxからテキストを取得
            var paramName = parameterName.Text;
            var paramValue = parameterValue.Text;

            // パラメータが両方とも入力されていればViewModelに追加
            if (!string.IsNullOrWhiteSpace(paramName) && !string.IsNullOrWhiteSpace(paramValue))
            {
                _viewModel.AddParameter(paramName, paramValue);
                parameterName.Clear();
                parameterValue.Clear();
            }
            else
            {
                MessageBox.Show("Both parameter name and value must be provided.");
            }
        }

        private void RemoveParameter(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var parameter = button?.DataContext as ParameterItemViewModel;
            if (parameter != null)
            {
                _viewModel.RemoveParameter(parameter);
            }
        }

        private void SaveParameters(object sender, RoutedEventArgs e)
        {
            _viewModel.SaveParameters();
            this.DialogResult = true; // ダイアログの結果を設定
            this.Close(); // ダイアログを閉じる
        }
    }
}
