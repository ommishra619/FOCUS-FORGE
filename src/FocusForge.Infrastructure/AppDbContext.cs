using Microsoft.EntityFrameworkCore;
using System.IO;

namespace FocusForge.Infrastructure;

public sealed class AppDbContext : DbContext
{
    public DbSet<StoredTask> Tasks { get; set; } = null!;
    public DbSet<ScheduleBlock> Schedule { get; set; } = null!;
    public DbSet<StoredWeightEntry> WeightEntries { get; set; } = null!;
    public DbSet<AgentSettings> AgentSettings { get; set; } = null!;
    public DbSet<PomodoroSession> PomodoroSessions { get; set; } = null!;
    public DbSet<Habit> Habits { get; set; } = null!;
    public DbSet<HabitLog> HabitLogs { get; set; } = null!;

    public AppDbContext()
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FocusForge");
        Directory.CreateDirectory(folder);
        var dbPath = Path.Combine(folder, "focusforge.db");
        optionsBuilder.UseSqlite($"Data Source={dbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StoredTask>()
            .HasMany(t => t.Checklist)
            .WithOne();
    }
}
