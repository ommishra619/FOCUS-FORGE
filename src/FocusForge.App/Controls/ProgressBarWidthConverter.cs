using System;
using System.Globalization;
using System.Windows.Data;

namespace FocusForge.App.Controls;

public class ProgressBarWidthConverter : IMultiValueConverter
{
    public static readonly ProgressBarWidthConverter Instance = new();

    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length == 2 && values[0] is double val && values[1] is double width)
        {
            var w = val * width;
            return w > 0 ? w : 0;
        }
        return 0.0;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
