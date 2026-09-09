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
        var loginWindow = new LoginWindow();
        loginWindow.Show();
    }
}
