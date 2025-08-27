using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace MVVM.ConvertersLibrary.Generic
{
    // Converter for int
    public class IntConverter : IValueConverter<string, int>
    {
        public int Convert(string value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (int.TryParse(value, NumberStyles.Any, culture, out int result))
            {
                return result;
            }
            throw new FormatException("Cannot convert string to int");
        }

        public string ConvertBack(int value, Type sourceType, object? parameter, CultureInfo culture)
        {
            return value.ToString(culture);
        }
    }

    // Converter for double
    public class DoubleConverter : IValueConverter<string, double>
    {
        public double Convert(string value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (double.TryParse(value, NumberStyles.Any, culture, out double result))
            {
                return result;
            }
            throw new FormatException("Cannot convert string to double");
        }

        public string ConvertBack(double value, Type sourceType, object? parameter, CultureInfo culture)
        {
            return value.ToString(culture);
        }
    }

    // Converter for bool
    public class BoolConverter : IValueConverter<string, bool>
    {
        public bool Convert(string value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (bool.TryParse(value, out bool result))
            {
                return result;
            }
            throw new FormatException("Cannot convert string to bool");
        }

        public string ConvertBack(bool value, Type sourceType, object? parameter, CultureInfo culture)
        {
            return value.ToString(culture);
        }
    }

    // Converter for DateTime
    public class DateTimeConverter : IValueConverter<string, DateTime>
    {
        public DateTime Convert(string value, Type targetType, object? parameter, CultureInfo culture)
        {
            var format = parameter as string;
            if (!string.IsNullOrEmpty(format))
            {
                if (DateTime.TryParseExact(value, format, culture, DateTimeStyles.None, out DateTime result))
                    return result;
                throw new FormatException($"Cannot parse '{value}' with format '{format}'.");
            }
            else
            {
                if (DateTime.TryParse(value, culture, DateTimeStyles.None, out DateTime result))
                    return result;
                throw new FormatException($"Cannot parse '{value}'.");
            }
        }

        public string ConvertBack(DateTime value, Type sourceType, object? parameter, CultureInfo culture)
        {
            string? format = parameter as string;

            if (!string.IsNullOrEmpty(format))
            {
                return value.ToString(format, culture);
            }
            else
            {
                return value.ToString(culture);
            }
        }
    }


    // Converter for decimal
    public class DecimalConverter : IValueConverter<string, decimal>
    {
        public decimal Convert(string value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (decimal.TryParse(value, NumberStyles.Any, culture, out decimal result))
            {
                return result;
            }
            throw new FormatException("Cannot convert string to decimal");
        }

        public string ConvertBack(decimal value, Type sourceType, object? parameter, CultureInfo culture)
        {
            return value.ToString(culture);
        }
    }

    // Converter for float
    public class FloatConverter : IValueConverter<string, float>
    {
        public float Convert(string value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (float.TryParse(value, NumberStyles.Any, culture, out float result))
            {
                return result;
            }
            throw new FormatException("Cannot convert string to float");
        }

        public string ConvertBack(float value, Type sourceType, object? parameter, CultureInfo culture)
        {
            return value.ToString(culture);
        }
    }

    // Converter for long
    public class LongConverter : IValueConverter<string, long>
    {
        public long Convert(string value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (long.TryParse(value, NumberStyles.Any, culture, out long result))
            {
                return result;
            }
            throw new FormatException("Cannot convert string to long");
        }

        public string ConvertBack(long value, Type sourceType, object? parameter, CultureInfo culture)
        {
            return value.ToString(culture);
        }
    }

    // Converter for short
    public class ShortConverter : IValueConverter<string, short>
    {
        public short Convert(string value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (short.TryParse(value, NumberStyles.Any, culture, out short result))
            {
                return result;
            }
            throw new FormatException("Cannot convert string to short");
        }

        public string ConvertBack(short value, Type sourceType, object? parameter, CultureInfo culture)
        {
            return value.ToString(culture);
        }
    }

    // Converter for byte
    public class ByteConverter : IValueConverter<string, byte>
    {
        public byte Convert(string value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (byte.TryParse(value, NumberStyles.Any, culture, out byte result))
            {
                return result;
            }
            throw new FormatException("Cannot convert string to byte");
        }

        public string ConvertBack(byte value, Type sourceType, object? parameter, CultureInfo culture)
        {
            return value.ToString(culture);
        }
    }
}
