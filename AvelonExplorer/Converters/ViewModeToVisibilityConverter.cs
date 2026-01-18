using Avalonia.Data.Converters;
using AvelonExplorer.Enums;
using System;
using System.Globalization;

namespace AvelonExplorer.Converters;

public class ViewModeToVisibilityConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo? culture)
    {
        if (value is FileSystemTabViewMode viewMode && parameter is string paramMode)
        {
            if (Enum.TryParse<FileSystemTabViewMode>(paramMode, out var targetMode))
            {
                return viewMode == targetMode;
            }
        }
        return false;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo? culture)
    {
        throw new NotImplementedException();
    }
}
