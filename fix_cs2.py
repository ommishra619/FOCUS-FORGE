with open('src/FocusForge.App/MainWindow.xaml.cs', 'r', encoding='utf-8') as f:
    content = f.read()

methods = '''
    private void ChangeAccentColor_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is string hex)
        {
            var color = (Color)ColorConverter.ConvertFromString(hex);
            Application.Current.Resources["AccentBrush"] = new SolidColorBrush(color);
        }
    }
'''

content = content.replace('public class FocusTask', methods + '\n    public class FocusTask')

with open('src/FocusForge.App/MainWindow.xaml.cs', 'w', encoding='utf-8') as f:
    f.write(content)
