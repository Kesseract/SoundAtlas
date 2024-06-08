using System.Windows;
using SoundAtlas.ViewModels.VirtualInstrument.Instrument.Categories;

namespace SoundAtlas.Views.VirtualInstrument.Instrument.Categories
{
    /// <summary>
    /// CategoryCreateModalView.xaml の相互作用ロジック
    /// </summary>
    public partial class CategoryCreateModalView : Window
    {
        public CategoryCreateModalView()
        {
            InitializeComponent();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = new CategoryCreateViewModel()
            {
                Classification1 = classification1.Text,
                Classification2 = classification2.Text,
                Classification3 = classification3.Text,
                Classification4 = classification4.Text,
            };

            viewModel.AddCategory();
            MessageBox.Show("Category create successfully.");
            DialogResult = true;
            Close();
        }
    }
}
