using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FocusForge.Core;
using AppFocusTask = FocusForge.App.Models.FocusTask;

namespace FocusForge.App;

public sealed class TaskEditorDialog : Window
{
    private readonly TextBox _title = new();
    private readonly TextBox _detail = new();
    private readonly TextBox _time = new();
    private readonly ComboBox _schedule = new();
    private readonly DatePicker _dueDate = new();
    private readonly ComboBox _recurrence = new();
    private readonly TextBox _tags = new();
    private readonly TextBox _checklistInput = new();
    private readonly ListBox _checklist = new();
    private readonly System.Collections.ObjectModel.ObservableCollection<string> _checklistItems = new();

    public string TaskTitle => _title.Text.Trim();
    public string Detail => _detail.Text.Trim();
    public string TimeLabel => _time.Text.Trim();
    public TaskScheduleKind ScheduleKind => ((ScheduleChoice)_schedule.SelectedItem).Kind;
    public DateTimeOffset? DueAt => ScheduleKind == TaskScheduleKind.SpecificDate && _dueDate.SelectedDate.HasValue
        ? new DateTimeOffset(_dueDate.SelectedDate.Value)
        : null;
    public RecurrencePattern Recurrence => (RecurrencePattern)_recurrence.SelectedItem;
    public IReadOnlyList<string> Tags => _tags.Text.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
    public IReadOnlyList<string> ChecklistItems => _checklistItems.ToList();

    public TaskEditorDialog(string heading, string titleLabel, string detailLabel, string timeLabel, AppFocusTask? task = null)
    {
        Title = heading;
        Width = 460;
        Height = 650;
        WindowStartupLocation = WindowStartupLocation.CenterOwner;
        ResizeMode = ResizeMode.CanMinimize;
        Background = new SolidColorBrush(Color.FromRgb(22, 29, 43));
        Foreground = new SolidColorBrush(Color.FromRgb(245, 247, 251));

        var panel = new StackPanel { Margin = new Thickness(26) };
        panel.Children.Add(new TextBlock { Text = heading, FontSize = 22, FontWeight = FontWeights.SemiBold, Margin = new Thickness(0, 0, 0, 20) });
        AddField(panel, titleLabel, _title);
        AddField(panel, detailLabel, _detail);
        AddField(panel, timeLabel, _time);

        AddScheduleFields(panel);
        AddField(panel, "Tags, separated by commas", _tags);
        AddChecklistField(panel);

        var buttons = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(0, 18, 0, 0) };
        var cancel = new Button { Content = "Cancel", Padding = new Thickness(14, 8, 14, 8), Margin = new Thickness(0, 0, 8, 0) };
        cancel.Click += (_, _) => DialogResult = false;
        var save = new Button { Content = "Save", Padding = new Thickness(18, 8, 18, 8), Background = new SolidColorBrush(Color.FromRgb(126, 231, 199)), Foreground = new SolidColorBrush(Color.FromRgb(16, 35, 31)), FontWeight = FontWeights.SemiBold };
        save.Click += Save_Click;
        buttons.Children.Add(cancel);
        buttons.Children.Add(save);
        panel.Children.Add(buttons);
        Content = panel;
        if (task is not null)
        {
            _title.Text = task.Title;
            _detail.Text = task.Detail;
            _time.Text = task.TimeLabel;
            _schedule.SelectedItem = _schedule.Items.Cast<ScheduleChoice>().FirstOrDefault(item => item.Kind == task.ScheduleKind);
            _dueDate.SelectedDate = task.DueAt?.Date;
            _recurrence.SelectedItem = task.Recurrence;
            _tags.Text = string.Join(", ", task.Tags);
            foreach (var item in task.Checklist)
            {
                _checklistItems.Add(item.Title);
            }
        }

        _checklist.ItemsSource = _checklistItems;
        Loaded += (_, _) => _title.Focus();
    }

    private void AddScheduleFields(Panel panel)
    {
        panel.Children.Add(new TextBlock { Text = "Schedule", Foreground = new SolidColorBrush(Color.FromRgb(154, 165, 184)), FontSize = 12, Margin = new Thickness(0, 0, 0, 5) });
        _schedule.ItemsSource = new[]
        {
            new ScheduleChoice(TaskScheduleKind.Unscheduled, "Unscheduled"),
            new ScheduleChoice(TaskScheduleKind.Someday, "Someday"),
            new ScheduleChoice(TaskScheduleKind.SpecificDate, "Specific date")
        };
        _schedule.SelectedIndex = 0;
        _schedule.Margin = new Thickness(0, 0, 0, 10);
        _schedule.SelectionChanged += (_, _) => 
        {
            if (ScheduleKind != TaskScheduleKind.SpecificDate)
            {
                _dueDate.SelectedDate = null;
            }
        };
        panel.Children.Add(_schedule);

        _dueDate.Margin = new Thickness(0, 0, 0, 10);
        _dueDate.IsEnabled = true;
        _dueDate.SelectedDateChanged += (_, _) =>
        {
            if (_dueDate.SelectedDate.HasValue)
            {
                _schedule.SelectedItem = _schedule.Items.Cast<ScheduleChoice>().FirstOrDefault(item => item.Kind == TaskScheduleKind.SpecificDate);
            }
        };
        panel.Children.Add(_dueDate);

        panel.Children.Add(new TextBlock { Text = "Repeat", Foreground = new SolidColorBrush(Color.FromRgb(154, 165, 184)), FontSize = 12, Margin = new Thickness(0, 0, 0, 5) });
        _recurrence.ItemsSource = Enum.GetValues<RecurrencePattern>();
        _recurrence.SelectedItem = RecurrencePattern.None;
        _recurrence.Margin = new Thickness(0, 0, 0, 12);
        panel.Children.Add(_recurrence);
    }

    private void AddChecklistField(Panel panel)
    {
        panel.Children.Add(new TextBlock { Text = "Checklist items", Foreground = new SolidColorBrush(Color.FromRgb(154, 165, 184)), FontSize = 12, Margin = new Thickness(0, 0, 0, 5) });
        var row = new DockPanel { Margin = new Thickness(0, 0, 0, 7) };
        _checklistInput.Padding = new Thickness(9, 7, 9, 7);
        DockPanel.SetDock(_checklistInput, Dock.Left);
        row.Children.Add(_checklistInput);
        var add = new Button { Content = "Add", Padding = new Thickness(12, 7, 12, 7), Margin = new Thickness(8, 0, 0, 0) };
        add.Click += AddChecklistItem_Click;
        DockPanel.SetDock(add, Dock.Right);
        row.Children.Add(add);
        panel.Children.Add(row);
        _checklist.MaxHeight = 90;
        _checklist.Margin = new Thickness(0, 0, 0, 6);
        panel.Children.Add(_checklist);
        var remove = new Button { Content = "Remove selected", HorizontalAlignment = HorizontalAlignment.Left, Padding = new Thickness(10, 5, 10, 5) };
        remove.Click += (_, _) =>
        {
            if (_checklist.SelectedItem is string item)
            {
                _checklistItems.Remove(item);
            }
        };
        panel.Children.Add(remove);
    }

    private void AddChecklistItem_Click(object sender, RoutedEventArgs e)
    {
        var item = _checklistInput.Text.Trim();
        if (item.Length == 0)
        {
            return;
        }

        _checklistItems.Add(item);
        _checklistInput.Clear();
        _checklistInput.Focus();
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

    private sealed record ScheduleChoice(TaskScheduleKind Kind, string Label)
    {
        public override string ToString() => Label;
    }
}
