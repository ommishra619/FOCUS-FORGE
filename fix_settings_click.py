with open('src/FocusForge.App/MainWindow.xaml.cs', 'r', encoding='utf-8') as f:
    content = f.read()

# Fix Settings_Click
content = content.replace('private void Settings_Click(object sender, RoutedEventArgs e) => ShowSection("CONTROL", "Settings", "Local-only preferences and recovery controls.", showTasks: false);',
                          'private void Settings_Click(object sender, RoutedEventArgs e) => ShowSection("CONTROL", "Settings", "Local-only preferences and recovery controls.", showSettings: true);')

# Update ShowSection signature and body
content = content.replace('private void ShowSection(string eyebrow, string title, string description, bool showTasks = false, bool showSchedule = false)',
                          'private void ShowSection(string eyebrow, string title, string description, bool showTasks = false, bool showSchedule = false, bool showSettings = false)')

content = content.replace('AddScheduleButton.Visibility = showSchedule ? Visibility.Visible : Visibility.Collapsed;',
                          'AddScheduleButton.Visibility = showSchedule ? Visibility.Visible : Visibility.Collapsed;\n        if (FindName("SettingsPanel") is System.Windows.UIElement settingsPanel) settingsPanel.Visibility = showSettings ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;')

# Add ChangeTheme_Click method
theme_method = '''
    private void ChangeTheme_Click(object sender, RoutedEventArgs e)
    {
        if (sender is System.Windows.Controls.Button btn && btn.Tag is string themeName)
        {
            var dict = new System.Windows.ResourceDictionary { Source = new Uri($"Themes/{themeName}.xaml", UriKind.Relative) };
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(dict);
            
            // Refresh elements that need explicit updates
            OnPropertyChanged(nameof(ProtectionColor));
            foreach (var task in Tasks) task.RefreshTheme();
        }
    }
'''

idx = content.rfind('}')
if idx != -1:
    content = content[:idx] + theme_method + '\n}\n'

with open('src/FocusForge.App/MainWindow.xaml.cs', 'w', encoding='utf-8') as f:
    f.write(content)
