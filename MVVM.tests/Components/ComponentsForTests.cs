using MVVM.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.tests.Components
{
    public abstract class ViewModelBase : IViewMode
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public bool Set<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        private void OnPropertyChanged(string? propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class CollectionProperty<T> : IBindableCollectionProperty<T>
    {
        private readonly object _sync = new object();

        private ObservableCollection<T> _value = new ObservableCollection<T>();

        public ObservableCollection<T> Value
        {
            get
            {
                lock (_sync)
                {
                    return _value;
                }
            }
            set
            {
                SetValue(value);
            }
        }

        private event NotifyCollectionChangedEventHandler? _collectionChanged;
        private event EventHandler<ValueChangedEventArgs<ObservableCollection<T>>>? _valueChanged;

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add
            {
                lock (_sync)
                {
                    _collectionChanged += value;
                }
            }
            remove
            {
                lock (_sync)
                {
                    _collectionChanged -= value;
                }
            }
        }

        public event EventHandler<ValueChangedEventArgs<ObservableCollection<T>>>? ValueChanged
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

        public CollectionProperty()
        {
            HookCollection(_value);
        }

        private void HookCollection(ObservableCollection<T> collection)
        {
            collection.CollectionChanged += OnCollectionChanged;
        }

        private void UnhookCollection(ObservableCollection<T> collection)
        {
            collection.CollectionChanged -= OnCollectionChanged;
        }

        private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyCollectionChangedEventHandler? handlers;

            lock (_sync)
            {
                handlers = _collectionChanged;
            }

            handlers?.Invoke(this, e);
        }

        public bool SetValue(ObservableCollection<T> newValue, bool emitEvent = true)
        {
            if (ReferenceEquals(_value, newValue))
                return false;

            // Отписываемся от старой коллекции
            UnhookCollection(_value);

            lock (_sync)
            {
                _value = newValue ?? new ObservableCollection<T>();
                HookCollection(_value);
            }

            if (emitEvent)
            {
                EventHandler<ValueChangedEventArgs<ObservableCollection<T>>>? handlers;

                lock (_sync)
                {
                    handlers = _valueChanged;
                }

                handlers?.Invoke(this, new ValueChangedEventArgs<ObservableCollection<T>>(_value));
            }

            return true;
        }
    }

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
