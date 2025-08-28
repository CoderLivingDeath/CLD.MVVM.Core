using CLD.MVVM.Core.Interfaces;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;

namespace CLD.MVVM.Core
{
    public class PropertyBinding<TTargetType, TSource, TSourceType> : IPropertyBinding<TTargetType, TSource, TSourceType>, IBinding, IDisposable where TSource : INotifyPropertyChanged
    {
        public IBindableProperty<TTargetType> Property { get; }

        public TSource Source { get; }

        public Expression<Func<TSource, TSourceType>> SourcePropertyExpression { get; }

        public BindingMode BindingMode { get; }

        public IValueConverter<TTargetType, TSourceType>? Converter { get; }

        public CultureInfo CultureInfo { get; set; } = CultureInfo.CurrentCulture;

        public object? ConvertionParam { get; set; } = null;

        private PropertyInfo? _sourcePropertyInfo;

        private readonly SynchronizationContext? _syncContext;

        private bool _isDisposed;

        public PropertyBinding(
            IBindableProperty<TTargetType> property,
            TSource source,
            Expression<Func<TSource, TSourceType>> sourcePropertyExpression,
            BindingMode bindingMode = BindingMode.TwoWay,
            IValueConverter<TTargetType, TSourceType>? converter = null,
            SynchronizationContext? synchronizationContext = null)
        {
            Property = property ?? throw new ArgumentNullException(nameof(property));
            Source = source ?? throw new ArgumentNullException(nameof(source));
            SourcePropertyExpression = sourcePropertyExpression ?? throw new ArgumentNullException(nameof(sourcePropertyExpression));
            BindingMode = bindingMode;
            Converter = converter;

            _syncContext = synchronizationContext ?? SynchronizationContext.Current;

            _sourcePropertyInfo = GetPropertyInfo(SourcePropertyExpression)
                                  ?? throw new ArgumentException("sourcePropertyExpression must point to a property.");
        }


        private PropertyInfo? GetPropertyInfo(Expression<Func<TSource, TSourceType>> expression)
        {
            if (expression.Body is MemberExpression memberExpr && memberExpr.Member is PropertyInfo pi)
                return pi;

            if (expression.Body is UnaryExpression unaryExpr
                && unaryExpr.NodeType == ExpressionType.Convert
                && unaryExpr.Operand is MemberExpression memberOperand
                && memberOperand.Member is PropertyInfo pi2)
                return pi2;

            return null;
        }

        public void Bind()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(PropertyBinding<TTargetType, TSource, TSourceType>));

            Source.PropertyChanged += Source_PropertyChanged;
            Property.ValueChanged += Property_ValueChanged;

            if (BindingMode == BindingMode.OneWay ||
                BindingMode == BindingMode.TwoWay ||
                BindingMode == BindingMode.OneTime)
            {
                UpdatePropertyFromSource();
            }
        }

        public void Unbind()
        {
            Source.PropertyChanged -= Source_PropertyChanged;
            Property.ValueChanged -= Property_ValueChanged;
        }

        private void Source_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == _sourcePropertyInfo?.Name)
            {
                if (BindingMode == BindingMode.OneWay ||
                    BindingMode == BindingMode.TwoWay ||
                    BindingMode == BindingMode.OneTime)
                {
                    UpdatePropertyFromSource();
                }
            }
        }

        private void Property_ValueChanged(object? sender, EventArgs e)
        {
            if (BindingMode == BindingMode.TwoWay || BindingMode == BindingMode.OneWayToSource)
                UpdateSourceFromProperty();
        }

        private void UpdatePropertyFromSource()
        {
            if (_sourcePropertyInfo == null)
                return;

            var sourceValue = (TSourceType)_sourcePropertyInfo.GetValue(Source)!;

            TTargetType targetValue;

            if (Converter != null)
            {
                targetValue = Converter.ConvertBack(sourceValue, typeof(TTargetType), ConvertionParam, CultureInfo);
            }
            else
            {
                if (sourceValue is TTargetType castedValue)
                    targetValue = castedValue;
                else
                    targetValue = default!;
            }

            void SetValue() => Property.Value = targetValue;

            if (_syncContext != null)
                _syncContext.Post(_ => SetValue(), null);
            else
                SetValue();
        }

        private void UpdateSourceFromProperty()
        {
            if (_sourcePropertyInfo == null)
                return;

            TTargetType targetValue = Property.Value;

            TSourceType sourceValue;

            if (Converter != null)
            {
                sourceValue = Converter.Convert(targetValue, _sourcePropertyInfo.PropertyType, ConvertionParam, CultureInfo);
            }
            else
            {
                if (targetValue is TSourceType castedValue)
                    sourceValue = castedValue;
                else
                    sourceValue = default!;
            }

            void SetValue() => _sourcePropertyInfo.SetValue(Source, sourceValue);

            if (_syncContext != null)
                _syncContext.Post(_ => SetValue(), null);
            else
                SetValue();
        }


        public void Dispose()
        {
            if (_isDisposed)
                return;

            Unbind();
            _isDisposed = true;
        }
    }
}