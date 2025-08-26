using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading;

public class PropertyBinding : IDisposable
{
    private readonly INotifyPropertyChanged? _sourceNotifier;
    private readonly INotifyPropertyChanged? _targetNotifier;
    private readonly object _source;
    private readonly object _target;
    private readonly PropertyInfo _sourcePropertyInfo;
    private readonly PropertyInfo _targetPropertyInfo;
    private readonly BindingWay _mode;
    private readonly IValueConverter? _converter;

    private readonly SynchronizationContext? _syncContext;

    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the PropertyBinding class.
    /// Sets up property binding between source and target objects according to the specified mode and optional converter.
    /// </summary>
    /// <param name="source">Source object containing the source property.</param>
    /// <param name="sourcePropertyInfo">PropertyInfo of the source property.</param>
    /// <param name="target">Target object containing the target property.</param>
    /// <param name="targetPropertyInfo">PropertyInfo of the target property.</param>
    /// <param name="mode">Binding mode (OneWay, TwoWay, OneWayToSource, etc.).</param>
    /// <param name="converter">Optional value converter to convert between source and target property types.</param>
    /// <exception cref="ArgumentException">Thrown if properties are not writable or types are incompatible (without converter).</exception>
    public PropertyBinding(
        object source,
        PropertyInfo sourcePropertyInfo,
        object target,
        PropertyInfo targetPropertyInfo,
        BindingWay mode,
        IValueConverter? converter = null)
    {
        _source = source ?? throw new ArgumentNullException(nameof(source));
        _target = target ?? throw new ArgumentNullException(nameof(target));
        _sourcePropertyInfo = sourcePropertyInfo ?? throw new ArgumentNullException(nameof(sourcePropertyInfo));
        _targetPropertyInfo = targetPropertyInfo ?? throw new ArgumentNullException(nameof(targetPropertyInfo));
        _mode = mode;
        _converter = converter;

        _syncContext = SynchronizationContext.Current;

        if (!_targetPropertyInfo.CanWrite)
            throw new ArgumentException($"Property '{_targetPropertyInfo.Name}' on target object is read-only", nameof(targetPropertyInfo));

        if (((_mode & BindingWay.TwoWay) == BindingWay.TwoWay || (_mode & BindingWay.OneWayToSource) == BindingWay.OneWayToSource)
            && !_sourcePropertyInfo.CanWrite)
            throw new ArgumentException($"Property '{_sourcePropertyInfo.Name}' on source object is read-only, cannot do TwoWay or OneWayToSource binding", nameof(sourcePropertyInfo));

        CheckPropertyTypes();

        _sourceNotifier = source as INotifyPropertyChanged;
        _targetNotifier = target as INotifyPropertyChanged;

        if (_sourceNotifier != null && ((_mode & BindingWay.OneWay) == BindingWay.OneWay || (_mode & BindingWay.TwoWay) == BindingWay.TwoWay))
            _sourceNotifier.PropertyChanged += OnSourcePropertyChanged;

        if (_targetNotifier != null && ((_mode & BindingWay.TwoWay) == BindingWay.TwoWay || (_mode & BindingWay.OneWayToSource) == BindingWay.OneWayToSource))
            _targetNotifier.PropertyChanged += OnTargetPropertyChanged;

        if ((_mode & BindingWay.OneWay) == BindingWay.OneWay || (_mode & BindingWay.TwoWay) == BindingWay.TwoWay || _mode == BindingWay.None)
            UpdateTarget();
        else if ((_mode & BindingWay.OneWayToSource) == BindingWay.OneWayToSource)
            UpdateSource();
    }

    /// <summary>
    /// Checks property type compatibility between source and target, throws if incompatible and no converter is provided.
    /// </summary>
    private void CheckPropertyTypes()
    {
        var sourceType = _sourcePropertyInfo.PropertyType;
        var targetType = _targetPropertyInfo.PropertyType;

        if (_converter == null)
        {
            if (!targetType.IsAssignableFrom(sourceType) && !sourceType.IsAssignableFrom(targetType))
                throw new ArgumentException($"Source property type '{sourceType.FullName}' and target property type '{targetType.FullName}' are not compatible and no converter is provided.");
        }
    }

    /// <summary>
    /// Safely invokes the specified action on the synchronization context if available, or synchronously otherwise.
    /// Skips invocation if disposed.
    /// </summary>
    /// <param name="action">The action to invoke.</param>
    private void SafeInvoke(Action action)
    {
        if (_disposed) return;

        if (_syncContext != null)
        {
            _syncContext.Post(_ =>
            {
                if (_disposed) return;
                try
                {
                    action();
                }
                catch
                {
                    // Suppress exceptions to avoid crashes during binding updates.
                }
            }, null);
        }
        else
        {
            try
            {
                action();
            }
            catch
            {
                // Suppress exceptions.
            }
        }
    }

    private void OnSourcePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (_disposed) return;

        if (e.PropertyName == _sourcePropertyInfo.Name || string.IsNullOrEmpty(e.PropertyName))
        {
            if (((_mode & BindingWay.OneWay) == BindingWay.OneWay) || ((_mode & BindingWay.TwoWay) == BindingWay.TwoWay))
                SafeInvoke(UpdateTarget);
        }
    }

    private void OnTargetPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (_disposed) return;

        if (e.PropertyName == _targetPropertyInfo.Name || string.IsNullOrEmpty(e.PropertyName))
        {
            if (((_mode & BindingWay.TwoWay) == BindingWay.TwoWay) || ((_mode & BindingWay.OneWayToSource) == BindingWay.OneWayToSource))
                SafeInvoke(UpdateSource);
        }
    }

    /// <summary>
    /// Updates the target property value from the source property.
    /// Applies converter if provided.
    /// </summary>
    private void UpdateTarget()
    {
        try
        {
            var value = _sourcePropertyInfo.GetValue(_source);
            if (_converter != null)
                value = _converter.Convert(value!);

            var currentTargetValue = _targetPropertyInfo.GetValue(_target);

            if (!Equals(value, currentTargetValue))
                _targetPropertyInfo.SetValue(_target, value);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to update target property '{_targetPropertyInfo.Name}'", ex);
        }
    }

    /// <summary>
    /// Updates the source property value from the target property.
    /// Applies converter back if provided.
    /// </summary>
    private void UpdateSource()
    {
        try
        {
            var value = _targetPropertyInfo.GetValue(_target);
            if (_converter != null)
                value = _converter.ConvertBack(value!);

            var currentSourceValue = _sourcePropertyInfo.GetValue(_source);

            if (!Equals(value, currentSourceValue))
                _sourcePropertyInfo.SetValue(_source, value);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to update source property '{_sourcePropertyInfo.Name}'", ex);
        }
    }

    /// <summary>
    /// Releases all resources used by this instance and unsubscribes from events.
    /// </summary>
    /// <param name="disposing">True if called from Dispose; false if called from finalizer.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            if (_sourceNotifier != null)
                _sourceNotifier.PropertyChanged -= OnSourcePropertyChanged;

            if (_targetNotifier != null)
                _targetNotifier.PropertyChanged -= OnTargetPropertyChanged;
        }

        _disposed = true;
    }

    /// <summary>
    /// Releases all resources used by this instance.
    /// </summary>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}

