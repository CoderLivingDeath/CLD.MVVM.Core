using System;
using System.Globalization;

public interface IValueConverter
{
    object Convert(object value, Type targetType, object? parameter, CultureInfo culture);
    object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture);
}

public interface IValueConverter<TSource, TTarget>
{
    TTarget Convert(TSource value, Type targetType, object? parameter, CultureInfo culture);
    TSource ConvertBack(TTarget value, Type sourceType, object? parameter, CultureInfo culture);
}

