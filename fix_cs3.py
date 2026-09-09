with open('src/FocusForge.App/MainWindow.xaml.cs', 'r', encoding='utf-8') as f:
    content = f.read()

methods = '''
    private void ToggleTheme_Click(object sender, RoutedEventArgs e)
    {
        App.ToggleTheme();
        OnPropertyChanged(nameof(ProtectionColor));
    }

    private void ConfigureProtectedApps_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new ProtectedAppsDialog { Owner = this };
        dialog.ShowDialog();
    }

    private void ChangeAccentColor_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is string hex)
        {
            var color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(hex);
            Application.Current.Resources["AccentBrush"] = new System.Windows.Media.SolidColorBrush(color);
        }
    }
'''

# Find the last '}'
idx = content.rfind('}')
if idx != -1:
    content = content[:idx] + methods + '\n}\n'

with open('src/FocusForge.App/MainWindow.xaml.cs', 'w', encoding='utf-8') as f:
    f.write(content)
