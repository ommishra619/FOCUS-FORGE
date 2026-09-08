using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FocusForge.Core;
using AppFocusTask = FocusForge.App.Models.FocusTask;

namespace FocusForge.App;

public partial class TaskEditorDialog : Window
{
    private readonly System.Collections.ObjectModel.ObservableCollection<string> _checklistItemsList = new();

    public string TaskTitle => TitleInput.Text.Trim();
    public string Detail => DetailInput.Text.Trim();
    public string TimeLabel => TimeInput.Text.Trim();
    public TaskScheduleKind ScheduleKind => ((ScheduleChoice)ScheduleInput.SelectedItem).Kind;
    public DateTimeOffset? DueAt => ScheduleKind == TaskScheduleKind.SpecificDate && DueDateInput.SelectedDate.HasValue
        ? new DateTimeOffset(DueDateInput.SelectedDate.Value)
        : null;
    public RecurrencePattern Recurrence => (RecurrencePattern)RecurrenceInput.SelectedItem;
    public IReadOnlyList<string> Tags => TagsInput.Text.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
    public IReadOnlyList<string> ChecklistItemsList => _checklistItemsList.ToList();

    public TaskEditorDialog(string heading, string titleLabel, string detailLabel, string timeLabel, AppFocusTask? task = null)
    {
        InitializeComponent();
        
        HeadingText.Text = heading;
        
        // Setup Schedule choices
        ScheduleInput.ItemsSource = new[]
        {
            new ScheduleChoice(TaskScheduleKind.Unscheduled, "Unscheduled"),
            new ScheduleChoice(TaskScheduleKind.Someday, "Someday"),
            new ScheduleChoice(TaskScheduleKind.SpecificDate, "Specific date")
        };
        ScheduleInput.SelectedIndex = 0;
        
        // Setup Recurrence choices
        RecurrenceInput.ItemsSource = Enum.GetValues<RecurrencePattern>();
        RecurrenceInput.SelectedItem = RecurrencePattern.None;

        ChecklistItems.ItemsSource = _checklistItemsList;

        if (task is not null)
        {
            TitleInput.Text = task.Title;
            DetailInput.Text = task.Detail;
            TimeInput.Text = task.TimeLabel;
            ScheduleInput.SelectedItem = ScheduleInput.Items.Cast<ScheduleChoice>().FirstOrDefault(item => item.Kind == task.ScheduleKind);
            DueDateInput.SelectedDate = task.DueAt?.Date;
            RecurrenceInput.SelectedItem = task.Recurrence;
            TagsInput.Text = string.Join(", ", task.Tags);
            foreach (var item in task.Checklist)
            {
                _checklistItemsList.Add(item.Title);
            }
        }

        Loaded += (_, _) => TitleInput.Focus();
    }

    private void ScheduleInput_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ScheduleKind != TaskScheduleKind.SpecificDate)
        {
            DueDateInput.SelectedDate = null;
        }
    }

    private void DueDateInput_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
    {
        if (DueDateInput.SelectedDate.HasValue)
        {
            ScheduleInput.SelectedItem = ScheduleInput.Items.Cast<ScheduleChoice>().FirstOrDefault(item => item.Kind == TaskScheduleKind.SpecificDate);
        }
    }

    private void AddChecklistItem_Click(object sender, RoutedEventArgs e)
    {
        var item = ChecklistInput.Text.Trim();
        if (item.Length == 0)
        {
            return;
        }

        _checklistItemsList.Add(item);
        ChecklistInput.Clear();
        ChecklistInput.Focus();
    }

    private void RemoveChecklistItem_Click(object sender, RoutedEventArgs e)
    {
        if (ChecklistItems.SelectedItem is string item)
        {
            _checklistItemsList.Remove(item);
        }
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
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
