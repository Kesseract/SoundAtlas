using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Win32;
using SoundAtlas.Models;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.IO;

namespace SoundAtlas.ViewModels.VirtualInstrument.VirtualInstrument.Parameters
{
    public class ParameterViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;
        private int _currentPresetId;

        public ObservableCollection<ParameterItemViewModel> Parameters { get; private set; }
        public string PresetName { get; private set; } = "";

        public ParameterViewModel(int presetId)
        {
            _databaseService = new DatabaseService();
            _currentPresetId = presetId;
            Parameters = new ObservableCollection<ParameterItemViewModel>();
            LoadPresetAndParameters();
        }

        private void LoadPresetAndParameters()
        {
            // プリセット情報のロード
            var preset = _databaseService.GetEntityById<VirtualInstrumentPresetModel>(_currentPresetId);
            if (preset != null)
            {
                PresetName = preset.Name;

                // パラメーター情報のロード
                var parameters = _databaseService.GetAllEntities<VirtualInstrumentParameterModel>()
                    .Where(p => p.VirtualInstrumentPresetId == _currentPresetId)
                    .Select(p => new ParameterItemViewModel
                    {
                        Name = p.Name,
                        Value = p.Value
                    }).ToList();

                foreach (var parameter in parameters)
                {
                    Parameters.Add(parameter);
                }
            }
        }

        public void AddParameter(string name, string value)
        {
            Parameters.Add(new ParameterItemViewModel { Name = name, Value = value });
        }

        public void RemoveParameter(ParameterItemViewModel parameter)
        {
            Parameters.Remove(parameter);
        }

        public void SaveParameters()
        {
            try
            {
                // Presetに関連する既存のパラメータを削除
                var deleteEntities = _databaseService.GetEntitiesByCondition<VirtualInstrumentParameterModel>(
                    p => p.VirtualInstrumentPresetId == _currentPresetId
                );

                _databaseService.DeleteEntities(deleteEntities);

                // 新しいパラメータを追加
                foreach (var parameter in Parameters)
                {
                    _databaseService.AddEntity(new VirtualInstrumentParameterModel
                    {
                        VirtualInstrumentPresetId = _currentPresetId,
                        Name = parameter.Name,
                        Value = parameter.Value
                    });
                }
                MessageBox.Show("Parameters saved successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save parameters: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void ExportCsv()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV file (*.csv)|*.csv",
                FileName = "parameters_export.csv"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                StringBuilder csvContent = new StringBuilder();
                csvContent.AppendLine("PresetName,ParameterName,ParameterValue");

                var allParameters = _databaseService.GetAllEntitiesIncluding<VirtualInstrumentParameterModel>(parameter => parameter.VirtualInstrumentPreset);
                foreach (var Parameter in allParameters)
                {
                    csvContent.AppendLine($"{Parameter.VirtualInstrumentPreset.Name},{Parameter.Name},{Parameter.Value}");
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
                    var existingPresets = _databaseService.GetAllEntities<VirtualInstrumentParameterModel>().ToList();
                    var allPresets = _databaseService.GetAllEntities<VirtualInstrumentPresetModel>().ToDictionary(v => v.Name, v => v);

                    foreach (var line in csvContent.Skip(1)) // ヘッダー行をスキップ
                    {
                        var columns = line.Split(',');
                        if (columns.Length == 3)
                        {
                            // 名前で検索し、一致するIDを取得
                            var virtualInstrumentPreset = allPresets.GetValueOrDefault(columns[0]);

                            if (virtualInstrumentPreset != null)
                            {
                                // 既存データとの重複チェック
                                if (!existingPresets.Any(p => p.VirtualInstrumentPresetId == virtualInstrumentPreset.VirtualInstrumentPresetId && p.Name == columns[1] && p.Value == columns[2]))
                                {
                                    var newPreset = new VirtualInstrumentParameterModel
                                    {
                                        VirtualInstrumentPresetId = virtualInstrumentPreset.VirtualInstrumentPresetId,
                                        Name = columns[1],
                                        Value = columns[2],
                                    };
                                    _databaseService.AddEntity(newPreset);
                                    existingPresets.Add(newPreset); // リストに追加して重複チェック用に保持
                                }
                            }
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

    public class ParameterItemViewModel : ObservableObject
    {
        private string _name = "";
        private string _value = "";

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }
    }
}
