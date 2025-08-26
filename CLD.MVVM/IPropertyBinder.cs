using System.Linq.Expressions;

public interface IPropertyBinder : IDisposable
{
    IBinding Bind<TSource, TTarget>(
        TSource source,
        Expression<Func<TSource, object>> sourceProperty,
        TTarget target,
        Expression<Func<TTarget, object>> targetProperty,
        BindingWay mode = BindingWay.TwoWay,
        IValueConverter converter = null);  // Добавлен конвертер

    void Unbind(IBinding binding);
}
