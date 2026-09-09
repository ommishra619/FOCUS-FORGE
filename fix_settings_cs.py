with open('src/FocusForge.App/MainWindow.xaml.cs', 'r', encoding='utf-8') as f:
    content = f.read()

replacement = '''
    private void ThemePresetComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (ThemePresetComboBox.SelectedItem is System.Windows.Controls.ComboBoxItem item && item.Tag is string themeName)
        {
            var dict = new System.Windows.ResourceDictionary { Source = new Uri($"Themes/{themeName}.xaml", UriKind.Relative) };
            var mergedDicts = Application.Current.Resources.MergedDictionaries;
            for (int i = 0; i < mergedDicts.Count; i++)
            {
                if (mergedDicts[i].Source != null && mergedDicts[i].Source.OriginalString.StartsWith("Themes/"))
                {
                    mergedDicts[i] = dict;
                    break;
                }
            }
            
            // Update the hex code text blocks based on the theme
            if (BgColorText != null && FgColorText != null && AcColorText != null)
            {
                if (themeName == "RedWhiteTheme") { BgColorText.Text = "# FFFFFF"; FgColorText.Text = "# DC2626"; AcColorText.Text = "# DC2626"; }
                else if (themeName == "PinkWhiteTheme") { BgColorText.Text = "# F472B6"; FgColorText.Text = "# FFFFFF"; AcColorText.Text = "# FFFFFF"; }
                else if (themeName == "BlueWhiteTheme") { BgColorText.Text = "# 38BDF8"; FgColorText.Text = "# FFFFFF"; AcColorText.Text = "# FFFFFF"; }
                else if (themeName == "BlackWhiteTheme") { BgColorText.Text = "# 000000"; FgColorText.Text = "# FFFFFF"; AcColorText.Text = "# FFFFFF"; }
            }
            
            OnPropertyChanged(nameof(ProtectionColor));
            if (Tasks != null) foreach (var task in Tasks) task.RefreshTheme();
        }
    }
'''

import re
pattern = r'private void ChangeTheme_Click.*?foreach \(var task in Tasks\) task\.RefreshTheme\(\);\s*\}\s*\}'
content = re.sub(pattern, replacement.strip(), content, flags=re.DOTALL)

with open('src/FocusForge.App/MainWindow.xaml.cs', 'w', encoding='utf-8') as f:
    f.write(content)
