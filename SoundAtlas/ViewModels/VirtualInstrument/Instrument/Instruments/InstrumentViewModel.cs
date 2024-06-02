using Microsoft.Toolkit.Mvvm.Input;
using SoundAtlas.Models;
using SoundAtlas.Views.VirtualInstrument.Instrument.Instruments;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using Microsoft.Win32;
using System.Text;
using System.IO;

namespace SoundAtlas.ViewModels.VirtualInstrument.Instrument.Instruments
{
    public class InstrumentViewModel
    {
        private readonly DatabaseService _databaseService;
        public ObservableCollection<InstrumentItemViewModel> Instruments { get; private set; }
        private ObservableCollection<InstrumentItemViewModel> _allInstruments; // すべてのカテゴリーを保持するコレクション

        public string? SearchText { get; set; }
        public string? SelectedSearchColumn { get; set; }
        public ICommand ShowInstrumentUpdateCommand { get; private set; }
        public ICommand? DeleteSelectedInstrumentsCommand { get; private set; }

        public InstrumentViewModel()
        {
            _databaseService = new DatabaseService();
            Instruments = new ObservableCollection<InstrumentItemViewModel>();
            _allInstruments = new ObservableCollection<InstrumentItemViewModel>();
            Application.Current.Dispatcher.Invoke(() => {
                LoadInstruments();
            });
            ShowInstrumentUpdateCommand = new RelayCommand<InstrumentItemViewModel>(ShowInstrumentUpdate);
        }

        private void OnDataChanged(object sender, EventArgs e)
        {
            // データベースの変更があった場合に単語リストを再読み込み
            Application.Current.Dispatcher.Invoke(() => {
                LoadInstruments();
            });
        }

        public void LoadInstruments()
        {
            Instruments.Clear(); // 既存のコレクションをクリアする
            _allInstruments.Clear(); // 既存のコレクションをクリアする

            // データベースからデータを取得
            var InstrumentList = _databaseService.GetAllEntitiesIncluding<InstrumentModel, InstrumentCategoryModel>(instrument => instrument.InstrumentCategory).Select(Instrument => new InstrumentItemViewModel
            {
                InstrumentId = Instrument.InstrumentId,
                Name = Instrument.Name,
                Classification1 = Instrument.InstrumentCategory.Classification1,
                Classification2 = Instrument.InstrumentCategory.Classification2,
                Classification3 = Instrument.InstrumentCategory.Classification3,
                Classification4 = Instrument.InstrumentCategory.Classification4,
                IsSelected = false
            }).ToList();

            // コレクションにデータを追加
            foreach (var Instrument in InstrumentList)
            {
                Instruments.Add(Instrument);
                _allInstruments.Add(Instrument);
            }
        }

        public void SearchInstruments()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                // 検索テキストが空の場合、全データを表示
                Instruments.Clear();
                foreach (var Instrument in _allInstruments)
                {
                    Instruments.Add(Instrument);
                }
            }
            else
            {
                IEnumerable<InstrumentItemViewModel> filteredInstruments = _allInstruments;

                if (SelectedSearchColumn == "Classification1")
                {
                    filteredInstruments = filteredInstruments.Where(w => w.Classification1 != null && w.Classification1.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
                }
                else if (SelectedSearchColumn == "Classification2")
                {
                    filteredInstruments = filteredInstruments.Where(w => w.Classification2 != null && w.Classification2.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
                }
                else if (SelectedSearchColumn == "Classification3")
                {
                    filteredInstruments = filteredInstruments.Where(w => w.Classification3 != null && w.Classification3.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
                }
                else if (SelectedSearchColumn == "Classification4")
                {
                    filteredInstruments = filteredInstruments.Where(w => w.Classification4 != null && w.Classification4.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
                }

                Instruments.Clear();
                foreach (var Instrument in filteredInstruments)
                {
                    Instruments.Add(Instrument);
                }
            }
        }

        private void ShowInstrumentUpdate(InstrumentItemViewModel? Instrument)
        {
            if (Instrument == null)
            {
                // 例: 適切なエラーメッセージを表示
                MessageBox.Show("The selected Instrument is null.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            // InstrumentDetailViewModelのインスタンスを生成し、適切なモーダルビューを開く
            var updateViewModel = new InstrumentUpdateViewModel(Instrument.InstrumentId);
            var updateModal = new InstrumentUpdateModalView(updateViewModel);
            updateModal.ShowDialog();
        }

        public void ExportCsv()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV file (*.csv)|*.csv",
                FileName = "Instruments_export.csv"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                StringBuilder csvContent = new StringBuilder();
                csvContent.AppendLine("Classification1,Classification2,Classification3,Classification4");

                foreach (var Instrument in Instruments)
                {
                    csvContent.AppendLine($"{Instrument.Classification1},{Instrument.Classification2},{Instrument.Classification3},{Instrument.Classification4}");
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
                    var Instruments = new List<InstrumentItemViewModel>();

                    foreach (var line in csvContent.Skip(1)) // ヘッダー行をスキップ
                    {
                        var columns = line.Split(',');
                        if (columns.Length == 4)
                        {
                            Instruments.Add(new InstrumentItemViewModel
                            {
                                Classification1 = columns[0],
                                Classification2 = columns[1],
                                Classification3 = columns[2],
                                Classification4 = columns[3]
                            });
                        }
                    }

                    foreach (var Instrument in Instruments)
                    {
                        if (!Instruments.Any(c => c.Classification1 == Instrument.Classification1 && c.Classification2 == Instrument.Classification2
                        && c.Classification3 == Instrument.Classification3 && c.Classification4 == Instrument.Classification4))
                        {
                            var newInstrument = new InstrumentModel
                            {
                                Name = Instrument.Name,
                                InstrumentCategoryId = ,
                            };

                            _databaseService.AddEntity(newInstrument);
                            Instruments.Add(new InstrumentItemViewModel
                            {
                                InstrumentId = Instrument.InstrumentId,
                                Name = Instrument.Name,
                                Classification1 = Instrument.Classification1,
                                Classification2 = Instrument.Classification2,
                                Classification3 = Instrument.Classification3,
                                Classification4 = Instrument.Classification4,
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

    public class InstrumentItemViewModel
    {
        public int InstrumentId { get; set; }
        public string Name { get; set; } = "";
        public string Classification1 { get; set; } = "";
        public string? Classification2 { get; set; }
        public string? Classification3 { get; set; }
        public string? Classification4 { get; set; }
        public bool IsSelected { get; set; }
    }

}