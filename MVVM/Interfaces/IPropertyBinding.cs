using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq.Expressions;

namespace CLD.MVVM.Core.Interfaces
{
    public interface IPropertyBinding<TTargetType, TSource, TSourceType> : IBinding where TSource : INotifyPropertyChanged
    {
        BindingMode BindingMode { get; }
        IValueConverter<TTargetType, TSourceType>? Converter { get; }
        object? ConvertionParam { get; set; }
        CultureInfo CultureInfo { get; set; }
        IBindableProperty<TTargetType> Property { get; }
        TSource Source { get; }
        Expression<Func<TSource, TSourceType>> SourcePropertyExpression { get; }

        void Bind();
        void Dispose();
        void Unbind();
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