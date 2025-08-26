public interface IValueConverter
{
    /// <summary>
    /// Converts a value from the source type to the target type.
    /// </summary>
    /// <param name="value">The value produced by the binding source. Can be null.</param>
    /// <returns>The converted value. Can be null.</returns>
    object? Convert(object? value);

    /// <summary>
    /// Converts a value from the target type back to the source type.
    /// </summary>
    /// <param name="value">The value produced by the binding target. Can be null.</param>
    /// <returns>The converted value. Can be null.</returns>
    object? ConvertBack(object? value);
}
