with open('src/FocusForge.App/MainWindow.xaml.cs', 'r', encoding='utf-8') as f:
    content = f.read()

methods = '''
    private void ToggleTheme_Click(object sender, RoutedEventArgs e)
    {
        App.ToggleTheme();
        OnPropertyChanged(nameof(ProtectionColor));
        foreach (var task in Tasks) task.RefreshTheme();
    }

    private void ConfigureProtectedApps_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new ProtectedAppsDialog { Owner = this };
        dialog.ShowDialog();
    }
'''

content = content.replace('public class FocusTask', methods + '\n    public class FocusTask')

with open('src/FocusForge.App/MainWindow.xaml.cs', 'w', encoding='utf-8') as f:
    f.write(content)
