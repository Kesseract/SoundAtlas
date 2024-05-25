using Microsoft.Toolkit.Mvvm.Input;
using SoundAtlas.Models;
using SoundAtlas.Views.Theory;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using Microsoft.Win32;
using System.Text;
using System.IO;

namespace SoundAtlas.ViewModels.Theory
{
    public class TheoryViewModel
    {
        private readonly DatabaseService _databaseService;

        public ObservableCollection<TheoryItemViewModel> Theories { get; private set; }
        private ObservableCollection<TheoryItemViewModel> _allTheories;

        public string? SearchText { get; set; }
        public string? SelectedSearchColumn { get; set; }
        public ICommand ShowTheoryUpdateCommand { get; private set; }
        public ICommand? DeleteSelectedTheoriesCommand { get; private set; }

        public TheoryViewModel()
        {
            _databaseService = new DatabaseService();
            Theories = new ObservableCollection<TheoryItemViewModel>();
            _allTheories = new ObservableCollection<TheoryItemViewModel>();
            Application.Current.Dispatcher.Invoke(() => {
                LoadTheories();
            });
            ShowTheoryUpdateCommand = new RelayCommand<TheoryItemViewModel>(ShowTheoryUpdate);
        }

        private void OnDataChanged(object sender, EventArgs e)
        {
            // データベースの変更があった場合に単語リストを再読み込み
            Application.Current.Dispatcher.Invoke(() => {
                LoadTheories();
            });
        }

        public void LoadTheories()
        {
            Theories.Clear(); // 既存のコレクションをクリアする
            _allTheories.Clear(); // 既存のコレクションをクリアする

            // データベースからデータを取得
            var theoryList = _databaseService.GetAllEntities<TheoryModel>().Select(theory => new TheoryItemViewModel
            {
                TheoryId = theory.TheoryId,
                Name = theory.Name,
                MelodyFlg = theory.MelodyFlg,
                ChordFlg = theory.ChordFlg,
                RhythmFlg = theory.RhythmFlg,
                Abstract = theory.Abstract,
                Detail = theory.Detail,
                IsSelected = false
            }).ToList();

            // コレクションにデータを追加
            foreach (var word in theoryList)
            {
                Theories.Add(word);
                _allTheories.Add(word);
            }
        }

        public void SearchTheories()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                // 検索テキストが空の場合、全データを表示
                Theories.Clear();
                foreach (var word in _allTheories)
                {
                    Theories.Add(word);
                }
            }
            else
            {
                IEnumerable<TheoryItemViewModel> filteredWords = _allTheories;

                if (SelectedSearchColumn == "Word")
                {
                    filteredWords = filteredWords.Where(w => w.Name != null && w.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
                }
                else if (SelectedSearchColumn == "Abstract")
                {
                    filteredWords = filteredWords.Where(w => w.Abstract != null && w.Abstract.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
                }

                Theories.Clear();
                foreach (var word in filteredWords)
                {
                    Theories.Add(word);
                }
            }
        }

        private void ShowTheoryUpdate(TheoryItemViewModel? theory)
        {
            if (theory == null)
            {
                // 例: 適切なエラーメッセージを表示
                MessageBox.Show("The selected theory is null.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            // WordDetailViewModelのインスタンスを生成し、適切なモーダルビューを開く
            var theoryUpdateViewModel = new TheoryUpdateViewModel(theory.TheoryId);
            var theoryUpdateModal = new TheoryUpdateModalView(theoryUpdateViewModel);
            theoryUpdateModal.ShowDialog();
        }

        public void ExportCsv()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV file (*.csv)|*.csv",
                FileName = "theories_export.csv"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                StringBuilder csvContent = new StringBuilder();
                csvContent.AppendLine("Name,MelodyFlg,ChordFlg,RhythmFlg,Abstract,Detail");

                foreach (var theory in Theories)
                {
                    csvContent.AppendLine($"{theory.Name},{theory.MelodyFlg},{theory.ChordFlg},{theory.RhythmFlg},{theory.Abstract},{theory.Detail}");
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
                    var theories = new List<TheoryItemViewModel>();

                    foreach (var line in csvContent.Skip(1)) // ヘッダー行をスキップ
                    {
                        var columns = line.Split(',');
                        if (columns.Length == 6)
                        {
                            bool melodyFlg, chordFlg, rhythmFlg;

                            // 各フラグをbool型に変換
                            if (bool.TryParse(columns[1], out melodyFlg) &&
                                bool.TryParse(columns[2], out chordFlg) &&
                                bool.TryParse(columns[3], out rhythmFlg))
                            {
                                theories.Add(new TheoryItemViewModel
                                {
                                    Name = columns[0],
                                    MelodyFlg = melodyFlg,
                                    ChordFlg = chordFlg,
                                    RhythmFlg = rhythmFlg,
                                    Abstract = columns[4],
                                    Detail = columns[5]
                                });
                            }
                            else
                            {
                                MessageBox.Show("CSVファイルのフラグ列に無効な値があります。", "インポートエラー", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        }
                    }

                    foreach (var theory in theories)
                    {
                        if (!Theories.Any(t => t.Name == theory.Name && t.MelodyFlg == theory.MelodyFlg && t.ChordFlg == theory.ChordFlg 
                        && t.RhythmFlg == theory.RhythmFlg && t.Abstract == theory.Abstract && t.Detail == theory.Detail))
                        {
                            var newTheory = new TheoryModel
                            {
                                Name = theory.Name,
                                MelodyFlg = theory.MelodyFlg,
                                ChordFlg = theory.ChordFlg,
                                RhythmFlg = theory.RhythmFlg,
                                Abstract = theory.Abstract,
                                Detail = theory.Detail
                            };

                            _databaseService.AddEntity(newTheory);
                            Theories.Add(new TheoryItemViewModel
                            {
                                TheoryId = newTheory.TheoryId,
                                Name = newTheory.Name,
                                MelodyFlg = newTheory.MelodyFlg,
                                ChordFlg = newTheory.ChordFlg,
                                RhythmFlg = newTheory.RhythmFlg,
                                Abstract = newTheory.Abstract,
                                Detail = newTheory.Detail,
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

    public class TheoryItemViewModel
    {
        public int TheoryId { get; set; }
        public string Name { get; set; } = "";
        public bool MelodyFlg { get; set; } = false;
        public bool ChordFlg { get; set; } = false;
        public bool RhythmFlg { get; set; } = false;
        public string? Abstract { get; set; }
        public string? Detail { get; set; }
        public bool IsSelected { get; set; }
    }
}
