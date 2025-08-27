using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVVM;
using MVVM.ConvertersLibrary.Generic;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using static MVVM.tests.BindingManagerTests;

namespace MVVM.tests
{
    [TestClass]
    public class BindingManagerTests
    {

        public class CollectionPropertyBinding : IBinding
        {
            public void Dispose()
            {
                throw new NotImplementedException();
            }
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
            public readonly TextProperty TextProperty;

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

        public abstract class ViewModelBase : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            protected bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
            {
                if (EqualityComparer<T>.Default.Equals(field, value))
                    return false;

                field = value;
                OnPropertyChanged(propertyName);
                return true;
            }

            protected void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public class ViewModel : ViewModelBase
        {
            private ObservableCollection<int> _list = new ObservableCollection<int>();
            public ObservableCollection<int> List
            {
                get => _list;
                set => Set(ref _list, value);
            }

            private string _field = string.Empty;
            public string Field
            {
                get => _field;
                set => Set(ref _field, value);
            }

            private string _submitField = string.Empty;
            public string SubmitField
            {
                get => _submitField;
                set => Set(ref _submitField, value);
            }

            private int _IntValue = 0;
            public int IntValue
            {
                get => _IntValue;
                set => Set(ref _IntValue, value);
            }

            private string _stringValue = string.Empty;
            public string StringValue
            {
                get => _stringValue;
                set => Set(ref _stringValue, value);
            }

            private double _doubleValue = 0.0;
            public double DoubleValue
            {
                get => _doubleValue;
                set => Set(ref _doubleValue, value);
            }

            private float _floatValue = 0f;
            public float FloatValue
            {
                get => _floatValue;
                set => Set(ref _floatValue, value);
            }

            private bool _boolValue = false;
            public bool BoolValue
            {
                get => _boolValue;
                set => Set(ref _boolValue, value);
            }

            private decimal _decimalValue = 0m;
            public decimal DecimalValue
            {
                get => _decimalValue;
                set => Set(ref _decimalValue, value);
            }

            private long _longValue = 0L;
            public long LongValue
            {
                get => _longValue;
                set => Set(ref _longValue, value);
            }

            private short _shortValue = 0;
            public short ShortValue
            {
                get => _shortValue;
                set => Set(ref _shortValue, value);
            }

            private byte _byteValue = 0;
            public byte ByteValue
            {
                get => _byteValue;
                set => Set(ref _byteValue, value);
            }

            private char _charValue = '\0';
            public char CharValue
            {
                get => _charValue;
                set => Set(ref _charValue, value);
            }

            private uint _uintValue = 0;
            public uint UIntValue
            {
                get => _uintValue;
                set => Set(ref _uintValue, value);
            }

            private ulong _ulongValue = 0UL;
            public ulong ULongValue
            {
                get => _ulongValue;
                set => Set(ref _ulongValue, value);
            }

            private ushort _ushortValue = 0;
            public ushort UShortValue
            {
                get => _ushortValue;
                set => Set(ref _ushortValue, value);
            }

            private sbyte _sbyteValue = 0;
            public sbyte SByteValue
            {
                get => _sbyteValue;
                set => Set(ref _sbyteValue, value);
            }

            private DateTime _dateTimeValue = default;
            public DateTime DateTimeValue
            {
                get => _dateTimeValue;
                set => Set(ref _dateTimeValue, value);
            }

            private TimeSpan _timeSpanValue = default;
            public TimeSpan TimeSpanValue
            {
                get => _timeSpanValue;
                set => Set(ref _timeSpanValue, value);
            }
        }

        [TestMethod]
        public void Binding_Valid_PropertToSource()
        {
            BindingManager bindingManager = new BindingManager();
            View view = new View();
            ViewModel viewModel = new ViewModel();

            view.field.Text = "init";
            viewModel.Field = "init";

            bindingManager.Bind(view.field.TextProperty, viewModel, x => x.Field);

            view.field.Text = "NewValue";

            Assert.AreEqual("NewValue", viewModel.Field);
        }

        [TestMethod]
        public void Binding_Valid_SourceToProperty()
        {
            BindingManager bindingManager = new BindingManager();
            View view = new View();
            ViewModel viewModel = new ViewModel();

            view.field.Text = "init";
            viewModel.Field = "init";

            bindingManager.Bind(view.field.TextProperty, viewModel, x => x.Field);

            viewModel.Field = "NewValue";

            Assert.AreEqual("NewValue", view.field.Text);
        }

        [TestMethod]
        public void Binding_Valid_InitBinding()
        {
            BindingManager bindingManager = new BindingManager();
            View view = new View();
            ViewModel viewModel = new ViewModel();

            view.field.Text = "ViewValue";
            viewModel.Field = "SourceValue";

            bindingManager.Bind(view.field.TextProperty, viewModel, x => x.Field);

            Assert.AreEqual("SourceValue", view.field.Text);
        }

