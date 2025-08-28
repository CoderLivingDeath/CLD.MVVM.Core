using System;

namespace CLD.MVVM.Core
{
    public class ValueChangedEventArgs<T> : EventArgs
    {
        public T NewValue { get; }

        public ValueChangedEventArgs(T newValue) => NewValue = newValue;
    }

    public interface IBindableProperty<T>
    {
        T Value { get; set; }
        event EventHandler<ValueChangedEventArgs<T>>? ValueChanged;
    }
}