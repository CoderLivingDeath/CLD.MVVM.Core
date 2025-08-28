using CLD.MVVM.Core.Interfaces;
using System;
using System.Globalization;

namespace CLD.MVVM.Core.ConvertersLibrary
{
    // Converter for int
    public class IntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string str && int.TryParse(str, NumberStyles.Any, culture, out int result))
            {
                return result;
            }
            throw new FormatException("Cannot convert string to int");
        }

        public object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is int intValue)
            {
                return intValue.ToString(culture);
            }
            throw new FormatException("Cannot convert value to string");
        }
    }

    // Converter for double
    public class DoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string str && double.TryParse(str, NumberStyles.Any, culture, out double result))
            {
                return result;
            }
            throw new FormatException("Cannot convert string to double");
        }

        public object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is double doubleValue)
            {
                return doubleValue.ToString(culture);
            }
            throw new FormatException("Cannot convert value to string");
        }
    }

    // Converter for bool
    public class BoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string str && bool.TryParse(str, out bool result))
            {
                return result;
            }
            throw new FormatException("Cannot convert string to bool");
        }

        public object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue.ToString(culture);
            }
            throw new FormatException("Cannot convert value to string");
        }
    }

    // Converter for DateTime
    public class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string str && DateTime.TryParse(str, culture, DateTimeStyles.None, out DateTime result))
            {
                return result;
            }
            throw new FormatException("Cannot convert string to DateTime");
        }

        public object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is DateTime dateTimeValue)
            {
                var format = parameter as string;

                if (!string.IsNullOrEmpty(format))
                {
                    return dateTimeValue.ToString(format, culture);
                }
                else
                {
                    return dateTimeValue.ToString(culture);
                }
            }
            throw new FormatException("Cannot convert value to string");
        }

    }

    // Converter for decimal
    public class DecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string str && decimal.TryParse(str, NumberStyles.Any, culture, out decimal result))
            {
                return result;
            }
            throw new FormatException("Cannot convert string to decimal");
        }

        public object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is decimal decimalValue)
            {
                return decimalValue.ToString(culture);
            }
            throw new FormatException("Cannot convert value to string");
        }
    }

    // Converter for float
    public class FloatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string str && float.TryParse(str, NumberStyles.Any, culture, out float result))
            {
                return result;
            }
            throw new FormatException("Cannot convert string to float");
        }

        public object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is float floatValue)
            {
                return floatValue.ToString(culture);
            }
            throw new FormatException("Cannot convert value to string");
        }
    }

    // Converter for long
    public class LongConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string str && long.TryParse(str, NumberStyles.Any, culture, out long result))
            {
                return result;
            }
            throw new FormatException("Cannot convert string to long");
        }

        public object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is long longValue)
            {
                return longValue.ToString(culture);
            }
            throw new FormatException("Cannot convert value to string");
        }
    }

    // Converter for short
    public class ShortConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string str && short.TryParse(str, NumberStyles.Any, culture, out short result))
            {
                return result;
            }
            throw new FormatException("Cannot convert string to short");
        }

        public object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is short shortValue)
            {
                return shortValue.ToString(culture);
            }
            throw new FormatException("Cannot convert value to string");
        }
    }

    // Converter for byte
    public class ByteConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string str && byte.TryParse(str, NumberStyles.Any, culture, out byte result))
            {
                return result;
            }
            throw new FormatException("Cannot convert string to byte");
        }

        public object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is byte byteValue)
            {
                return byteValue.ToString(culture);
            }
            throw new FormatException("Cannot convert value to string");
        }
    }
}