        [TestMethod]
        public void Binding_Unbind()
        {
            BindingManager bindingManager = new BindingManager();
            View view = new View();
            ViewModel viewModel = new ViewModel();

            view.field.Text = "ViewValue";
            viewModel.Field = "SourceValue";

            var binding = bindingManager.Bind(view.field.TextProperty, viewModel, x => x.Field);

            Assert.AreEqual("SourceValue", view.field.Text);

            binding.Unbind();

            viewModel.Field = "NewValue";

            Assert.AreEqual("SourceValue", view.field.Text);
        }
    }


    [TestClass]
    public class ValueConverterTests
    {
        [TestMethod]
        public void Binding_Valid_ConvertStringToInt()
        {
            BindingManager bindingManager = new BindingManager();
            View view = new View();
            ViewModel viewModel = new ViewModel();

            view.field.Text = "ViewValue";
            viewModel.IntValue = 1;

            bindingManager.Bind(view.field.TextProperty, viewModel, x => x.IntValue, converter: new IntConverter());

            Assert.AreEqual("1", view.field.Text);
        }

        [TestMethod]
        public void Binding_Valid_ConvertStringToDouble()
        {
            BindingManager bindingManager = new BindingManager();
            View view = new View();
            ViewModel viewModel = new ViewModel();

            view.field.Text = "12.34";
            viewModel.DoubleValue = 42.42;

            bindingManager.Bind(view.field.TextProperty, viewModel, x => x.DoubleValue, converter: new ConvertersLibrary.Generic.DoubleConverter());

            Assert.AreEqual("42.42", view.field.Text);
        }

        [TestMethod]
        public void Binding_Valid_ConvertStringToBool()
        {
            BindingManager bindingManager = new BindingManager();
            View view = new View();
            ViewModel viewModel = new ViewModel();

            view.field.Text = "false";
            viewModel.BoolValue = true;

            bindingManager.Bind(view.field.TextProperty, viewModel, x => x.BoolValue, converter: new BoolConverter());

            Assert.AreEqual("True", view.field.Text);
        }

        [TestMethod]
        public void Binding_Valid_ConvertStringToDateTime()
        {
            BindingManager bindingManager = new BindingManager();
            View view = new View();
            ViewModel viewModel = new ViewModel();

            var date = new DateTime(1997, 1, 1);
            view.field.Text = new DateTime(2025, 8, 27).ToString(CultureInfo.InvariantCulture);
            viewModel.DateTimeValue = date;

            bindingManager.Bind(view.field.TextProperty, viewModel, x => x.DateTimeValue, converter: new ConvertersLibrary.Generic.DateTimeConverter(),
                cultureInfo: CultureInfo.InvariantCulture, ConvertionParam: "MM/dd/yyyy HH:mm:ss");

            Assert.AreEqual(date.ToString(CultureInfo.InvariantCulture), view.field.Text);
        }

        [TestMethod]
        public void Binding_Valid_ConvertStringToDecimal()
        {
            BindingManager bindingManager = new BindingManager();
            View view = new View();
            ViewModel viewModel = new ViewModel();

            view.field.Text = "0";
            viewModel.DecimalValue = 123.456m;

            bindingManager.Bind(view.field.TextProperty, viewModel, x => x.DecimalValue, converter: new ConvertersLibrary.Generic.DecimalConverter());

            Assert.AreEqual("123.456", view.field.Text);
        }

        [TestMethod]
        public void Binding_Valid_ConvertStringToFloat()
        {
            BindingManager bindingManager = new BindingManager();
            View view = new View();
            ViewModel viewModel = new ViewModel();

            view.field.Text = "12.34";
            viewModel.FloatValue = 12.34f;

            bindingManager.Bind(view.field.TextProperty, viewModel, x => x.FloatValue, converter: new FloatConverter());

            Assert.AreEqual("12.34", view.field.Text);
        }

        [TestMethod]
        public void Binding_Valid_ConvertStringToLong()
        {
            BindingManager bindingManager = new BindingManager();
            View view = new View();
            ViewModel viewModel = new ViewModel();

            view.field.Text = "0";
            viewModel.LongValue = 1234567890;

            bindingManager.Bind(view.field.TextProperty, viewModel, x => x.LongValue, converter: new LongConverter());

            Assert.AreEqual("1234567890", view.field.Text);
        }

        [TestMethod]
        public void Binding_Valid_ConvertStringToShort()
        {
            BindingManager bindingManager = new BindingManager();
            View view = new View();
            ViewModel viewModel = new ViewModel();

            view.field.Text = "0";
            viewModel.ShortValue = 123;

            bindingManager.Bind(view.field.TextProperty, viewModel, x => x.ShortValue, converter: new ShortConverter());

            Assert.AreEqual("123", view.field.Text);
        }

        [TestMethod]
        public void Binding_Valid_ConvertStringToByte()
        {
            BindingManager bindingManager = new BindingManager();
            View view = new View();
            ViewModel viewModel = new ViewModel();

            view.field.Text = "0";
            viewModel.ByteValue = 255;

            bindingManager.Bind(view.field.TextProperty, viewModel, x => x.ByteValue, converter: new ConvertersLibrary.Generic.ByteConverter());

            Assert.AreEqual("255", view.field.Text);
        }
    }
}