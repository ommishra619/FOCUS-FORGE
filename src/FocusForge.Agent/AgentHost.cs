using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using FocusForge.Infrastructure;

namespace FocusForge.Agent;

public sealed class AgentHost : ApplicationContext
{
    private readonly NotifyIcon _trayIcon;
    private readonly System.Windows.Forms.Timer _watchTimer;
    private readonly ProcessBlocker _processBlocker = new();
    private readonly AgentSettings _settings;

    public AgentHost(AgentSettings settings)
    {
        _settings = settings;
        _trayIcon = new NotifyIcon
        {
            Icon = SystemIcons.Application,
            Text = "FocusForge",
            Visible = true,
            ContextMenuStrip = BuildMenu()
        };

        _watchTimer = new System.Windows.Forms.Timer { Interval = 5000 };
        _watchTimer.Tick += (_, _) => EnforceLocks();
        _watchTimer.Start();
        EnforceLocks();
    }

    private ContextMenuStrip BuildMenu()
    {
        var menu = new ContextMenuStrip();
        menu.Items.Add("FocusForge is running", null, (_, _) => ShowStatus());
        menu.Items.Add("Emergency unlock", null, (_, _) => EmergencyUnlock());
        menu.Items.Add(new ToolStripSeparator());
        menu.Items.Add("Exit agent", null, (_, _) => ExitThread());
        return menu;
    }

    private void EnforceLocks()
    {
        using (var db = new AppDbContext())
        {
            var latest = System.Linq.Queryable.FirstOrDefault(db.AgentSettings, s => s.Id == _settings.Id);
            if (latest != null)
            {
                _settings.IsLocked = latest.IsLocked;
                _settings.ProtectedProcessNames = latest.ProtectedProcessNames;
                _settings.LockUntilUtc = latest.LockUntilUtc;
            }
        }

        if (_settings.IsLocked && _settings.LockUntilUtc is not null && DateTimeOffset.UtcNow >= _settings.LockUntilUtc)
        {
            _settings.IsLocked = false;
            _settings.LockUntilUtc = null;
            _settings.Save();
        }

        if (!_settings.IsLocked)
        {
            return;
        }

        foreach (var processName in _settings.ProtectedProcessNames)
        {
            var closed = _processBlocker.CloseRunning(processName);
            if (closed > 0)
            {
                _trayIcon.ShowBalloonTip(2500, "FocusForge", $"Closed {processName} while your focus rule is active.", ToolTipIcon.Info);
            }
        }
    }

    private void ShowStatus()
    {
        _trayIcon.ShowBalloonTip(3000, "FocusForge", _settings.IsLocked ? "Protected apps are locked." : "Protected apps are unlocked.", ToolTipIcon.Info);
    }

    private void EmergencyUnlock()
    {
        _settings.IsLocked = false;
        _settings.Save();
        _trayIcon.ShowBalloonTip(3000, "FocusForge", "Emergency unlock enabled. This action was recorded locally.", ToolTipIcon.Warning);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _watchTimer.Dispose();
            _trayIcon.Visible = false;
            _trayIcon.Dispose();
        }

        base.Dispose(disposing);
    }
}
