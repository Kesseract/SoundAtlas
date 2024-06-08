using SoundAtlas.Models;

namespace SoundAtlas.ViewModels.VirtualInstrument.VirtualInstrument.VirtualInstruments
{
    public class VirtualInstrumentCreateViewModel
    {
        private readonly DatabaseService _databaseService;
        public string Name { get; set; } = "";
        public string? SiteUrl { get; set; }
        public string? Version { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string? Image { get; set; }
        public string? Memo { get; set; }


        public VirtualInstrumentCreateViewModel()
        {
            _databaseService = new DatabaseService();
        }

        public void AddVirtualInstrument()
        {
            var virtualInstrument = new VirtualInstrumentModel
            {
                Name = Name,
                SiteUrl = SiteUrl ?? string.Empty,
                Version = Version ?? string.Empty,
                LastUpdated = LastUpdated,
                Image = Image ?? string.Empty,
                Memo = Memo ?? string.Empty,
            };

            _databaseService.AddEntity(virtualInstrument);
        }
    }
}
