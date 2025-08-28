using CLD.MVVM.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace CLD.MVVM.Components
{
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
}
