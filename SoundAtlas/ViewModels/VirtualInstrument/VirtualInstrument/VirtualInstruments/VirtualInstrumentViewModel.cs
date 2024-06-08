using Microsoft.Toolkit.Mvvm.Input;
using SoundAtlas.Models;
using SoundAtlas.Views.VirtualInstrument.VirtualInstrument.VirtualInstruments;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using Microsoft.Win32;
using System.Text;
using System.IO;
using SoundAtlas.ViewModels.VirtualInstrument.VirtualInstrument.VirtualInstruments;
using System.Xml.Linq;

namespace SoundAtlas.ViewModels.VirtualInstrument.VirtualInstrument.VirtualInstruments
{
    public class VirtualInstrumentViewModel
    {
        private readonly DatabaseService _databaseService;
        public ObservableCollection<VirtualInstrumentItemViewModel> VirtualInstruments { get; private set; }
        private ObservableCollection<VirtualInstrumentItemViewModel> _allVirtualInstruments; // すべての楽器を保持するコレクション

        public string? SearchText { get; set; }
        public string? SelectedSearchColumn { get; set; }
        public ICommand ShowVirtualInstrumentUpdateCommand { get; private set; }
        public ICommand? DeleteSelectedVirtualInstrumentsCommand { get; private set; }

        public VirtualInstrumentViewModel()
        {
            _databaseService = new DatabaseService();
            VirtualInstruments = new ObservableCollection<VirtualInstrumentItemViewModel>();
            _allVirtualInstruments = new ObservableCollection<VirtualInstrumentItemViewModel>();
            Application.Current.Dispatcher.Invoke(() => {
                LoadVirtualInstruments();
            });
            ShowVirtualInstrumentUpdateCommand = new RelayCommand<VirtualInstrumentItemViewModel>(ShowVirtualInstrumentUpdate);
        }

        private void OnDataChanged(object sender, EventArgs e)
        {
            // データベースの変更があった場合に単語リストを再読み込み
            Application.Current.Dispatcher.Invoke(() => {
                LoadVirtualInstruments();
            });
        }

        public void LoadVirtualInstruments()
        {
            VirtualInstruments.Clear(); // 既存のコレクションをクリアする
            _allVirtualInstruments.Clear(); // 既存のコレクションをクリアする

            // データベースからデータを取得
            var VirtualInstrumentList = _databaseService.GetAllEntities<VirtualInstrumentModel>().Select(virtualInstrument => new VirtualInstrumentItemViewModel
            {
                VirtualInstrumentId = virtualInstrument.VirtualInstrumentId,
                Name = virtualInstrument.Name,
                SiteUrl = virtualInstrument.SiteUrl,
                Version = virtualInstrument.Version,
                LastUpdated = virtualInstrument.LastUpdated,
                Image = virtualInstrument.Image,
                Memo = virtualInstrument.Memo,
                IsSelected = false
            }).ToList();

            // コレクションにデータを追加
            foreach (var VirtualInstrument in VirtualInstrumentList)
            {
                VirtualInstruments.Add(VirtualInstrument);
                _allVirtualInstruments.Add(VirtualInstrument);
            }
        }

        public void SearchVirtualInstruments()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                // 検索テキストが空の場合、全データを表示
                VirtualInstruments.Clear();
                foreach (var VirtualInstrument in _allVirtualInstruments)
                {
                    VirtualInstruments.Add(VirtualInstrument);
                }
            }
            else
            {
                IEnumerable<VirtualInstrumentItemViewModel> filteredVirtualInstruments = _allVirtualInstruments;

                if (SelectedSearchColumn == "Name")
                {
                    filteredVirtualInstruments = filteredVirtualInstruments.Where(w => w.Name != null && w.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
                }

                VirtualInstruments.Clear();
                foreach (var VirtualInstrument in filteredVirtualInstruments)
                {
                    VirtualInstruments.Add(VirtualInstrument);
                }
            }
        }

        private void ShowVirtualInstrumentUpdate(VirtualInstrumentItemViewModel? VirtualInstrument)
        {
            if (VirtualInstrument == null)
            {
                // 例: 適切なエラーメッセージを表示
                MessageBox.Show("The selected VirtualInstrument is null.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            // VirtualInstrumentDetailViewModelのインスタンスを生成し、適切なモーダルビューを開く
            var updateViewModel = new VirtualInstrumentUpdateViewModel(VirtualInstrument.VirtualInstrumentId);
            var updateModal = new VirtualInstrumentUpdateModalView(updateViewModel);
            updateModal.ShowDialog();
        }

        public void ExportCsv()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV file (*.csv)|*.csv",
                FileName = "virtual_instrument_export.csv"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                StringBuilder csvContent = new StringBuilder();
                csvContent.AppendLine("Name,SiteUrl,Version,LastUpdated,Image,Memo");

                foreach (var virtualinstrument in VirtualInstruments)
                {
                    csvContent.AppendLine($"{virtualinstrument.Name},{virtualinstrument.SiteUrl},{virtualinstrument.Version},{virtualinstrument.LastUpdated},{virtualinstrument.Image},{virtualinstrument.Memo}");
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
                    var virtualInstruments = new List<VirtualInstrumentItemViewModel>();

                    foreach (var line in csvContent.Skip(1)) // ヘッダー行をスキップ
                    {
                        var columns = line.Split(',');
                        if (columns.Length == 6)
                        {
                            DateTime lastUpdated;
                            if (!DateTime.TryParse(columns[3], out lastUpdated))
                            {
                                MessageBox.Show($"Invalid date format in line: {line}");
                                continue; // Skip this line
                            }

                            virtualInstruments.Add(new VirtualInstrumentItemViewModel
                            {
                                Name = columns[0],
                                SiteUrl = columns[1],
                                Version = columns[2],
                                LastUpdated = lastUpdated,
                                Image = columns[4],
                                Memo = columns[5]
                            });
                        }
                    }

                    foreach (var virtualInstrument in virtualInstruments)
                    {
                        if (!VirtualInstruments.Any(c => c.Name == virtualInstrument.Name))
                        {
                            var newInstrument = new VirtualInstrumentModel
                            {
                                Name = virtualInstrument.Name,
                                SiteUrl = virtualInstrument.SiteUrl,
                                Version = virtualInstrument.Version,
                                LastUpdated = virtualInstrument.LastUpdated,
                                Image = virtualInstrument.Image,
                                Memo = virtualInstrument.Memo,
                            };

                            _databaseService.AddEntity(newInstrument);
                            VirtualInstruments.Add(virtualInstrument);
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

    public class VirtualInstrumentItemViewModel
    {
        public int VirtualInstrumentId { get; set; }
        public string Name { get; set; } = "";
        public string? SiteUrl { get; set; }
        public string? Version { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string? Image { get; set; }
        public string? Memo { get; set; }
        public bool IsSelected { get; set; }
    }

}