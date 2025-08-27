using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq.Expressions;

namespace MVVM
{
    public class PropertyBinding : IBinding
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    public class Binder
    {
        public IBinding Bind<TTarget, TSourceValue, TTargetValue>(
            IBindableProperty<TSourceValue> sourceProp,
            TTarget target,
            Expression<Func<TTarget, TTargetValue>> targetProp,
            BindingMode bindingMode = BindingMode.TwoWay,
            IValueConverter converter = null) where TTarget : INotifyPropertyChanged
        {
            throw new NotImplementedException();
        }

        public IBinding BindCollection<TTarget, TSourceValue, TTargetValue>(
            IBindableCollectionProperty<TSourceValue> sourceProp,
            TTarget target,
            Expression<Func<TTarget, TTargetValue>> targetProp,
            BindingMode bindingMode = BindingMode.TwoWay,
            IValueConverter converter = null) where TTarget : INotifyPropertyChanged
        {
            throw new NotImplementedException();
        }
    }

    public class View
    {
        public View()
        {
            field = new Field();
            NumsList = new CollectionProperty<int>();
            SubmitField = new SubmitField();
        }

        public CollectionProperty<int> NumsList { get; }

        public Field field { get; }

        public SubmitField SubmitField { get; }
    }

    public class CollectionProperty<T> : IBindableCollectionProperty<T>
    {
        private ObservableCollection<T> _value = new ObservableCollection<T>();

        public ObservableCollection<T> Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    if (_value != null)
                    {
                        _value.CollectionChanged -= OnCollectionChanged;
                    }

                    _value = value ?? new ObservableCollection<T>();
                    _value.CollectionChanged += OnCollectionChanged;

                    ValueChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event NotifyCollectionChangedEventHandler? CollectionChanged;
        public event EventHandler? ValueChanged;

        public CollectionProperty()
        {
            _value.CollectionChanged += OnCollectionChanged;
        }

        private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }
    }
    public class TextProperty : IBindableProperty<string>
    {
        private string _value = string.Empty;

        public string Value
        {
            get => _value;
            set => SetValue(value);
        }

        public event EventHandler? ValueChanged;

        public TextProperty(string value)
        {
            _value = value;
        }
        public TextProperty()
        {

        }

        public void SetValue(string value)
        {
            if (_value != value)
            {
                _value = value;
                InvokeChange();
            }
        }

        public void InvokeChange()
        {
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public class Field
    {
        private readonly TextProperty TextProperty;

        public event EventHandler? TextChanged;

        public Field()
        {
            TextProperty = new TextProperty();
            TextProperty.ValueChanged += (s, e) => OnTextChanged();
        }

        protected virtual void OnTextChanged()
        {
            TextChanged?.Invoke(this, EventArgs.Empty);
        }

        public string Text
        {
            get => TextProperty.Value;
            set => TextProperty.SetValue(value);
        }
    }

    public class SubmitField
    {
        public TextProperty TextProperty { get; }

        public event EventHandler? TextChanged;

        public SubmitField()
        {
            TextProperty = new TextProperty();
        }

        public void Submit()
        {
            OnTextChanged();
        }

        protected virtual void OnTextChanged()
        {
            TextChanged?.Invoke(this, EventArgs.Empty);
        }

        public string Text
        {
            get => TextProperty.Value;
            set => TextProperty.SetValue(value);
        }
    }
}