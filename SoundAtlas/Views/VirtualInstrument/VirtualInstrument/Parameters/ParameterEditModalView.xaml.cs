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
            _viewModel = new ParameterViewModel();
            _viewModel.LoadParameters(presetId);
            DataContext = _viewModel;
        }

        private void AddParameter(object sender, RoutedEventArgs e)
        {
            _viewModel.AddParameter(parameterName.Text, parameterValue.Text);
            parameterName.Clear();
            parameterValue.Clear();
        }

        private void RemoveParameter(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var parameter = button?.CommandParameter as VirtualInstrumentParameterModel;
            if (parameter != null)
            {
                _viewModel.RemoveParameter(parameter);
            }
        }

        private void SaveParameters(object sender, RoutedEventArgs e)
        {
            // 保存ロジック
            this.DialogResult = true;
            this.Close();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Text == textBox.Tag.ToString())
            {
                textBox.Text = "";
                textBox.Foreground = Brushes.Black;
                textBox.FontStyle = FontStyles.Normal;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = textBox.Tag.ToString();
                textBox.Foreground = Brushes.LightGray;
                textBox.FontStyle = FontStyles.Italic;
            }
        }
    }
}
