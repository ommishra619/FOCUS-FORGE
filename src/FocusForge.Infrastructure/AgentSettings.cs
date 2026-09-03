namespace FocusForge.Infrastructure;

public sealed class AgentSettings
{
    private static readonly LocalStore<AgentSettings> Store = new("agent-settings.json");

    public bool IsLocked { get; set; }
    public List<string> ProtectedProcessNames { get; set; } = new();

    public static Task<AgentSettings> LoadAsync() => Store.LoadAsync();
    public void Save() => Store.SaveAsync(this).GetAwaiter().GetResult();
}
