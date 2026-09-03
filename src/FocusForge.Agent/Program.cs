using System.Windows.Forms;
using Microsoft.Win32;
using FocusForge.Agent;
using FocusForge.Infrastructure;

var settings = await AgentSettings.LoadAsync();
if (args.Contains("--lock", StringComparer.OrdinalIgnoreCase))
{
	settings.IsLocked = true;
	settings.Save();
}

if (args.Contains("--startup", StringComparer.OrdinalIgnoreCase))
{
	using var startupKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", writable: true)
		?? Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run");
	startupKey.SetValue("FocusForge", $"\"{Environment.ProcessPath}\"");
}

ApplicationConfiguration.Initialize();
Application.Run(new AgentHost(settings));
