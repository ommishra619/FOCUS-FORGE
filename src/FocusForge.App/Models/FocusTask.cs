using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using FocusForge.Core;

namespace FocusForge.App.Models;

public sealed class FocusTask : INotifyPropertyChanged
{
    private string _title = string.Empty;
    private string _detail = string.Empty;
    private string _timeLabel = string.Empty;
    private string _accentKey = "TaskWorkAccent";
    private string? _accentOverride;
    private bool _isComplete;

    public Guid Id { get; init; } = Guid.NewGuid();
    public string Title { get => _title; set => SetField(ref _title, value); }
    public string Detail { get => _detail; set => SetField(ref _detail, value); }
    public string TimeLabel { get => _timeLabel; set => SetField(ref _timeLabel, value); }
    public string AccentKey 
    { 
        get => _accentKey; 
        set 
        {
            SetField(ref _accentKey, value);
            OnPropertyChanged(nameof(Accent));
        }
    }
    public string Accent
    {
        get => _accentOverride ?? ((System.Windows.Media.SolidColorBrush)System.Windows.Application.Current.Resources[_accentKey])?.Color.ToString() ?? "#059669";
        set
        {
            _accentOverride = value;
            OnPropertyChanged();
        }
    }
    
    public void RefreshTheme()
    {
        OnPropertyChanged(nameof(Accent));
    }
    public TaskScheduleKind ScheduleKind { get; set; } = TaskScheduleKind.Unscheduled;
    public DateTimeOffset? DueAt { get; set; }
    public RecurrencePattern Recurrence { get; set; }
    public ObservableCollection<string> Tags { get; } = new();
    public ObservableCollection<ChecklistItem> Checklist { get; } = new();

    public int ChecklistCompletedCount => Checklist.Count(item => item.IsComplete);
    public string ChecklistProgress => Checklist.Count == 0 ? string.Empty : $"{ChecklistCompletedCount} of {Checklist.Count}";

    public bool IsComplete
    {
        get => _isComplete;
        set
        {
            if (_isComplete == value)
            {
                return;
            }

            _isComplete = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsComplete)));
        }
    }

    public void RecalculateCompletion()
    {
        if (Checklist.Count > 0)
        {
            IsComplete = Checklist.All(item => item.IsComplete);
        }

        OnPropertyChanged(nameof(ChecklistCompletedCount));
        OnPropertyChanged(nameof(ChecklistProgress));
    }

    private void SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return;
        }

        field = value;
        OnPropertyChanged(propertyName);
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public event PropertyChangedEventHandler? PropertyChanged;
}

public sealed class ChecklistItem : INotifyPropertyChanged
{
    private bool _isComplete;

    public Guid Id { get; init; } = Guid.NewGuid();
    public string Title { get; init; } = string.Empty;
    public bool IsComplete
    {
        get => _isComplete;
        set
        {
            if (_isComplete == value)
            {
                return;
            }

            _isComplete = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsComplete)));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}
