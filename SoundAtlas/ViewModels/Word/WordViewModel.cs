using Microsoft.Toolkit.Mvvm.Input;
using SoundAtlas.Models;
using SoundAtlas.Views.Word;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using Microsoft.Win32;
using System.Text;
using System.IO;

namespace SoundAtlas.ViewModels.Word
{
    public class WordViewModel
    {
        private readonly DatabaseService _databaseService;
        public ObservableCollection<WordItemViewModel> Words { get; private set; }
        private ObservableCollection<WordItemViewModel> _allWords; // すべてのワードを保持するコレクション

        public string? SearchText { get; set; }
        public string? SelectedSearchColumn { get; set; }
        public ICommand ShowWordUpdateCommand { get; private set; }
        public ICommand? DeleteSelectedWordsCommand { get; private set; }

        public WordViewModel()
        {
            _databaseService = new DatabaseService();
            Words = new ObservableCollection<WordItemViewModel>();
            _allWords = new ObservableCollection<WordItemViewModel>();
            Application.Current.Dispatcher.Invoke(() => {
                LoadWords();
            });
            ShowWordUpdateCommand = new RelayCommand<WordItemViewModel>(ShowWordUpdate);
        }

        private void OnDataChanged(object sender, EventArgs e)
        {
            // データベースの変更があった場合に単語リストを再読み込み
            Application.Current.Dispatcher.Invoke(() => {
                LoadWords();
            });
        }

        public void LoadWords()
        {
            Words.Clear(); // 既存のコレクションをクリアする
            _allWords.Clear(); // 既存のコレクションをクリアする

            // データベースからデータを取得
            var wordList = _databaseService.GetAllEntities<WordModel>().Select(word => new WordItemViewModel
            {
                WordId = word.WordId,
                Name = word.Name,
                Abstract = word.Abstract,
                Detail = word.Detail,
                IsSelected = false
            }).ToList();

            // コレクションにデータを追加
            foreach (var word in wordList)
            {
                Words.Add(word);
                _allWords.Add(word);
            }
        }

        public void SearchWords()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                // 検索テキストが空の場合、全データを表示
                Words.Clear();
                foreach (var word in _allWords)
                {
                    Words.Add(word);
                }
            }
            else
            {
                IEnumerable<WordItemViewModel> filteredWords = _allWords;

                if (SelectedSearchColumn == "Word")
                {
                    filteredWords = filteredWords.Where(w => w.Name != null && w.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
                }
                else if (SelectedSearchColumn == "Abstract")
                {
                    filteredWords = filteredWords.Where(w => w.Abstract != null && w.Abstract.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
                }

                Words.Clear();
                foreach (var word in filteredWords)
                {
                    Words.Add(word);
                }
            }
        }

        private void ShowWordUpdate(WordItemViewModel? word)
        {
            if (word == null)
            {
                // 例: 適切なエラーメッセージを表示
                MessageBox.Show("The selected word is null.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            // WordDetailViewModelのインスタンスを生成し、適切なモーダルビューを開く
            var updateViewModel = new WordUpdateViewModel(word.WordId);
            var updateModal = new WordUpdateModalView(updateViewModel);
            updateModal.ShowDialog();
        }

        public void ExportCsv()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV file (*.csv)|*.csv",
                FileName = "words_export.csv"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                StringBuilder csvContent = new StringBuilder();
                csvContent.AppendLine("Name,Abstract,Detail");

                foreach (var word in Words)
                {
                    csvContent.AppendLine($"{word.Name},{word.Abstract},{word.Detail}");
                }

                File.WriteAllText(saveFileDialog.FileName, csvContent.ToString());
                MessageBox.Show("CSV Export successful", "Export successful", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void ImportCsv()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "CSV file (*.csv)|*.csv"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    var csvContent = File.ReadAllLines(openFileDialog.FileName);
                    var words = new List<WordItemViewModel>();

                    foreach (var line in csvContent.Skip(1)) // ヘッダー行をスキップ
                    {
                        var columns = line.Split(',');
                        if (columns.Length == 3)
                        {
                            words.Add(new WordItemViewModel
                            {
                                Name = columns[0],
                                Abstract = columns[1],
                                Detail = columns[2]
                            });
                        }
                    }

                    foreach (var word in words)
                    {
                        if (!Words.Any(w => w.Name == word.Name && w.Abstract == word.Abstract && w.Detail == word.Detail))
                        {
                            var newWord = new WordModel
                            {
                                Name = word.Name,
                                Abstract = word.Abstract,
                                Detail = word.Detail
                            };

                            _databaseService.AddEntity(newWord);
                            Words.Add(new WordItemViewModel
                            {
                                WordId = newWord.WordId,
                                Name = newWord.Name,
                                Abstract = newWord.Abstract,
                                Detail = newWord.Detail,
                                IsSelected = false
                            });
                        }
                    }

                    MessageBox.Show("CSVファイルのインポートが完了しました。", "インポート成功", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"CSVファイルのインポートに失敗しました: {ex.Message}", "インポート失敗", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }

    public class WordItemViewModel
    {
        public int WordId { get; set; }
        public string Name { get; set; } = "";
        public string? Abstract { get; set; }
        public string? Detail { get; set; }
        public bool IsSelected { get; set; }
    }

}