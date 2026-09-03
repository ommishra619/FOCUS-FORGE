using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FocusForge.App;

public sealed class TaskEditorDialog : Window
{
    private readonly TextBox _title = new();
    private readonly TextBox _detail = new();
    private readonly TextBox _time = new();

    public string TaskTitle => _title.Text.Trim();
    public string Detail => _detail.Text.Trim();
    public string TimeLabel => _time.Text.Trim();

    public TaskEditorDialog(string heading, string titleLabel, string detailLabel, string timeLabel)
    {
        Title = heading;
        Width = 420;
        Height = 350;
        WindowStartupLocation = WindowStartupLocation.CenterOwner;
        ResizeMode = ResizeMode.NoResize;
        Background = new SolidColorBrush(Color.FromRgb(22, 29, 43));
        Foreground = new SolidColorBrush(Color.FromRgb(245, 247, 251));

        var panel = new StackPanel { Margin = new Thickness(26) };
        panel.Children.Add(new TextBlock { Text = heading, FontSize = 22, FontWeight = FontWeights.SemiBold, Margin = new Thickness(0, 0, 0, 20) });
        AddField(panel, titleLabel, _title);
        AddField(panel, detailLabel, _detail);
        AddField(panel, timeLabel, _time);

        var buttons = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(0, 18, 0, 0) };
        var cancel = new Button { Content = "Cancel", Padding = new Thickness(14, 8, 14, 8), Margin = new Thickness(0, 0, 8, 0) };
        cancel.Click += (_, _) => DialogResult = false;
        var save = new Button { Content = "Save", Padding = new Thickness(18, 8, 18, 8), Background = new SolidColorBrush(Color.FromRgb(126, 231, 199)), Foreground = new SolidColorBrush(Color.FromRgb(16, 35, 31)), FontWeight = FontWeights.SemiBold };
        save.Click += Save_Click;
        buttons.Children.Add(cancel);
        buttons.Children.Add(save);
        panel.Children.Add(buttons);
        Content = panel;
        Loaded += (_, _) => _title.Focus();
    }

    private static void AddField(Panel panel, string label, TextBox input)
    {
        panel.Children.Add(new TextBlock { Text = label, Foreground = new SolidColorBrush(Color.FromRgb(154, 165, 184)), FontSize = 12, Margin = new Thickness(0, 0, 0, 5) });
        input.Padding = new Thickness(9, 7, 9, 7);
        input.Margin = new Thickness(0, 0, 0, 12);
        panel.Children.Add(input);
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TaskTitle))
        {
            MessageBox.Show(this, "Give this item a name first.", "FocusForge", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        DialogResult = true;
    }
}
