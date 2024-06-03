using SoundAtlas.Models;

namespace SoundAtlas.ViewModels.VirtualInstrument.Instrument.Instruments
{
    public class InstrumentUpdateViewModel
    {
        private readonly DatabaseService _databaseService;
        public InstrumentUpdateItemViewModel? InstrumentUpdate { get; private set; }


        public InstrumentUpdateViewModel(int instrumentId)
        {
            _databaseService = new DatabaseService();
            LoadInstrumentById(instrumentId);
        }

        private void LoadInstrumentById(int InstrumentId)
        {
            var Instrument = _databaseService.GetEntityById<InstrumentModel>(InstrumentId);
            if (Instrument != null)
            {
                InstrumentUpdate = new InstrumentUpdateItemViewModel
                {
                    InstrumentId = Instrument.InstrumentId,
                    Name = Instrument.Name,
                };
            }
        }
        public void UpdateInstrumentDetail()
        {
            if (InstrumentUpdate != null)
            {
                _databaseService.UpdateEntity(new InstrumentModel
                {
                    InstrumentId = InstrumentUpdate.InstrumentId,
                    Name = InstrumentUpdate.Name
                });
            }
        }
    }
    public class InstrumentUpdateItemViewModel
    {
        public int InstrumentId { get; set; }
        public string Name { get; set; } = "";
    }
}
