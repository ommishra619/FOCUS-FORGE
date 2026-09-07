using FocusForge.Core;

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FocusForge.Infrastructure;

public sealed class WorkspaceState
{
    public List<StoredTask> Tasks { get; set; } = new();
    public List<ScheduleBlock> Schedule { get; set; } = new();
    public List<StoredWeightEntry> WeightEntries { get; set; } = new();
    public List<Habit> Habits { get; set; } = new();
    public List<HabitLog> HabitLogs { get; set; } = new();

    public static async Task<WorkspaceState> LoadAsync()
    {
        using var db = new AppDbContext();
        await db.Database.EnsureCreatedAsync();
        
        return new WorkspaceState
        {
            Tasks = await db.Tasks.Include(t => t.Checklist).ToListAsync(),
            Schedule = await db.Schedule.ToListAsync(),
            WeightEntries = await db.WeightEntries.ToListAsync(),
            Habits = await db.Habits.ToListAsync(),
            HabitLogs = await db.HabitLogs.ToListAsync()
        };
    }

    public async Task SaveAsync()
    {
        using var db = new AppDbContext();
        
        // Wipe and replace to mirror the previous JSON serialization behavior
        db.Tasks.RemoveRange(db.Tasks);
        db.Schedule.RemoveRange(db.Schedule);
        db.WeightEntries.RemoveRange(db.WeightEntries);
        db.Habits.RemoveRange(db.Habits);
        db.HabitLogs.RemoveRange(db.HabitLogs);
        await db.SaveChangesAsync();
        
        db.Tasks.AddRange(Tasks);
        db.Schedule.AddRange(Schedule);
        db.WeightEntries.AddRange(WeightEntries);
        db.Habits.AddRange(Habits);
        db.HabitLogs.AddRange(HabitLogs);
        await db.SaveChangesAsync();
    }
}

public sealed class StoredTask
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string Detail { get; set; } = string.Empty;
    public string TimeLabel { get; set; } = string.Empty;
    public string Accent { get; set; } = "#7EE7C7";
    public bool IsComplete { get; set; }
    public TaskScheduleKind ScheduleKind { get; set; } = TaskScheduleKind.Unscheduled;
    public DateTimeOffset? DueAt { get; set; }
    public RecurrencePattern Recurrence { get; set; } = RecurrencePattern.None;
    public List<string> Tags { get; set; } = new();
    public List<StoredChecklistItem> Checklist { get; set; } = new();
}

public sealed class StoredChecklistItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public bool IsComplete { get; set; }
}

public sealed class StoredWeightEntry
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTimeOffset RecordedAt { get; set; } = DateTimeOffset.Now;
    public double Weight { get; set; }
    public string Note { get; set; } = string.Empty;
}

public sealed class ScheduleBlock
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Day { get; set; } = "Friday";
    public string Time { get; set; } = "11:30 AM";
    public string Title { get; set; } = "Lunch + reset";
    public string Duration { get; set; } = "30 min";
    public string Category { get; set; } = "Personal";
}

public sealed class Habit
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string AccentKey { get; set; } = "TaskWorkAccent";
}

public sealed class HabitLog
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid HabitId { get; set; }
    public DateTime Date { get; set; }
}

public sealed class PomodoroSession
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTimeOffset CompletedAt { get; set; } = DateTimeOffset.Now;
    public TimeSpan Duration { get; set; }
}
