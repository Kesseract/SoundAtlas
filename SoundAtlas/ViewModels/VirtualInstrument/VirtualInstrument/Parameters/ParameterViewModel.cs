using Microsoft.Toolkit.Mvvm.ComponentModel;
using SoundAtlas.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundAtlas.ViewModels.VirtualInstrument.VirtualInstrument.Parameters
{
    public class ParameterViewModel : ObservableObject
    {
        private ObservableCollection<VirtualInstrumentParameterModel> _parameters;
        public ObservableCollection<VirtualInstrumentParameterModel> Parameters
        {
            get => _parameters;
            set => SetProperty(ref _parameters, value);
        }

        public ParameterViewModel()
        {
            Parameters = new ObservableCollection<VirtualInstrumentParameterModel>();
        }

        public void LoadParameters(int presetId)
        {
            // データベースからpresetIdに紐づくパラメータを取得してParametersに追加
        }

        public void AddParameter(string name, string value)
        {
            Parameters.Add(new VirtualInstrumentParameterModel { Name = name, Value = value });
            // データベースにも追加するロジック
        }

        public void RemoveParameter(VirtualInstrumentParameterModel parameter)
        {
            Parameters.Remove(parameter);
            // データベースからも削除するロジック
        }
    }
}
