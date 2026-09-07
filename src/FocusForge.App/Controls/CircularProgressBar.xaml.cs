using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FocusForge.App.Controls;

public partial class CircularProgressBar : UserControl, INotifyPropertyChanged
{
    public static readonly DependencyProperty ProgressProperty =
        DependencyProperty.Register(nameof(Progress), typeof(double), typeof(CircularProgressBar),
            new PropertyMetadata(0.0, OnProgressChanged));

    public static readonly DependencyProperty StrokeThicknessProperty =
        DependencyProperty.Register(nameof(StrokeThickness), typeof(double), typeof(CircularProgressBar),
            new PropertyMetadata(12.0));

    public static readonly DependencyProperty ProgressBrushProperty =
        DependencyProperty.Register(nameof(ProgressBrush), typeof(Brush), typeof(CircularProgressBar),
            new PropertyMetadata(Brushes.White));

    public static readonly DependencyProperty TextBrushProperty =
        DependencyProperty.Register(nameof(TextBrush), typeof(Brush), typeof(CircularProgressBar),
            new PropertyMetadata(Brushes.White));

    public double Progress
    {
        get => (double)GetValue(ProgressProperty);
        set => SetValue(ProgressProperty, value);
    }

    public double StrokeThickness
    {
        get => (double)GetValue(StrokeThicknessProperty);
        set => SetValue(StrokeThicknessProperty, value);
    }

    public Brush ProgressBrush
    {
        get => (Brush)GetValue(ProgressBrushProperty);
        set => SetValue(ProgressBrushProperty, value);
    }

    public Brush TextBrush
    {
        get => (Brush)GetValue(TextBrushProperty);
        set => SetValue(TextBrushProperty, value);
    }

    public string PercentageText => $"{((int)(Progress * 100))}%";
    
    public Point StartPoint => new Point(Width / 2, StrokeThickness / 2);
    public Size ArcSize => new Size((Width - StrokeThickness) / 2, (Height - StrokeThickness) / 2);
    public bool IsLargeArc => Progress > 0.5;

    public Point EndPoint
    {
        get
        {
            var p = Progress >= 1.0 ? 0.999 : Progress;
            var angle = p * 360 - 90;
            var radians = angle * Math.PI / 180;
            var radius = (Width - StrokeThickness) / 2;
            var x = Width / 2 + radius * Math.Cos(radians);
            var y = Height / 2 + radius * Math.Sin(radians);
            return new Point(x, y);
        }
    }

    public CircularProgressBar()
    {
        InitializeComponent();
        SizeChanged += (s, e) => NotifyAll();
    }

    private static void OnProgressChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is CircularProgressBar bar) bar.NotifyAll();
    }

    private void NotifyAll()
    {
        OnPropertyChanged(nameof(StartPoint));
        OnPropertyChanged(nameof(EndPoint));
        OnPropertyChanged(nameof(ArcSize));
        OnPropertyChanged(nameof(IsLargeArc));
        OnPropertyChanged(nameof(PercentageText));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
