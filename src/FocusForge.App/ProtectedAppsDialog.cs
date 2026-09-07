using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FocusForge.App;

public class SelectableApp
{
    public string ProcessName { get; set; } = "";
    public string WindowTitle { get; set; } = "";
    public bool IsSelected { get; set; }
}

public sealed class ProtectedAppsDialog : Window
{
    private readonly TextBox _customApps = new();
    private readonly ComboBox _duration = new();
    private readonly ListBox _appList = new();

    public IReadOnlyList<string> ProcessNames
    {
        get
        {
            var selectedFromList = _appList.ItemsSource.Cast<SelectableApp>()
                .Where(a => a.IsSelected)
                .Select(a => a.ProcessName);

            var custom = _customApps.Text
                .Split(new[] { ',', ';', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Where(name => !string.IsNullOrWhiteSpace(name));

            return selectedFromList.Concat(custom)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }
    }

    public TimeSpan Duration => _duration.SelectedValue is int minutes
        ? TimeSpan.FromMinutes(minutes)
        : TimeSpan.FromMinutes(25);

    public ProtectedAppsDialog(IEnumerable<string> processNames)
    {
        Title = "Protect apps";
        Width = 500;
        Height = 600;
        WindowStartupLocation = WindowStartupLocation.CenterOwner;
        ResizeMode = ResizeMode.NoResize;
        
        var panel = new StackPanel { Margin = new Thickness(26) };
        panel.Children.Add(new TextBlock { Text = "Protect apps", FontSize = 22, FontWeight = FontWeights.SemiBold, Margin = new Thickness(0, 0, 0, 8) });
        panel.Children.Add(new TextBlock { Text = "Choose the apps to close during your focus session.", Foreground = new SolidColorBrush(Color.FromRgb(154, 165, 184)), FontSize = 12, Margin = new Thickness(0, 0, 0, 18) });

        panel.Children.Add(new TextBlock { Text = "RUNNING APPS", Foreground = new SolidColorBrush(Color.FromRgb(154, 165, 184)), FontSize = 12, Margin = new Thickness(0, 0, 0, 5) });
        
        _appList.Height = 200;
        _appList.Margin = new Thickness(0, 0, 0, 14);
        _appList.BorderThickness = new Thickness(1);
        _appList.BorderBrush = new SolidColorBrush(Color.FromRgb(226, 232, 240));

        var existingNames = processNames.ToHashSet(StringComparer.OrdinalIgnoreCase);
        var runningApps = Process.GetProcesses()
            .Where(p => p.MainWindowHandle != IntPtr.Zero && !string.IsNullOrEmpty(p.MainWindowTitle))
            .GroupBy(p => p.ProcessName)
            .Select(g => g.First())
            .OrderBy(p => p.ProcessName)
            .Select(p => new SelectableApp 
            { 
                ProcessName = p.ProcessName, 
                WindowTitle = p.MainWindowTitle,
                IsSelected = existingNames.Contains(p.ProcessName)
            })
            .ToList();
            
        // Setup ItemTemplate for ListBox
        var factory = new FrameworkElementFactory(typeof(CheckBox));
        factory.SetBinding(CheckBox.IsCheckedProperty, new System.Windows.Data.Binding("IsSelected"));
        var textBinding = new System.Windows.Data.Binding { Path = new PropertyPath("."), Converter = new AppNameConverter() };
        factory.SetBinding(CheckBox.ContentProperty, textBinding);
        factory.SetValue(FrameworkElement.MarginProperty, new Thickness(5));
        
        _appList.ItemTemplate = new DataTemplate { VisualTree = factory };
        _appList.ItemsSource = runningApps;
        panel.Children.Add(_appList);

        panel.Children.Add(new TextBlock { Text = "CUSTOM PROCESS NAMES (comma-separated)", Foreground = new SolidColorBrush(Color.FromRgb(154, 165, 184)), FontSize = 12, Margin = new Thickness(0, 0, 0, 5) });
        var unselectedExisting = existingNames.Except(runningApps.Select(r => r.ProcessName)).ToList();
        _customApps.Text = string.Join(Environment.NewLine, unselectedExisting);
        _customApps.AcceptsReturn = true;
        _customApps.Height = 60;
        _customApps.TextWrapping = TextWrapping.Wrap;
        _customApps.Padding = new Thickness(9, 7, 9, 7);
        _customApps.Margin = new Thickness(0, 0, 0, 14);
        panel.Children.Add(_customApps);

        panel.Children.Add(new TextBlock { Text = "LOCK FOR", Foreground = new SolidColorBrush(Color.FromRgb(154, 165, 184)), FontSize = 12, Margin = new Thickness(0, 0, 0, 5) });
        _duration.ItemsSource = new[]
        {
            new { Label = "25 minutes", Minutes = 25 },
            new { Label = "45 minutes", Minutes = 45 },
            new { Label = "1 hour", Minutes = 60 },
            new { Label = "2 hours", Minutes = 120 },
            new { Label = "Until I finish", Minutes = 0 }
        };
        _duration.DisplayMemberPath = "Label";
        _duration.SelectedValuePath = "Minutes";
        _duration.SelectedIndex = 0;
        _duration.Padding = new Thickness(7, 5, 7, 5);
        panel.Children.Add(_duration);

        var buttons = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(0, 22, 0, 0) };
        var cancel = new Button { Content = "Cancel", Padding = new Thickness(14, 8, 14, 8), Margin = new Thickness(0, 0, 8, 0) };
        cancel.Click += (_, _) => DialogResult = false;
        var protect = new Button { Content = "Protect apps", Padding = new Thickness(18, 8, 18, 8), FontWeight = FontWeights.SemiBold };
        protect.Click += Protect_Click;
        buttons.Children.Add(cancel);
        buttons.Children.Add(protect);
        panel.Children.Add(buttons);

        Content = panel;
        Loaded += (_, _) => _appList.Focus();
    }

    private void Protect_Click(object sender, RoutedEventArgs e)
    {
        if (ProcessNames.Count == 0)
        {
            MessageBox.Show(this, "Add at least one process name.", "FocusForge", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        DialogResult = true;
    }
}

public class AppNameConverter : System.Windows.Data.IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (value is SelectableApp app)
            return $"{app.ProcessName} ({app.WindowTitle})";
        return "";
    }
    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) => throw new NotImplementedException();
}
