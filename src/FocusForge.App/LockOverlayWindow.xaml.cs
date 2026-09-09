using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using FocusForge.Infrastructure;

namespace FocusForge.App;

public partial class LockOverlayWindow : Window
{
    private readonly string _targetApp;
    private DispatcherTimer _timer;
    private AgentSettings _settings;
    private bool _isUnlocked = false;

    public LockOverlayWindow(string targetApp)
    {
        InitializeComponent();
        _targetApp = targetApp;
        SubtitleText.Text = $"{targetApp} is currently blocked to help you stay focused.";
        
        _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        _timer.Tick += Timer_Tick;
        
        LoadSettingsAndStart();
    }

    private async void LoadSettingsAndStart()
    {
        _settings = await AgentSettings.LoadAsync();
        UpdateCountdown();
        _timer.Start();
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
        UpdateCountdown();
        
        // Auto-close if session is over or unlocked
        if (!_settings.IsLocked || (_settings.LockUntilUtc.HasValue && DateTimeOffset.UtcNow >= _settings.LockUntilUtc.Value))
        {
            _isUnlocked = true;
            Close();
        }
    }

    private void UpdateCountdown()
    {
        if (_settings == null || !_settings.IsLocked) return;

        if (!_settings.LockUntilUtc.HasValue || _settings.LockUntilUtc.Value == DateTimeOffset.MaxValue)
        {
            CountdownText.Text = "Until I finish";
            return;
        }

        var remaining = _settings.LockUntilUtc.Value - DateTimeOffset.UtcNow;
        if (remaining < TimeSpan.Zero)
        {
            CountdownText.Text = "00:00:00";
        }
        else
        {
            CountdownText.Text = remaining.ToString(@"hh\:mm\:ss");
        }
    }

    private void CloseApp_Click(object sender, RoutedEventArgs e)
    {
        var blocker = new ProcessBlocker();
        blocker.CloseRunning(_targetApp);
        
        _isUnlocked = true;
        Close();
    }

    private void EmergencyUnlock_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(_settings.Password))
        {
            MessageBox.Show("No password is set for Emergency Unlock. You cannot bypass the focus timer.", "Unlock Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        if (EmergencyPasswordBox.Password == _settings.Password)
        {
            _settings.IsLocked = false;
            _settings.Save();
            
            _isUnlocked = true;
            Close();
        }
        else
        {
            MessageBox.Show("Incorrect password.", "Unlock Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            EmergencyPasswordBox.Password = "";
        }
    }

    private void Window_Closing(object sender, CancelEventArgs e)
    {
        if (!_isUnlocked)
        {
            // Prevent Alt-F4 or closing without clicking the buttons
            e.Cancel = true;
        }
    }
}
