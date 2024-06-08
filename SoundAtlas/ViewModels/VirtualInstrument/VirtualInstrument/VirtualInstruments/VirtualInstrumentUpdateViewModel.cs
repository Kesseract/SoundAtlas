using SoundAtlas.Models;

namespace SoundAtlas.ViewModels.VirtualInstrument.VirtualInstrument.VirtualInstruments
{
    public class VirtualInstrumentUpdateViewModel
    {
        private readonly DatabaseService _databaseService;
        public VirtualInstrumentUpdateItemViewModel? VirtualInstrumentUpdate { get; private set; }


        public VirtualInstrumentUpdateViewModel(int virtualInstrumentId)
        {
            _databaseService = new DatabaseService();
            VirtualInstrumentById(virtualInstrumentId);
        }

        private void VirtualInstrumentById(int virtualInstrumentId)
        {
            var virtualInstrument = _databaseService.GetEntityById<VirtualInstrumentModel>(virtualInstrumentId);
            if (virtualInstrument != null)
            {
                VirtualInstrumentUpdate = new VirtualInstrumentUpdateItemViewModel
                {
                    VirtualInstrumentId = virtualInstrument.VirtualInstrumentId,
                    Name = virtualInstrument.Name,
                    SiteUrl = virtualInstrument.SiteUrl,
                    Version = virtualInstrument.Version,
                    LastUpdated = virtualInstrument.LastUpdated,
                    Image = virtualInstrument.Image,
                    Memo = virtualInstrument.Memo,
                };
            }
        }
        public void UpdateVirtualInstrumentDetail()
        {
            if (VirtualInstrumentUpdate != null)
            {
                _databaseService.UpdateEntity(new VirtualInstrumentModel
                {
                    VirtualInstrumentId = VirtualInstrumentUpdate.VirtualInstrumentId,
                    Name = VirtualInstrumentUpdate.Name,
                    SiteUrl = VirtualInstrumentUpdate.SiteUrl,
                    Version = VirtualInstrumentUpdate.Version,
                    LastUpdated = VirtualInstrumentUpdate.LastUpdated,
                    Image = VirtualInstrumentUpdate.Image,
                    Memo = VirtualInstrumentUpdate.Memo,
                });
            }
        }
    }
    public class VirtualInstrumentUpdateItemViewModel
    {
        public int VirtualInstrumentId { get; set; }
        public string Name { get; set; } = "";
        public string? SiteUrl { get; set; }
        public string? Version { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string? Image { get; set; }
        public string? Memo { get; set; }
        public bool IsSelected { get; set; }
    }
}
