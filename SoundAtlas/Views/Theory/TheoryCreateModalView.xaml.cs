using SoundAtlas.ViewModels.Theory;
using System.Windows;

namespace SoundAtlas.Views.Theory
{
    /// <summary>
    /// TheoryCreateModalView.xaml の相互作用ロジック
    /// </summary>
    public partial class TheoryCreateModalView : Window
    {
        public TheoryCreateModalView()
        {
            InitializeComponent();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = new TheoryCreateViewModel()
            {
                Theory = txtTheory.Text,
                MelodyFlg = checkBoxMelodyFlg.IsChecked ?? false,
                ChordFlg = checkBoxChordFlg.IsChecked ?? false,
                RhythmFlg = checkBoxRhythmFlg.IsChecked ?? false,
                Abstract = txtAbstract.Text,
                Detail = txtDetail.Text
            };

            viewModel.AddTheory();
            MessageBox.Show("Theory create successfully.");
            DialogResult = true;
            Close();
        }
    }
}
