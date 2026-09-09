using System;
using System.Windows;
using FocusForge.Infrastructure;

namespace FocusForge.App;

public partial class LoginWindow : Window
{
    private AgentSettings _settings = new();
    private bool _isSetupMode;

    public LoginWindow()
    {
        InitializeComponent();
        Loaded += LoginWindow_Loaded;
    }

    private async void LoginWindow_Loaded(object sender, RoutedEventArgs e)
    {
        _settings = await AgentSettings.LoadAsync();
        
        if (string.IsNullOrEmpty(_settings.UserName) && string.IsNullOrEmpty(_settings.Password))
        {
            _isSetupMode = true;
            SubtitleText.Text = "Welcome to FocusForge";
            InstructionsText.Text = "This is your first time here. Let's set up your profile.";
            ActionButton.Content = "Create Profile";
            
            NameTextBox.Focus();
        }
        else
        {
            _isSetupMode = false;
            SubtitleText.Text = "Welcome back";
            InstructionsText.Text = "Please enter your password to continue.";
            NameTextBox.Text = _settings.UserName;
            NameTextBox.IsEnabled = false;
            ActionButton.Content = "Login";
            
            PasswordBox.Focus();
        }
    }

    private void ActionButton_Click(object sender, RoutedEventArgs e)
    {
        ErrorText.Visibility = Visibility.Collapsed;

        var name = NameTextBox.Text.Trim();
        var pass = PasswordBox.Password;

        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(pass))
        {
            ErrorText.Text = "Name and password cannot be empty.";
            ErrorText.Visibility = Visibility.Visible;
            return;
        }

        if (_isSetupMode)
        {
            _settings.UserName = name;
            _settings.Password = pass;
            _settings.Save();
            
            OpenMainWindow();
        }
        else
        {
            if (pass == _settings.Password)
            {
                OpenMainWindow();
            }
            else
            {
                ErrorText.Text = "Incorrect password.";
                ErrorText.Visibility = Visibility.Visible;
            }
        }
    }

    private void OpenMainWindow()
    {
        var mainWindow = new MainWindow();
        Application.Current.MainWindow = mainWindow;
        mainWindow.Show();
        this.Close();
    }

    private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
        {
            this.DragMove();
        }
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }
}
