using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FocusForge.App;

public sealed class ProtectedAppsDialog : Window
{
    private readonly TextBox _apps = new();
    private readonly ComboBox _duration = new();

    public IReadOnlyList<string> ProcessNames => _apps.Text
        .Split(new[] { ',', ';', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        .Where(name => !string.IsNullOrWhiteSpace(name))
        .Distinct(StringComparer.OrdinalIgnoreCase)
        .ToList();

    public TimeSpan Duration => _duration.SelectedValue is int minutes
        ? TimeSpan.FromMinutes(minutes)
        : TimeSpan.FromMinutes(25);

    public ProtectedAppsDialog(IEnumerable<string> processNames)
    {
        Title = "Protect apps";
        Width = 460;
        Height = 390;
        WindowStartupLocation = WindowStartupLocation.CenterOwner;
        ResizeMode = ResizeMode.NoResize;
        Background = new SolidColorBrush(Color.FromRgb(22, 29, 43));
        Foreground = new SolidColorBrush(Color.FromRgb(245, 247, 251));

        var panel = new StackPanel { Margin = new Thickness(26) };
        panel.Children.Add(new TextBlock { Text = "Protect apps", FontSize = 22, FontWeight = FontWeights.SemiBold, Margin = new Thickness(0, 0, 0, 8) });
        panel.Children.Add(new TextBlock { Text = "Choose the apps to close during your focus session.", Foreground = new SolidColorBrush(Color.FromRgb(154, 165, 184)), FontSize = 12, Margin = new Thickness(0, 0, 0, 18) });

        panel.Children.Add(new TextBlock { Text = "PROCESS NAMES", Foreground = new SolidColorBrush(Color.FromRgb(154, 165, 184)), FontSize = 12, Margin = new Thickness(0, 0, 0, 5) });
        _apps.Text = string.Join(Environment.NewLine, processNames);
        _apps.AcceptsReturn = true;
        _apps.Height = 82;
        _apps.TextWrapping = TextWrapping.Wrap;
        _apps.Padding = new Thickness(9, 7, 9, 7);
        _apps.Margin = new Thickness(0, 0, 0, 14);
        panel.Children.Add(_apps);

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
        var protect = new Button { Content = "Protect apps", Padding = new Thickness(18, 8, 18, 8), Background = new SolidColorBrush(Color.FromRgb(126, 231, 199)), Foreground = new SolidColorBrush(Color.FromRgb(16, 35, 31)), FontWeight = FontWeights.SemiBold };
        protect.Click += Protect_Click;
        buttons.Children.Add(cancel);
        buttons.Children.Add(protect);
        panel.Children.Add(buttons);

        Content = panel;
        Loaded += (_, _) => _apps.Focus();
    }

    private void Protect_Click(object sender, RoutedEventArgs e)
    {
        if (ProcessNames.Count == 0)
        {
            MessageBox.Show(this, "Add at least one process name, such as steam or discord.", "FocusForge", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        DialogResult = true;
    }
}
