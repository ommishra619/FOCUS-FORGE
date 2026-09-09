using System;
using System.Windows;

namespace FocusForge.App;

public partial class App : Application
{
    public static void ApplyTheme(string themeName)
    {
        var dict = new ResourceDictionary { Source = new Uri($"pack://application:,,,/FocusForge.App;component/Themes/{themeName}.xaml") };
        var mergedDicts = Current.Resources.MergedDictionaries;
        for (int i = 0; i < mergedDicts.Count; i++)
        {
            if (mergedDicts[i].Source != null && mergedDicts[i].Source.OriginalString.Contains("Themes/"))
            {
                mergedDicts[i] = dict;
                return;
            }
        }
        mergedDicts.Add(dict);
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        
        try
        {
            var agentName = "FocusForge.Agent";
            if (System.Diagnostics.Process.GetProcessesByName(agentName).Length == 0)
            {
                var basePath = AppDomain.CurrentDomain.BaseDirectory;
                var agentPaths = new[]
                {
                    System.IO.Path.Combine(basePath, "FocusForge.Agent.exe"),
                    System.IO.Path.GetFullPath(System.IO.Path.Combine(basePath, "..", "..", "..", "..", "FocusForge.Agent", "bin", "Debug", "net8.0-windows", "FocusForge.Agent.exe"))
                };
                var agentPath = System.Array.Find(agentPaths, System.IO.File.Exists);

                if (agentPath is not null)
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = agentPath,
                        UseShellExecute = true,
                        CreateNoWindow = true
                    });
                }
            }
        }
        catch (Exception)
        {
            // Ignore startup errors for the agent
        }

        var loginWindow = new LoginWindow();
        loginWindow.Show();
    }
}
