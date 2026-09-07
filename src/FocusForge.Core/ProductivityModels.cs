namespace FocusForge.Core;

public enum TaskScheduleKind
{
    Unscheduled,
    Someday,
    SpecificDate
}

public enum RecurrencePattern
{
    None,
    Daily,
    Weekdays,
    Weekly,
    Monthly
}

public sealed record ChecklistItem(Guid Id, string Title, bool IsComplete = false);

public sealed record FocusTask(Guid Id, string Title, DateTimeOffset? DueAt, bool IsComplete = false)
{
    public TaskScheduleKind ScheduleKind { get; init; } = DueAt.HasValue ? TaskScheduleKind.SpecificDate : TaskScheduleKind.Unscheduled;
    public string TimeLabel { get; init; } = string.Empty;
    public RecurrencePattern Recurrence { get; init; }
    public IReadOnlyList<string> Tags { get; init; } = Array.Empty<string>();
    public IReadOnlyList<ChecklistItem> Checklist { get; init; } = Array.Empty<ChecklistItem>();
}

public static class TaskRules
{
    public static bool IsComplete(bool ownCompletion, IReadOnlyCollection<ChecklistItem> checklist)
        => checklist.Count == 0 ? ownCompletion : checklist.All(item => item.IsComplete);

    public static DateTimeOffset? GetNextDue(DateTimeOffset dueAt, RecurrencePattern recurrence)
    {
        return recurrence switch
        {
            RecurrencePattern.Daily => dueAt.AddDays(1),
            RecurrencePattern.Weekdays => NextWeekday(dueAt),
            RecurrencePattern.Weekly => dueAt.AddDays(7),
            RecurrencePattern.Monthly => dueAt.AddMonths(1),
            _ => null
        };
    }

    private static DateTimeOffset NextWeekday(DateTimeOffset value)
    {
        var next = value.AddDays(1);
        while (next.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
        {
            next = next.AddDays(1);
        }

        return next;
    }
}

public sealed record FocusSession(Guid Id, string Title, DateTimeOffset StartedAt, TimeSpan Duration, bool IsComplete = false)
{
    public bool HasFinished(DateTimeOffset now) => now >= StartedAt + Duration;
}

public sealed record ProtectedApp(string Name, string ProcessName);

public sealed record LockDecision(bool IsLocked, string Reason, DateTimeOffset? UnlocksAt = null);
