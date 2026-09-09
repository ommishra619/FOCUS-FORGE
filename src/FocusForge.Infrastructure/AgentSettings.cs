namespace FocusForge.Infrastructure;

public sealed class AgentSettings
{
    private static readonly LocalStore<AgentSettings> Store = new("agent-settings.json");

    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool IsLocked { get; set; }
    public List<string> ProtectedProcessNames { get; set; } = new();
    public DateTimeOffset? LockUntilUtc { get; set; }

    public static Task<AgentSettings> LoadAsync() => Store.LoadAsync();
    public void Save() => Store.SaveAsync(this).GetAwaiter().GetResult();
    public Task SaveAsync() => Store.SaveAsync(this);
}
