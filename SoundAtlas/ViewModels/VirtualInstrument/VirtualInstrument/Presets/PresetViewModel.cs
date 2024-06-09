using Microsoft.Toolkit.Mvvm.Input;
using SoundAtlas.Models;
using SoundAtlas.Views.VirtualInstrument.VirtualInstrument.Presets;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using Microsoft.Win32;
using System.Text;
using System.IO;

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
                FileName = "presets_export.csv"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                StringBuilder csvContent = new StringBuilder();
                csvContent.AppendLine("Name,VirtualInstrumentName,InstrumentName,Rate,MelodyFlg,ChordFlg,BassFlg,ChordRhythmFlg,PercussionFlg");

                foreach (var Preset in Presets)
                {
                    csvContent.AppendLine($"{Preset.PresetName},{Preset.VirtualInstrumentName},{Preset.InstrumentName},{Preset.Rate}," +
                        $"{Preset.MelodyFlg},{Preset.ChordFlg},{Preset.BassFlg},{Preset.ChordRhythmFlg},{Preset.PercussionFlg}");
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
                    // 事前に必要なデータをロード
                    var existingPresets = _databaseService.GetAllEntities<VirtualInstrumentPresetModel>().ToList();
                    var allVirtualInstruments = _databaseService.GetAllEntities<VirtualInstrumentModel>().ToDictionary(v => v.Name, v => v);
                    var allInstruments = _databaseService.GetAllEntities<InstrumentModel>().ToDictionary(i => i.Name, i => i);

                    foreach (var line in csvContent.Skip(1)) // ヘッダー行をスキップ
                    {
                        var columns = line.Split(',');
                        if (columns.Length == 9)
                        {
                            // 名前で検索し、一致するIDを取得
                            var virtualInstrument = allVirtualInstruments.GetValueOrDefault(columns[1]);
                            var instrument = allInstruments.GetValueOrDefault(columns[2]);

                            if (virtualInstrument != null && instrument != null &&
                                bool.TryParse(columns[4], out bool melodyFlg) &&
                                bool.TryParse(columns[5], out bool chordFlg) &&
                                bool.TryParse(columns[6], out bool bassFlg) &&
                                bool.TryParse(columns[7], out bool chordRhythmFlg) &&
                                bool.TryParse(columns[8], out bool percussionFlg))
                            {
                                // 既存データとの重複チェック
                                if (!existingPresets.Any(p => p.Name == columns[0] && p.VirtualInstrumentId == virtualInstrument.VirtualInstrumentId && p.InstrumentId == instrument.InstrumentId
                                && p.Rate == int.Parse(columns[3]) && p.MelodyFlg == melodyFlg && p.ChordFlg == chordFlg 
                                && p.BassFlg == bassFlg && p.ChordRhythmFlg == chordRhythmFlg && p.PercussionFlg == percussionFlg))
                                {
                                    var newPreset = new VirtualInstrumentPresetModel
                                    {
                                        Name = columns[0],
                                        VirtualInstrumentId = virtualInstrument.VirtualInstrumentId,
                                        InstrumentId = instrument.InstrumentId,
                                        Rate = int.Parse(columns[3]),
                                        MelodyFlg = melodyFlg,
                                        ChordFlg = chordFlg,
                                        BassFlg = bassFlg,
                                        ChordRhythmFlg = chordRhythmFlg,
                                        PercussionFlg = percussionFlg
                                    };
                                    _databaseService.AddEntity(newPreset);
                                    existingPresets.Add(newPreset); // リストに追加して重複チェック用に保持
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