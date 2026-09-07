using System;
using System.Windows;

namespace FocusForge.App;

public partial class App : Application
{
    public static bool IsDarkTheme { get; private set; }

    public static void ToggleTheme()
    {
        IsDarkTheme = !IsDarkTheme;
        var themeName = IsDarkTheme ? "DarkTheme" : "LightTheme";
        var dict = new ResourceDictionary { Source = new Uri($"pack://application:,,,/FocusForge.App;component/Themes/{themeName}.xaml") };
        Current.Resources.MergedDictionaries.Clear();
        Current.Resources.MergedDictionaries.Add(dict);
    }
}
