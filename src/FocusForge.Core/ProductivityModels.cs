namespace FocusForge.Core;

public sealed record FocusTask(Guid Id, string Title, DateTimeOffset? DueAt, bool IsComplete = false);

public sealed record FocusSession(Guid Id, string Title, DateTimeOffset StartedAt, TimeSpan Duration, bool IsComplete = false)
{
    public bool HasFinished(DateTimeOffset now) => now >= StartedAt + Duration;
}

public sealed record ProtectedApp(string Name, string ProcessName);

public sealed record LockDecision(bool IsLocked, string Reason, DateTimeOffset? UnlocksAt = null);
