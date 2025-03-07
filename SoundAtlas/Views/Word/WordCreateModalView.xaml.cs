﻿using System.Windows;
using SoundAtlas.ViewModels.Word;

namespace SoundAtlas.Views
{
    /// <summary>
    /// AddWordModalView.xaml の相互作用ロジック
    /// </summary>
    public partial class WordCreateModalView : Window
    {
        public WordCreateModalView()
        {
            InitializeComponent();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = new WordCreateViewModel()
            {
                Word = txtWord.Text,
                Abstract = txtAbstract.Text,
                Details = txtDetails.Text
            };

            viewModel.AddWord();
            MessageBox.Show("Word create successfully.");
            DialogResult = true;
            Close();
        }
    }
}
