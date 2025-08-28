using CLD.MVVM.Core.Interfaces;
using System;
using System.Globalization;

namespace CLD.MVVM.Core.ConvertersLibrary.Generic
{
    // Converter for int
    public class IntConverter : IValueConverter<string, int>
    {
        private readonly IFormatProvider? _formatter;

        public IntConverter(IFormatProvider? formatter = null)
        {
            _formatter = formatter;
        }

        public int Convert(string value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (int.TryParse(value, NumberStyles.Any, parameter as IFormatProvider ?? _formatter ?? culture, out int result))
            {
                return result;
            }
            throw new FormatException("Cannot convert string to int");
        }

        public string ConvertBack(int value, Type sourceType, object? parameter, CultureInfo culture)
        {
            return value.ToString(parameter as IFormatProvider ?? _formatter ?? culture);
        }
    }

    // Converter for double
    public class DoubleConverter : IValueConverter<string, double>
    {
        private readonly IFormatProvider? _formatter;

        public DoubleConverter(IFormatProvider? formatter = null)
        {
            _formatter = formatter;
        }

        public double Convert(string value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (double.TryParse(value, NumberStyles.Any, parameter as IFormatProvider ?? _formatter ?? culture, out double result))
            {
                return result;
            }
            throw new FormatException("Cannot convert string to double");
        }

        public string ConvertBack(double value, Type sourceType, object? parameter, CultureInfo culture)
        {
            return value.ToString(parameter as IFormatProvider ?? _formatter ?? culture);
        }
    }

    // Converter for bool
    public class BoolConverter : IValueConverter<string, bool>
    {
        private readonly IFormatProvider? _formatter;

        public BoolConverter(IFormatProvider? formatter = null)
        {
            _formatter = formatter;
        }

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
            return value.ToString(parameter as IFormatProvider ?? _formatter ?? culture);
        }
    }

    // Converter for DateTime
    public class DateTimeConverter : IValueConverter<string, DateTime>
    {
        private readonly IFormatProvider? _formatter;

        public DateTimeConverter(IFormatProvider? formatter = null)
        {
            _formatter = formatter;
        }

        public DateTime Convert(string value, Type targetType, object? parameter, CultureInfo culture)
        {
            var format = parameter as string;
            if (!string.IsNullOrEmpty(format))
            {
                if (DateTime.TryParseExact(value, format, parameter as IFormatProvider ?? _formatter ?? culture, DateTimeStyles.None, out DateTime result))
                    return result;
                throw new FormatException($"Cannot parse '{value}' with format '{format}'.");
            }
            else
            {
                if (DateTime.TryParse(value, parameter as IFormatProvider ?? _formatter ?? culture, DateTimeStyles.None, out DateTime result))
                    return result;
                throw new FormatException($"Cannot parse '{value}'.");
            }
        }

        public string ConvertBack(DateTime value, Type sourceType, object? parameter, CultureInfo culture)
        {
            string? format = parameter as string;

            if (!string.IsNullOrEmpty(format))
            {
                return value.ToString(format, parameter as IFormatProvider ?? _formatter ?? culture);
            }
            else
            {
                return value.ToString(parameter as IFormatProvider ?? _formatter ?? culture);
            }
        }
    }

    // Converter for decimal
    public class DecimalConverter : IValueConverter<string, decimal>
    {
        private readonly IFormatProvider? _formatter;

        public DecimalConverter(IFormatProvider? formatter = null)
        {
            _formatter = formatter;
        }

        public decimal Convert(string value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (decimal.TryParse(value, NumberStyles.Any, parameter as IFormatProvider ?? _formatter ?? culture, out decimal result))
            {
                return result;
            }
            throw new FormatException("Cannot convert string to decimal");
        }

        public string ConvertBack(decimal value, Type sourceType, object? parameter, CultureInfo culture)
        {
            return value.ToString(parameter as IFormatProvider ?? _formatter ?? culture);
        }
    }

    // Converter for float
    public class FloatConverter : IValueConverter<string, float>
    {
        private readonly IFormatProvider? _formatter;

        public FloatConverter(IFormatProvider? formatter = null)
        {
            _formatter = formatter;
        }

        public float Convert(string value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (float.TryParse(value, NumberStyles.Any, parameter as IFormatProvider ?? _formatter ?? culture, out float result))
            {
                return result;
            }
            throw new FormatException("Cannot convert string to float");
        }

        public string ConvertBack(float value, Type sourceType, object? parameter, CultureInfo culture)
        {
            return value.ToString(parameter as IFormatProvider ?? _formatter ?? culture);
        }
    }

    // Converter for long
    public class LongConverter : IValueConverter<string, long>
    {
        private readonly IFormatProvider? _formatter;

        public LongConverter(IFormatProvider? formatter = null)
        {
            _formatter = formatter;
        }

        public long Convert(string value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (long.TryParse(value, NumberStyles.Any, parameter as IFormatProvider ?? _formatter ?? culture, out long result))
            {
                return result;
            }
            throw new FormatException("Cannot convert string to long");
        }

        public string ConvertBack(long value, Type sourceType, object? parameter, CultureInfo culture)
        {
            return value.ToString(parameter as IFormatProvider ?? _formatter ?? culture);
        }
    }

    // Converter for short
    public class ShortConverter : IValueConverter<string, short>
    {
        private readonly IFormatProvider? _formatter;

        public ShortConverter(IFormatProvider? formatter = null)
        {
            _formatter = formatter;
        }

        public short Convert(string value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (short.TryParse(value, NumberStyles.Any, parameter as IFormatProvider ?? _formatter ?? culture, out short result))
            {
                return result;
            }
            throw new FormatException("Cannot convert string to short");
        }

        public string ConvertBack(short value, Type sourceType, object? parameter, CultureInfo culture)
        {
            return value.ToString(parameter as IFormatProvider ?? _formatter ?? culture);
        }
    }

    // Converter for byte
    public class ByteConverter : IValueConverter<string, byte>
    {
        private readonly IFormatProvider? _formatter;

        public ByteConverter(IFormatProvider? formatter = null)
        {
            _formatter = formatter;
        }

        public byte Convert(string value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (byte.TryParse(value, NumberStyles.Any, parameter as IFormatProvider ?? _formatter ?? culture, out byte result))
            {
                return result;
            }
            throw new FormatException("Cannot convert string to byte");
        }

        public string ConvertBack(byte value, Type sourceType, object? parameter, CultureInfo culture)
        {
            return value.ToString(parameter as IFormatProvider ?? _formatter ?? culture);
        }
    }
}