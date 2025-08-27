using System;

namespace MVVM
{
    public interface IBindableProperty<T>
    {
        T Value { get; set; }
        event EventHandler ValueChanged;
    }
}