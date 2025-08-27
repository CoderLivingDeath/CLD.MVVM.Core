using System;

namespace MVVM.Dev
{
    public interface IBindableProperty<T>
    {
        T Value { get; set; }
        event EventHandler ValueChanged;
    }
}