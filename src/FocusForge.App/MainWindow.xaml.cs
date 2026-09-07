using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using FocusForge.App.Models;
using FocusForge.Infrastructure;

namespace FocusForge.App;

public partial class MainWindow : Window, INotifyPropertyChanged
{
    private readonly DispatcherTimer _focusTimer;
    private DateTime _focusEndsAt = DateTime.Now.AddHours(1).AddMinutes(12).AddSeconds(40);
    private bool _sessionFinished;
    private AgentSettings _agentSettings = new();
    private WorkspaceState _workspaceState = new();

    public ObservableCollection<FocusTask> Tasks { get; } = new()
    {
        new FocusTask { Title = "Deep work session", Detail = "Finish the database schema", TimeLabel = "09:00 - 10:30" },
        new FocusTask { Title = "Review project notes", Detail = "Capture the next three actions", TimeLabel = "10:45 - 11:15" },
        new FocusTask { Title = "Move your body", Detail = "A short walk outside", TimeLabel = "02:00 - 02:30" },
        new FocusTask { Title = "Plan tomorrow", Detail = "Close the day with intention", TimeLabel = "06:00 - 06:15" }
    };

    public string FocusTimeLabel => _sessionFinished ? "Complete" : FormatRemaining(_focusEndsAt - DateTime.Now);
    public string FocusStatus => _sessionFinished ? "Session complete. Steam is ready to unlock." : "Steam is protected until this session ends.";
    public string ProtectionState => _sessionFinished ? "Unlocked" : "Locked";
    public string ProtectionColor => _sessionFinished ? "#7EE7C7" : "#FF8D8D";

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
        Loaded += LoadWorkspaceAsync;
        _focusTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        _focusTimer.Tick += FocusTimer_Tick;
        _focusTimer.Start();
    }

    private void AddTask_Click(object sender, RoutedEventArgs e)
    {
        var editor = new TaskEditorDialog("Add task", "Task name", "Details", "Time, for example 3:00 PM");
        editor.Owner = this;
        if (editor.ShowDialog() == true)
        {
            Tasks.Add(new FocusTask { Title = editor.TaskTitle, Detail = editor.Detail, TimeLabel = editor.TimeLabel });
            SaveWorkspace();
        }
    }

    private void TaskCheck_Click(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement { DataContext: FocusTask task })
        {
            task.IsComplete = !task.IsComplete;
            SaveWorkspace();
        }
    }

    private void FinishSession_Click(object sender, RoutedEventArgs e)
    {
        _sessionFinished = true;
        _agentSettings.IsLocked = false;
        _agentSettings.Save();
        _focusTimer.Stop();
        NotifyFocusChanged();
    }

    private void EmergencyUnlock_Click(object sender, RoutedEventArgs e)
    {
        _sessionFinished = true;
        _agentSettings.IsLocked = false;
        _agentSettings.Save();
        _focusTimer.Stop();
        NotifyFocusChanged();
    }

    private void LockSteam_Click(object sender, RoutedEventArgs e)
    {
        _agentSettings.IsLocked = true;
        if (!_agentSettings.ProtectedProcessNames.Contains("steam"))
        {
            _agentSettings.ProtectedProcessNames.Add("steam");
        }

        _agentSettings.Save();
        _sessionFinished = false;
        _focusEndsAt = DateTime.Now.AddMinutes(25);
        _focusTimer.Start();
        NotifyFocusChanged();
    }

    private void AddSchedule_Click(object sender, RoutedEventArgs e)
    {
        var editor = new TaskEditorDialog("Add schedule block", "Activity", "Category", "Day and time, for example Friday 7:00 PM");
        editor.Owner = this;
        if (editor.ShowDialog() == true)
        {
            _workspaceState.Schedule.Add(new ScheduleBlock { Day = "Custom", Time = editor.TimeLabel, Title = editor.TaskTitle, Duration = "30 min", Category = editor.Detail });
            SaveWorkspace();
            OnPropertyChanged(nameof(Schedule));
        }
    }

    public IReadOnlyList<ScheduleBlock> Schedule => _workspaceState.Schedule;

    private void Today_Click(object sender, RoutedEventArgs e) => ShowToday();
    private void Tasks_Click(object sender, RoutedEventArgs e) => ShowSection("WORKSPACE", "Tasks", "Everything you want to make time for.", showTasks: true);
    private void WeeklyPlan_Click(object sender, RoutedEventArgs e) => ShowSection("WORKSPACE", "Weekly plan", "A clear shape for the week ahead.", showSchedule: true);
    private void FocusSessions_Click(object sender, RoutedEventArgs e) => ShowSection("WORKSPACE", "Focus sessions", FocusStatus, showTasks: false);
    private void ProtectedApps_Click(object sender, RoutedEventArgs e) => ShowSection("CONTROL", "Protected apps", "Your distractions stay closed while the rule is active.", showTasks: false);
    private void Settings_Click(object sender, RoutedEventArgs e) => ShowSection("CONTROL", "Settings", "Local-only preferences and recovery controls.", showTasks: false);

    private void ShowToday()
    {
        TodayView.Visibility = Visibility.Visible;
        SectionView.Visibility = Visibility.Collapsed;
    }

    private void ShowSection(string eyebrow, string title, string description, bool showTasks = false, bool showSchedule = false)
    {
        TodayView.Visibility = Visibility.Collapsed;
        SectionView.Visibility = Visibility.Visible;
        SectionEyebrow.Text = eyebrow;
        SectionTitle.Text = title;
        SectionDescription.Text = description;
        SectionTasks.Visibility = showTasks ? Visibility.Visible : Visibility.Collapsed;
        SectionSchedule.Visibility = showSchedule ? Visibility.Visible : Visibility.Collapsed;
        AddTaskButton.Visibility = showTasks ? Visibility.Visible : Visibility.Collapsed;
        AddScheduleButton.Visibility = showSchedule ? Visibility.Visible : Visibility.Collapsed;
    }

    private async void LoadWorkspaceAsync(object sender, RoutedEventArgs e)
    {
        _agentSettings = await AgentSettings.LoadAsync();
        _workspaceState = await WorkspaceState.LoadAsync();
        if (_workspaceState.Tasks.Count > 0)
        {
            Tasks.Clear();
            foreach (var task in _workspaceState.Tasks)
            {
                Tasks.Add(new FocusTask { Title = task.Title, Detail = task.Detail, TimeLabel = task.TimeLabel, AccentKey = task.Accent, IsComplete = task.IsComplete });
            }
        }

        if (_workspaceState.Schedule.Count == 0)
        {
            _workspaceState.Schedule.Add(new ScheduleBlock { Day = "Friday", Time = "11:30 AM", Title = "Lunch + reset", Duration = "30 min", Category = "Personal" });
            _workspaceState.Schedule.Add(new ScheduleBlock { Day = "Friday", Time = "02:00 PM", Title = "Move your body", Duration = "30 min", Category = "Health" });
            await _workspaceState.SaveAsync();
        }

        OnPropertyChanged(nameof(Schedule));
    }

    private void SaveWorkspace()
    {
        _workspaceState.Tasks = Tasks.Select(task => new StoredTask
        {
            Title = task.Title,
            Detail = task.Detail,
            TimeLabel = task.TimeLabel,
            Accent = task.AccentKey,
            IsComplete = task.IsComplete
        }).ToList();
        _workspaceState.SaveAsync().GetAwaiter().GetResult();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void FocusTimer_Tick(object? sender, EventArgs e)
    {
        if (DateTime.Now >= _focusEndsAt)
        {
            _sessionFinished = true;
            _agentSettings.IsLocked = false;
            _agentSettings.Save();
            _focusTimer.Stop();
        }

        NotifyFocusChanged();
    }

    private void NotifyFocusChanged()
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FocusTimeLabel)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FocusStatus)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ProtectionState)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ProtectionColor)));
    }

    private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    private static string FormatRemaining(TimeSpan remaining)
    {
        if (remaining < TimeSpan.Zero)
        {
            return "00:00:00";
        }

        return remaining.ToString(@"hh\:mm\:ss");
    }
}
