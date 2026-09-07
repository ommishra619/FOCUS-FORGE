using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FocusForge.App.Models;

public sealed class HabitModel : INotifyPropertyChanged
{
    private string _title = string.Empty;
    private string _accentKey = "TaskWorkAccent";

    public Guid Id { get; init; } = Guid.NewGuid();
    
    public string Title 
    { 
        get => _title; 
        set => SetField(ref _title, value); 
    }
    
    public string AccentKey 
    { 
        get => _accentKey; 
        set 
        {
            SetField(ref _accentKey, value);
            OnPropertyChanged(nameof(Accent));
        }
    }
    
    public string Accent => ((System.Windows.Media.SolidColorBrush)System.Windows.Application.Current.Resources[_accentKey])?.Color.ToString() ?? "#059669";
    
    public ObservableCollection<ContributionDay> Heatmap { get; } = new();

    public void RefreshTheme()
    {
        OnPropertyChanged(nameof(Accent));
        foreach (var day in Heatmap)
        {
            day.RefreshTheme();
        }
    }

    private void SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (System.Collections.Generic.EqualityComparer<T>.Default.Equals(field, value)) return;
        field = value;
        OnPropertyChanged(propertyName);
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public event PropertyChangedEventHandler? PropertyChanged;
}

public sealed class ContributionDay : INotifyPropertyChanged
{
    private bool _isCompleted;
    private HabitModel _parent;

    public ContributionDay(HabitModel parent)
    {
        _parent = parent;
    }

    public DateTime Date { get; init; }
    
    public bool IsCompleted 
    { 
        get => _isCompleted; 
        set 
        {
            if (_isCompleted == value) return;
            _isCompleted = value;
            OnPropertyChanged(nameof(IsCompleted));
            OnPropertyChanged(nameof(Color));
        }
    }
    
    // We bind the square's Fill to this property
    public string Color => _isCompleted 
        ? _parent.Accent 
        : ((System.Windows.Media.SolidColorBrush)System.Windows.Application.Current.Resources["GhostButtonBorderBrush"])?.Color.ToString() ?? "#E5E7EB";

    public string TooltipText => $"{Date:MMM d, yyyy}";

    public void RefreshTheme()
    {
        OnPropertyChanged(nameof(Color));
    }

    private void OnPropertyChanged(string propertyName)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public event PropertyChangedEventHandler? PropertyChanged;
}
