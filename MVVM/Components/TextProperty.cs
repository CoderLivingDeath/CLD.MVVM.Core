using System;

namespace CLD.MVVM.Components
{
    public class TextProperty : IBindableProperty<string>
    {
        private readonly object _sync = new object();
        protected string _value = string.Empty;

        public string Value
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

        public event EventHandler<ValueChangedEventArgs<string>>? ValueChanged;

        event EventHandler<ValueChangedEventArgs<string>>? IBindableProperty<string>.ValueChanged
        {
            add
            {
                lock (_sync)
                {
                    ValueChanged += value;
                }
            }
            remove
            {
                lock (_sync)
                {
                    ValueChanged -= value;
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
                ValueChanged?.Invoke(this, new ValueChangedEventArgs<string>(newValue));
            }

            return changed;
        }
    }
}
