using System;

public interface IValueConverter
{
    object Convert(object value, Type targetType);
    object ConvertBack(object value, Type sourceType);
}

