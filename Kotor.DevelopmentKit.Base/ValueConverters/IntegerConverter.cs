using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Data.Converters;
using Avalonia;

namespace Kotor.DevelopmentKit.Base.ValueConverters;

public class IntegerConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        Int128? minimum = null;
        Int128? maximum = null;


        if (parameter is string parameters)
        {
            var modifiers = parameters.Split("|").Select(x => x.Replace(" ", "")).ToList();

            var minModifier = modifiers.FirstOrDefault(x => x.StartsWith("Min:"))?.Substring(4);
            if (minModifier is not null && Int128.TryParse(minModifier, out var min))
            {
                minimum = min;
            }

            var maxModifier = modifiers.FirstOrDefault(x => x.StartsWith("Max="))?.Substring(4);
            if (maxModifier is not null && Int128.TryParse(maxModifier, out var max))
            {
                maximum = max;
            }
        }

        if (Int128.TryParse(value?.ToString(), out var result))
        {
            if (minimum is not null && result < minimum)
                result = minimum.Value;
            if (maximum is not null && result > maximum)
                result = maximum.Value;

            return result;
        }

        return minimum ?? 0;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value.ToString();
    }
}
