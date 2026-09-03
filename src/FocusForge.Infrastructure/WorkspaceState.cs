namespace FocusForge.Infrastructure;

public sealed class WorkspaceState
{
    public List<StoredTask> Tasks { get; set; } = new();
    public List<ScheduleBlock> Schedule { get; set; } = new();

    private static readonly LocalStore<WorkspaceState> Store = new("workspace.json");

    public static Task<WorkspaceState> LoadAsync() => Store.LoadAsync();
    public Task SaveAsync() => Store.SaveAsync(this);
}

public sealed class StoredTask
{
    public string Title { get; set; } = string.Empty;
    public string Detail { get; set; } = string.Empty;
    public string TimeLabel { get; set; } = string.Empty;
    public string Accent { get; set; } = "#7EE7C7";
    public bool IsComplete { get; set; }
}

public sealed class ScheduleBlock
{
    public string Day { get; set; } = "Friday";
    public string Time { get; set; } = "11:30 AM";
    public string Title { get; set; } = "Lunch + reset";
    public string Duration { get; set; } = "30 min";
    public string Category { get; set; } = "Personal";
}
