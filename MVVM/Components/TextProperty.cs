using System;

namespace MVVM.Components
{
    public class TextProperty : IBindableProperty<string>
    {
        private readonly object _sync = new object();
        protected string _value = string.Empty;

        string IBindableProperty<string>.Value
        {
            get
            {
                lock (_sync) { return _value; }
            }
            set
            {
                SetValue(value);
            }
        }

        private event EventHandler<ValueChangedEventArgs<string>>? _valueChanged;

        event EventHandler<ValueChangedEventArgs<string>>? IBindableProperty<string>.ValueChanged
        {
            add
            {
                lock (_sync)
                {
                    _valueChanged += value;
                }
            }
            remove
            {
                lock (_sync)
                {
                    _valueChanged -= value;
                }
            }
        }

        public bool SetValue(string newValue, bool emitEvent = true)
        {
            bool changed = false;

            lock (_sync)
            {
                if (string.Equals(_value, newValue, StringComparison.Ordinal))
                    return false;

                _value = newValue;
                changed = true;
            }

            if (changed && emitEvent)
            {
                _valueChanged?.Invoke(this, new ValueChangedEventArgs<string>(newValue));
            }

            return changed;
        }
    }
}
