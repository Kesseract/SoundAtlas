using SoundAtlas.Models;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using static SoundAtlas.ViewModels.VirtualInstrument.VirtualInstrument.Presets.PresetCreateViewModel;

namespace SoundAtlas.ViewModels.VirtualInstrument.VirtualInstrument.Presets
{
    public class PresetUpdateViewModel
    {
        private readonly DatabaseService _databaseService;
        public PresetUpdateItemViewModel? PresetUpdate { get; private set; }
        public ObservableCollection<VirtualInstrumentCreateViewModel> VirtualInstruments { get; set; }

        public ObservableCollection<InstrumentCreateViewModel> Instruments { get; set; }

        public PresetUpdateViewModel(int presetId)
        {
            _databaseService = new DatabaseService();
            VirtualInstruments = new ObservableCollection<VirtualInstrumentCreateViewModel>();
            Instruments = new ObservableCollection<InstrumentCreateViewModel>();
            LoadVirtualInstruments();
            LoadInstruments();
            LoadPresetById(presetId);
        }

        private void LoadVirtualInstruments()
        {
            VirtualInstruments.Clear();
            var virtualInstrumentList = _databaseService.GetAllEntities<VirtualInstrumentModel>().Select(virtualInstrument => new VirtualInstrumentCreateViewModel
            {
                VirtualInstrumentId = virtualInstrument.VirtualInstrumentId,
                Name = virtualInstrument.Name
            }).ToList();

            // コレクションにデータを追加
            foreach (var virtualInstrument in virtualInstrumentList)
            {
                VirtualInstruments.Add(virtualInstrument);
            }
        }

        private void LoadInstruments()
        {
            Instruments.Clear();
            var instrumentList = _databaseService.GetAllEntities<InstrumentModel>().Select(instrument => new InstrumentCreateViewModel
            {
                InstrumentId = instrument.InstrumentId,
                Name = instrument.Name
            }).ToList();

            // コレクションにデータを追加
            foreach (var instrument in instrumentList)
            {
                Instruments.Add(instrument);
            }
        }

        private void LoadPresetById(int presetId)
        {
            var preset = _databaseService.GetEntityById<VirtualInstrumentPresetModel>(presetId);
            if (preset != null)
            {
                PresetUpdate = new PresetUpdateItemViewModel
                {
                    PresetId = preset.VirtualInstrumentPresetId,
                    PresetName = preset.Name,
                    SelectedVirtualInstrumentId = preset.VirtualInstrumentId,
                    SelectedInstrumentId = preset.InstrumentId,
                    Rate = preset.Rate,
                    MelodyFlg = preset.MelodyFlg,
                    ChordFlg = preset.ChordFlg,
                    BassFlg = preset.BassFlg,
                    ChordRhythmFlg = preset.ChordRhythmFlg,
                    PercussionFlg = preset.PercussionFlg,
                };
            }
        }

        public void UpdatePresetDetail()
        {
            if (PresetUpdate != null)
            {
                var updatedPreset = new VirtualInstrumentPresetModel
                {
                    VirtualInstrumentPresetId = PresetUpdate.PresetId,
                    Name = PresetUpdate.PresetName,
                    VirtualInstrumentId = PresetUpdate.SelectedVirtualInstrumentId,
                    InstrumentId = PresetUpdate.SelectedInstrumentId,
                    Rate = PresetUpdate.Rate,
                    MelodyFlg = PresetUpdate.MelodyFlg,
                    ChordFlg = PresetUpdate.ChordFlg,
                    BassFlg = PresetUpdate.BassFlg,
                    ChordRhythmFlg = PresetUpdate.ChordRhythmFlg,
                    PercussionFlg = PresetUpdate.PercussionFlg,
                };
                _databaseService.UpdateEntity(updatedPreset);
            }
        }
    }
    public class PresetUpdateItemViewModel
    {
        public int PresetId { get; set; }
        public string PresetName { get; set; } = "";
        public int SelectedVirtualInstrumentId { get; set; }
        public int SelectedInstrumentId { get; set; }
        public int Rate { get; set; }
        public bool MelodyFlg { get; set; }
        public bool ChordFlg { get; set; }
        public bool BassFlg { get; set; }
        public bool ChordRhythmFlg { get; set; }
        public bool PercussionFlg { get; set; }
    }
}
