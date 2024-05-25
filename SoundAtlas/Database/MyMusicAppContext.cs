using Microsoft.EntityFrameworkCore;
using SoundAtlas.Models;

public class MyMusicAppContext : DbContext
{
    public DbSet<InstrumentModel> Instruments { get; set; }
    public DbSet<InstrumentCategoryModel> InstrumentCategories { get; set; }
    public DbSet<InstrumentWordLinkageModel> InstrumentWordLinkages { get; set; }
    public DbSet<TheoryModel> Theories { get; set; }
    public DbSet<TheoryWordLinkageModel> TheoryWordLinkages { get; set; }
    public DbSet<VirtualInstrumentModel> VirtualInstruments { get; set; }
    public DbSet<VirtualInstrumentDetailModel> VirtualInstrumentDetails { get; set; }
    public DbSet<VirtualInstrumentParameterModel> VirtualInstrumentParameters { get; set; }
    public DbSet<VirtualInstrumentPresetModel> VirtualInstrumentPresets { get; set; }
    public DbSet<WordModel> Words { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(@"Data Source=C:\Users\hikar\cs\SoundAtlas\SoundAtlas\Database\MyMusicApp.db");
    }
}