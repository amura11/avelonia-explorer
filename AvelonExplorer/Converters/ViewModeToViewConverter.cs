using Avalonia.Data.Converters;
using AvelonExplorer.Enums;
using System;
using System.Globalization;

namespace AvelonExplorer.Converters;

public class ViewModeToViewConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo? culture)
    {
        if (value is FileSystemTabViewMode viewMode)
        {
            return viewMode switch
            {
                FileSystemTabViewMode.Details => "Details",
                FileSystemTabViewMode.Grid => "Grid",
                _ => "Details"
            };
        }
        return "Details";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo? culture)
    {
        throw new NotImplementedException();
    }
}
