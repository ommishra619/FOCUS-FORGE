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
    private AgentSettings _settings;

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

        _watchTimer = new System.Windows.Forms.Timer { Interval = 1000 };
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

    private readonly HashSet<string> _activeOverlays = new();

    private void EnforceLocks()
    {
        _settings = AgentSettings.LoadAsync().GetAwaiter().GetResult();

        if (_settings.IsLocked && _settings.LockUntilUtc is not null && DateTimeOffset.UtcNow >= _settings.LockUntilUtc)
        {
            _settings.IsLocked = false;
            _settings.LockUntilUtc = null;
            _settings.Save();
        }

        if (!_settings.IsLocked)
        {
            _activeOverlays.Clear();
            return;
        }

        foreach (var processName in _settings.ProtectedProcessNames)
        {
            var running = _processBlocker.FindRunning(processName);
            if (running.Count > 0)
            {
                if (!_activeOverlays.Contains(processName))
                {
                    _activeOverlays.Add(processName);
                    
                    // Launch FocusForge.App with overlay arguments
                    var appPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FocusForge.App.exe");
                    if (System.IO.File.Exists(appPath))
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = appPath,
                            Arguments = $"--overlay \"{processName}\"",
                            UseShellExecute = true
                        });
                    }
                }
            }
            else
            {
                _activeOverlays.Remove(processName);
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
