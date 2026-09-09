using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FocusForge.Infrastructure;

public sealed class AgentSettings
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public bool IsLocked { get; set; }
    public List<string> ProtectedProcessNames { get; set; } = new();
    public DateTimeOffset? LockUntilUtc { get; set; }
    public string UserName { get; set; } = "";
    public string Password { get; set; } = "";

    public static async Task<AgentSettings> LoadAsync()
    {
        using var db = new AppDbContext();
        await db.Database.EnsureCreatedAsync();
        var settings = await db.AgentSettings.FirstOrDefaultAsync();
        if (settings == null)
        {
            settings = new AgentSettings();
            db.AgentSettings.Add(settings);
            await db.SaveChangesAsync();
        }
        return settings;
    }

    public void Save()
    {
        using var db = new AppDbContext();
        var existing = db.AgentSettings.FirstOrDefault(s => s.Id == Id);
        if (existing != null)
        {
            existing.IsLocked = IsLocked;
            existing.ProtectedProcessNames = ProtectedProcessNames;
            existing.LockUntilUtc = LockUntilUtc;
            existing.UserName = UserName;
            existing.Password = Password;
            db.SaveChanges();
        }
    }
}
