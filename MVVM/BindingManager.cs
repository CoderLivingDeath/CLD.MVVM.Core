using CLD.MVVM.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq.Expressions;
using System.Threading;

namespace CLD.MVVM
{
    public class BindingManager : IDisposable
    {
        public HashSet<IBinding> Bindings { get; } = new HashSet<IBinding>();

        private bool _disposed = false;

        private readonly SynchronizationContext? _syncContext;

        public BindingManager(SynchronizationContext? syncContext = null)
        {
            _syncContext = syncContext;
        }


        public IPropertyBinding<TTargetType, TSource, TSourceType> Bind<TTargetType, TSource, TSourceType>(
            IBindableProperty<TTargetType> property,
            TSource source,
            Expression<Func<TSource, TSourceType>> dependencyProperty,
            BindingMode bindingMode = BindingMode.TwoWay,
            IValueConverter<TTargetType, TSourceType>? converter = null,
            CultureInfo? cultureInfo = null,
            object? ConvertionParam = null) where TSource : INotifyPropertyChanged
        {
            if (_disposed) throw new ObjectDisposedException(nameof(BindingManager));

            if (property == null) throw new ArgumentNullException(nameof(property));
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (dependencyProperty == null) throw new ArgumentNullException(nameof(dependencyProperty));

            var binding = new PropertyBinding<TTargetType, TSource, TSourceType>(property, source, dependencyProperty, bindingMode, converter, _syncContext);
            binding.CultureInfo = cultureInfo ?? CultureInfo.InvariantCulture;
            binding.ConvertionParam = ConvertionParam;

            binding.Bind();

            Bindings.Add(binding);

            return binding;
        }

        /// <summary>
        /// Освобождает все привязки и очищает коллекцию.
        /// </summary>
        public void DisposeAll()
        {
            if (_disposed) return;

            foreach (var binding in Bindings)
            {
                binding.Dispose();
            }
            Bindings.Clear();
        }

        /// <summary>
        /// Закрывает менеджер и освобождает ресурсы.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                // Освобождаем управляемые ресурсы
                DisposeAll();
            }

            // Нет неуправляемых ресурсов — ничего дополнительно не нужно

            _disposed = true;
        }
    }



    //public class PropertyBinding<TTarget, TSource> : IBinding, IDisposable
    //    where TSource : INotifyPropertyChanged
    //{
    //    public IBindableProperty<TTarget> Property { get; }
    //    public TSource Source { get; }
    //    public Expression<Func<TSource, TTarget>> DependencyProperty { get; }
    //    public BindingMode BindingMode { get; }
    //    public IValueConverter? Converter { get; }

    //    private PropertyInfo? _propertyInfo;
    //    private bool _isDisposed;

    //    public PropertyBinding(
    //        IBindableProperty<TTarget> property,
    //        TSource source,
    //        Expression<Func<TSource, TTarget>> dependencyProperty,
    //        BindingMode bindingMode = BindingMode.TwoWay,
    //        IValueConverter? converter = null)
    //    {
    //        Property = property ?? throw new ArgumentNullException(nameof(property));
    //        Source = source ?? throw new ArgumentNullException(nameof(source));
    //        DependencyProperty = dependencyProperty ?? throw new ArgumentNullException(nameof(dependencyProperty));
    //        BindingMode = bindingMode;
    //        Converter = converter;

    //        _propertyInfo = GetPropertyInfo(DependencyProperty)
    //            ?? throw new ArgumentException("dependencyProperty must point to a property.");

    //        Bind();
    //    }

    //    private PropertyInfo? GetPropertyInfo(Expression<Func<TSource, TTarget>> expr)
    //    {
    //        if (expr.Body is MemberExpression memberExpr && memberExpr.Member is PropertyInfo pi)
    //            return pi;
    //        if (expr.Body is UnaryExpression unaryExpr && unaryExpr.NodeType == ExpressionType.Convert
    //            && unaryExpr.Operand is MemberExpression memberOperand && memberOperand.Member is PropertyInfo pi2)
    //            return pi2;
    //        return null;
    //    }

    //    private PropertyInfo? GetPropertyInfoFromExpression(Expression<Func<INotifyPropertyChanged, TTarget>> expression)
    //    {
    //        Ожидается x => ((MyType)x).Property
    //            if (expression.Body is MemberExpression memberExpr)
    //        {
    //            if (memberExpr.Member is PropertyInfo propertyInfo)
    //                return propertyInfo;
    //        }
    //        else if (expression.Body is UnaryExpression unaryExpr &&
    //                 unaryExpr.NodeType == ExpressionType.Convert &&
    //                 unaryExpr.Operand is MemberExpression memberOperand &&
    //                 memberOperand.Member is PropertyInfo propertyInfo)
    //        {
    //            return propertyInfo;
    //        }

    //        return null;
    //    }

    //    private void Bind()
    //    {
    //        if (_isDisposed)
    //            throw new ObjectDisposedException(nameof(PropertyBinding<TTarget>));

    //        Source.PropertyChanged += Source_PropertyChanged;
    //        Property.ValueChanged += Property_ValueChanged;

    //        Начальное обновление Property.Value из Source, если BindingMode соответствует
    //            if (BindingMode == BindingMode.OneWay || BindingMode == BindingMode.TwoWay || BindingMode == BindingMode.OneTime)
    //        {
    //            UpdatePropertyFromSource();
    //        }
    //    }

    //    private void Unbind()
    //    {
    //        Source.PropertyChanged -= Source_PropertyChanged;
    //        Property.ValueChanged -= Property_ValueChanged;
    //    }

    //    private void Source_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    //    {
    //        if (e.PropertyName == _targetPropertyInfo?.Name)
    //        {
    //            if (BindingMode == BindingMode.OneWay || BindingMode == BindingMode.TwoWay || BindingMode == BindingMode.OneTime)
    //            {
    //                UpdatePropertyFromSource();
    //            }
    //        }
    //    }

    //    private void Property_ValueChanged(object? sender, EventArgs e)
    //    {
    //        if (BindingMode == BindingMode.TwoWay || BindingMode == BindingMode.OneWayToSource)
    //        {
    //            UpdateSourceFromProperty();
    //        }
    //    }

    //    private void UpdatePropertyFromSource()
    //    {
    //        if (_targetPropertyInfo == null)
    //            return;

    //        var sourceValue = _targetPropertyInfo.GetValue(Source);

    //        if (Converter != null)
    //        {
    //            sourceValue = Converter.Convert(sourceValue!, typeof(TTarget), null, CultureInfo.CurrentCulture);
    //        }

    //        if (sourceValue is TTarget typedValue)
    //        {
    //            Property.Value = typedValue;
    //        }
    //        else if (sourceValue == null && default(TTarget) == null)
    //        {
    //            Property.Value = default!;
    //        }
    //        Иначе просто игнорируем, можно добавить обработку ошибок по необходимости
    //        }

    //    private void UpdateSourceFromProperty()
    //    {
    //        if (_targetPropertyInfo == null)
    //            return;

    //        object? value = Property.Value;

    //        if (Converter != null)
    //        {
    //            value = Converter.ConvertBack(value!, _targetPropertyInfo.PropertyType, null, CultureInfo.CurrentCulture);
    //        }

    //        _targetPropertyInfo.SetValue(Source, value);
    //    }

    //    public void Dispose()
    //    {
    //        if (_isDisposed)
    //            return;

    //        Unbind();
    //        _isDisposed = true;
    //    }
    //}

}