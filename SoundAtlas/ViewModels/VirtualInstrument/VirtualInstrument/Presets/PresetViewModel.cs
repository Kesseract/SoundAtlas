using Microsoft.Toolkit.Mvvm.Input;
using SoundAtlas.Models;
using SoundAtlas.Views.VirtualInstrument.VirtualInstrument.Presets;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using Microsoft.Win32;
using System.Text;
using System.IO;
using System.Security.Policy;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SoundAtlas.ViewModels.VirtualInstrument.VirtualInstrument.Presets
{
    public class PresetViewModel
    {
        private readonly DatabaseService _databaseService;
        public ObservableCollection<PresetItemViewModel> Presets { get; private set; }
        private ObservableCollection<PresetItemViewModel> _allPresets; // すべての楽器を保持するコレクション

        public string? SearchText { get; set; }
        public string? SelectedSearchColumn { get; set; }
        public ICommand ShowPresetUpdateCommand { get; private set; }
        public ICommand? DeleteSelectedPresetsCommand { get; private set; }

        public PresetViewModel()
        {
            _databaseService = new DatabaseService();
            Presets = new ObservableCollection<PresetItemViewModel>();
            _allPresets = new ObservableCollection<PresetItemViewModel>();
            Application.Current.Dispatcher.Invoke(() => {
                LoadPresets();
            });
            ShowPresetUpdateCommand = new RelayCommand<PresetItemViewModel>(ShowPresetUpdate);
        }

        private void OnDataChanged(object sender, EventArgs e)
        {
            // データベースの変更があった場合に単語リストを再読み込み
            Application.Current.Dispatcher.Invoke(() => {
                LoadPresets();
            });
        }

        public void LoadPresets()
        {
            Presets.Clear(); // 既存のコレクションをクリアする
            _allPresets.Clear(); // 既存のコレクションをクリアする

            // データベースからデータを取得
            var PresetList = _databaseService.GetAllEntitiesIncluding<VirtualInstrumentPresetModel>(preset => preset.VirtualInstrument, preset => preset.Instrument).Select(Preset => new PresetItemViewModel
            {
                PresetId = Preset.VirtualInstrumentPresetId,
                PresetName = Preset.Name,
                VirtualInstrumentName = Preset.VirtualInstrument.Name,
                InstrumentName = Preset.Instrument.Name,
                Rate = Preset.Rate,
                MelodyFlg = Preset.MelodyFlg,
                ChordFlg = Preset.ChordFlg,
                BassFlg = Preset.BassFlg,
                ChordRhythmFlg = Preset.ChordRhythmFlg,
                PercussionFlg = Preset.PercussionFlg,
                IsSelected = false
            }).ToList();

            // コレクションにデータを追加
            foreach (var Preset in PresetList)
            {
                Presets.Add(Preset);
                _allPresets.Add(Preset);
            }
        }

        public void SearchPresets()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                // 検索テキストが空の場合、全データを表示
                Presets.Clear();
                foreach (var Preset in _allPresets)
                {
                    Presets.Add(Preset);
                }
            }
            else
            {
                IEnumerable<PresetItemViewModel> filteredPresets = _allPresets;

                if (SelectedSearchColumn == "Name")
                {
                    filteredPresets = filteredPresets.Where(w => w.PresetName != null && w.PresetName.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
                }

                Presets.Clear();
                foreach (var Preset in filteredPresets)
                {
                    Presets.Add(Preset);
                }
            }
        }

        private void ShowPresetUpdate(PresetItemViewModel? Preset)
        {
            if (Preset == null)
            {
                // 例: 適切なエラーメッセージを表示
                MessageBox.Show("The selected Preset is null.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            // PresettDetailViewModelのインスタンスを生成し、適切なモーダルビューを開く
            var updateViewModel = new PresetUpdateViewModel(Preset.PresetId);
            var updateModal = new PresetUpdateModalView(updateViewModel);
            updateModal.ShowDialog();
        }

        public void ExportCsv()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV file (*.csv)|*.csv",
                FileName = "Presets_export.csv"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                StringBuilder csvContent = new StringBuilder();
                csvContent.AppendLine("Name");

                foreach (var Preset in Presets)
                {
                    csvContent.AppendLine($"{Preset.PresetName}");
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
                    // データベースから既存の楽器を取得
                    var existingVirtualInstruments = _databaseService.GetAllEntities<VirtualInstrumentModel>().ToList();
                    var existingInstruments = _databaseService.GetAllEntities<InstrumentModel>().ToList();

                    foreach (var line in csvContent.Skip(1)) // ヘッダー行をスキップ
                    {
                        var columns = line.Split(',');
                        if (columns.Length == 5)
                        {
                            var categoryQuery = _databaseService.GetAllEntities<InstrumentCategoryModel>().AsQueryable();

                            // 各カテゴリフィールドが存在するかどうかを確認し、存在する場合のみクエリに含める
                            if (!string.IsNullOrWhiteSpace(columns[1]))
                                categoryQuery = categoryQuery.Where(c => c.Classification1 == columns[1]);
                            if (columns.Length > 2 && !string.IsNullOrWhiteSpace(columns[2]))
                                categoryQuery = categoryQuery.Where(c => c.Classification2 == columns[2]);
                            if (columns.Length > 3 && !string.IsNullOrWhiteSpace(columns[3]))
                                categoryQuery = categoryQuery.Where(c => c.Classification3 == columns[3]);
                            if (columns.Length > 4 && !string.IsNullOrWhiteSpace(columns[4]))
                                categoryQuery = categoryQuery.Where(c => c.Classification4 == columns[4]);

                            var matchedCategory = categoryQuery.FirstOrDefault();

                            if (matchedCategory != null)
                            {
                                // 重複チェックを行う
                                if (!existingInstruments.Any(i => i.Name == columns[0] && i.InstrumentCategoryId == matchedCategory.InstrumentCategoryId))
                                {
                                    var newInstrument = new InstrumentModel
                                    {
                                        Name = columns[0],
                                        InstrumentCategoryId = matchedCategory.InstrumentCategoryId
                                    };

                                    _databaseService.AddEntity(newInstrument);
                                    // 追加された楽器を既存のリストにも追加
                                    existingInstruments.Add(newInstrument);
                                }
                            }
                        }
                    }

                    MessageBox.Show("CSVファイルのインポートが完了しました。", "インポート成功", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadPresets();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"CSVファイルのインポートに失敗しました: {ex.Message}", "インポート失敗", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }

    public class PresetItemViewModel
    {
        public int PresetId { get; set; }
        public string PresetName { get; set; } = "";
        public string VirtualInstrumentName { get; set; } = "";
        public string InstrumentName { get; set; } = "";
        public int Rate {  get; set; }
        public bool MelodyFlg { get; set; }
        public bool ChordFlg { get; set; }
        public bool BassFlg { get; set; }
        public bool ChordRhythmFlg { get; set; }
        public bool PercussionFlg { get; set; }
        public bool IsSelected { get; set; }
    }

}