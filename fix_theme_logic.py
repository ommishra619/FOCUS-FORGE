with open('src/FocusForge.App/MainWindow.xaml.cs', 'r', encoding='utf-8') as f:
    content = f.read()

replacement = '''
    private void ChangeTheme_Click(object sender, RoutedEventArgs e)
    {
        if (sender is System.Windows.Controls.Button btn && btn.Tag is string themeName)
        {
            var dict = new System.Windows.ResourceDictionary { Source = new Uri($"Themes/{themeName}.xaml", UriKind.Relative) };
            
            // Find and replace the existing theme dictionary
            var mergedDicts = Application.Current.Resources.MergedDictionaries;
            for (int i = 0; i < mergedDicts.Count; i++)
            {
                if (mergedDicts[i].Source != null && mergedDicts[i].Source.OriginalString.StartsWith("Themes/"))
                {
                    mergedDicts[i] = dict;
                    break;
                }
            }
            
            // Refresh elements that need explicit updates
            OnPropertyChanged(nameof(ProtectionColor));
            foreach (var task in Tasks) task.RefreshTheme();
        }
    }
'''

import re
pattern = r'private void ChangeTheme_Click.*?foreach \(var task in Tasks\) task\.RefreshTheme\(\);\s*\}\s*\}'
content = re.sub(pattern, replacement.strip(), content, flags=re.DOTALL)

with open('src/FocusForge.App/MainWindow.xaml.cs', 'w', encoding='utf-8') as f:
    f.write(content)
