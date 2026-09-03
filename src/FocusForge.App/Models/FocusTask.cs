using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FocusForge.App.Models;

public sealed class FocusTask : INotifyPropertyChanged
{
    private bool _isComplete;

    public string Title { get; init; } = string.Empty;
    public string Detail { get; init; } = string.Empty;
    public string TimeLabel { get; init; } = string.Empty;
    public string Accent { get; init; } = "#7EE7C7";

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
