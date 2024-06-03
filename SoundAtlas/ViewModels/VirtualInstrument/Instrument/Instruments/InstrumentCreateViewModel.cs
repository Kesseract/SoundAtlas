using SoundAtlas.Models;

namespace SoundAtlas.ViewModels.VirtualInstrument.Instrument.Instruments
{
    public class InstrumentCreateViewModel
    {
        private readonly DatabaseService _databaseService;
        public string? Name { get; set; }

        public InstrumentCreateViewModel()
        {
            _databaseService = new DatabaseService();
        }

        public void AddInstrument()
        {
            var instrument = new InstrumentModel
            {
                Name = Name ?? string.Empty,
            };

            _databaseService.AddEntity(instrument);
        }
    }
}
