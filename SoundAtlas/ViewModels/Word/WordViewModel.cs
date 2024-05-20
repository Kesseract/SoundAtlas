using Microsoft.Toolkit.Mvvm.Input;
using SoundAtlas.Models;
using SoundAtlas.ViewModels.Word;
using SoundAtlas.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using Microsoft.Win32;
using System.Text;
using System.IO;

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
        var wordUpdateViewModel = new WordUpdateViewModel(word.WordId);
        var wordUpdateModal = new WordUpdateModalView(wordUpdateViewModel);
        wordUpdateModal.ShowDialog();
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
            MessageBox.Show("CSVファイルのエクスポートが完了しました。", "エクスポート成功", MessageBoxButton.OK, MessageBoxImage.Information);
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
