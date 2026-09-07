using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FocusForge.App.Models;

public sealed class WeightEntry : INotifyPropertyChanged
{
    private DateTimeOffset _recordedAt;
    private double _weight;
    private string _note = string.Empty;

    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTimeOffset RecordedAt { get => _recordedAt; set => SetField(ref _recordedAt, value); }
    public double Weight { get => _weight; set => SetField(ref _weight, value); }
    public string Note { get => _note; set => SetField(ref _note, value); }
    public string DisplayDate => RecordedAt.ToLocalTime().ToString("MMM d, yyyy");

    public event PropertyChangedEventHandler? PropertyChanged;

    private void SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return;
        }

        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        if (propertyName == nameof(RecordedAt))
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DisplayDate)));
        }
    }
}