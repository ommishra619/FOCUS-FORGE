namespace FocusForge.Core;

public sealed class LockPolicy
{
    public LockDecision Evaluate(FocusTask prerequisite, FocusSession? session, DateTimeOffset now, DateTimeOffset? emergencyUnlockUntil)
    {
        if (emergencyUnlockUntil is not null && now < emergencyUnlockUntil)
        {
            return new LockDecision(false, "Emergency unlock is active", emergencyUnlockUntil);
        }

        if (prerequisite.IsComplete)
        {
            return new LockDecision(false, "Prerequisite task completed");
        }

        if (session is not null && session.HasFinished(now))
        {
            return new LockDecision(false, "Focus session completed");
        }

        DateTimeOffset? unlocksAt = session is null ? null : session.StartedAt + session.Duration;
        return new LockDecision(true, $"Complete '{prerequisite.Title}' or finish the focus session", unlocksAt);
    }
}
