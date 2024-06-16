using Microsoft.Toolkit.Mvvm.ComponentModel;
using SoundAtlas.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SoundAtlas.ViewModels.VirtualInstrument.VirtualInstrument.Parameters
{
    public class ParameterViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;
        private int _currentPresetId;

        public ObservableCollection<ParameterItemViewModel> Parameters { get; private set; }
        public string PresetName { get; private set; }

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
    }

    public class ParameterItemViewModel : ObservableObject
    {
        private string _name;
        private string _value;

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
