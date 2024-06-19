using Microsoft.EntityFrameworkCore;
using SoundAtlas.Models;
using System.IO;

public class MyMusicAppContext : DbContext
{
    public DbSet<InstrumentModel> Instruments { get; set; }
    public DbSet<InstrumentCategoryModel> InstrumentCategories { get; set; }
    public DbSet<InstrumentWordLinkageModel> InstrumentWordLinkages { get; set; }
    public DbSet<TheoryModel> Theories { get; set; }
    public DbSet<TheoryWordLinkageModel> TheoryWordLinkages { get; set; }
    public DbSet<VirtualInstrumentModel> VirtualInstruments { get; set; }
    public DbSet<VirtualInstrumentPresetModel> VirtualInstrumentPresets { get; set; }
    public DbSet<VirtualInstrumentParameterModel> VirtualInstrumentParameters { get; set; }
    public DbSet<WordModel> Words { get; set; }

    // コマンド
    // Add-Migration [MigrationName]
    // Update-Databse


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var basePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\"));
        var dbPath = Path.Combine(basePath, "Database", "MyMusicApp.db");
        Console.WriteLine(dbPath);
        // C:\Users\hikar\cs\SoundAtlas\SoundAtlas\bin\Debug\net8.0-windows\Database\MyMusicApp.db
        optionsBuilder.UseSqlite($"Data Source={dbPath}");
    }
}