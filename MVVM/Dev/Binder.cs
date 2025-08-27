using MVVM.Dev;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

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
        }

        public Field field { get; }
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
        }

        protected virtual void OnTextChanged()
        {
            TextChanged?.Invoke(this, EventArgs.Empty);
        }

        public string Text { get => TextProperty.Value; set => TextProperty.SetValue(value); }
    }
}