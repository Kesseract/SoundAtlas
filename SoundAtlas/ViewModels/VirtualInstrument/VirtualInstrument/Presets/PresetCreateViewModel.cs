using SoundAtlas.Models;
using SoundAtlas.ViewModels.VirtualInstrument.VirtualInstrument.VirtualInstruments;
using System.Collections.ObjectModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SoundAtlas.ViewModels.VirtualInstrument.VirtualInstrument.Presets
{
    public class PresetCreateViewModel
    {
        private readonly DatabaseService _databaseService;
        public string? Name { get; set; }
        public int SelectedVirtualInstrumentId { get; set; }
        public ObservableCollection<VirtualInstrumentCreateViewModel> VirtualInstruments { get; set; } = new ObservableCollection<VirtualInstrumentCreateViewModel>();
        public int SelectedInstrumentId { get; set; }
        public ObservableCollection<InstrumentCreateViewModel> Instruments { get; set; } = new ObservableCollection<InstrumentCreateViewModel>();
        public int Rate { get; set; }
        public bool MelodyFlg { get; set; } = false;
        public bool ChordFlg { get; set; } = false;
        public bool BassFlg { get; set; } = false;
        public bool ChordRhythmFlg { get; set; } = false;
        public bool PercussionFlg { get; set; } = false;


        public PresetCreateViewModel()
        {
            _databaseService = new DatabaseService();
            LoadVirtualInstruments();
            LoadInstruments();
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

            if (VirtualInstruments.Any())
            {
                SelectedVirtualInstrumentId = VirtualInstruments[0].VirtualInstrumentId;
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

            if (Instruments.Any())
            {
                SelectedInstrumentId = Instruments[0].InstrumentId;
            }
        }

        public void AddPreset()
        {
            var preset = new VirtualInstrumentPresetModel
            {
                Name = Name ?? string.Empty,
                VirtualInstrumentId = SelectedVirtualInstrumentId,
                InstrumentId = SelectedInstrumentId,
                Rate = Rate,
                MelodyFlg = MelodyFlg,
                ChordFlg = ChordFlg,
                BassFlg = BassFlg,
                ChordRhythmFlg = ChordRhythmFlg,
                PercussionFlg = PercussionFlg
            };

            _databaseService.AddEntity(preset);
        }

        public class VirtualInstrumentCreateViewModel
        {
            public int VirtualInstrumentId { get; set; }
            public string Name { get; set; } = "";
        }

        public class InstrumentCreateViewModel
        {
            public int InstrumentId { get; set; }
            public string Name { get; set; } = "";
        }
    }
}
