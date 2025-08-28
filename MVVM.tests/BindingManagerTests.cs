using CLD.MVVM.ConvertersLibrary.Generic;
using CLD.MVVM.Tests.Components;
using System.Collections.ObjectModel;
using System.Globalization;

namespace CLD.MVVM.Tests
{
    public class View
    {
        public View(TextProperty textProperty, CollectionProperty<int> numsList)
        {
            _textProperty = textProperty;
            _numsList = numsList;
        }

        public View()
        {
            _textProperty = new();
            _numsList = new();
        }

        private TextProperty _textProperty { get; }
        private CollectionProperty<int> _numsList { get; }

        public TextProperty TextProperty => _textProperty;
        public CollectionProperty<int> NumsList => _numsList;
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

    [TestClass]
    public class BindingManagerTests
    {
        [TestMethod]
        public void Binding_Valid_PropertyToSource()
        {
            BindingManager bindingManager = new BindingManager();
            View view = new View();
            ViewModel viewModel = new ViewModel();

            view.TextProperty.Value = "init";
            viewModel.Field = "init";

            bindingManager.Bind(view.TextProperty, viewModel, x => x.Field);

            view.TextProperty.Value = "NewValue";

            Assert.AreEqual("NewValue", viewModel.Field);
        }

        [TestMethod]
        public void Binding_Valid_SourceToProperty()
        {
            BindingManager bindingManager = new BindingManager();
            View view = new View();
            ViewModel viewModel = new ViewModel();

            view.TextProperty.Value = "init";
            viewModel.Field = "init";

            bindingManager.Bind(view.TextProperty, viewModel, x => x.Field);

            viewModel.Field = "NewValue";

            Assert.AreEqual("NewValue", view.TextProperty.Value);
        }

        [TestMethod]
        public void Binding_Valid_InitBinding()
        {
            BindingManager bindingManager = new BindingManager();
            View view = new View();
            ViewModel viewModel = new ViewModel();

            view.TextProperty.Value = "ViewValue";
            viewModel.Field = "SourceValue";

            bindingManager.Bind(view.TextProperty, viewModel, x => x.Field);

            Assert.AreEqual("SourceValue", view.TextProperty.Value);
        }

        [TestMethod]
        public void Binding_Unbind()
        {
            BindingManager bindingManager = new BindingManager();
            View view = new View();
            ViewModel viewModel = new ViewModel();

            view.TextProperty.Value = "ViewValue";
            viewModel.Field = "SourceValue";

            var binding = bindingManager.Bind(view.TextProperty, viewModel, x => x.Field);

            Assert.AreEqual("SourceValue", view.TextProperty.Value);

            binding.Unbind();

            viewModel.Field = "NewValue";

            Assert.AreEqual("SourceValue", view.TextProperty.Value);
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

            view.TextProperty.Value = "ViewValue";
            viewModel.IntValue = 1;

            bindingManager.Bind(view.TextProperty, viewModel, x => x.IntValue, converter: new IntConverter());

            Assert.AreEqual("1", view.TextProperty.Value);
        }

        [TestMethod]
        public void Binding_Valid_ConvertStringToDouble()
        {
            BindingManager bindingManager = new BindingManager();
            View view = new View();
            ViewModel viewModel = new ViewModel();

            view.TextProperty.Value = "12.34";
            viewModel.DoubleValue = 42.42;

            bindingManager.Bind(view.TextProperty, viewModel, x => x.DoubleValue, converter: new ConvertersLibrary.Generic.DoubleConverter());

            Assert.AreEqual("42.42", view.TextProperty.Value);
        }

        [TestMethod]
        public void Binding_Valid_ConvertStringToBool()
        {
            BindingManager bindingManager = new BindingManager();
            View view = new View();
            ViewModel viewModel = new ViewModel();

            view.TextProperty.Value = "false";
            viewModel.BoolValue = true;

            bindingManager.Bind(view.TextProperty, viewModel, x => x.BoolValue, converter: new BoolConverter());

            Assert.AreEqual("True", view.TextProperty.Value);
        }

        [TestMethod]
        public void Binding_Valid_ConvertStringToDateTime()
        {
            BindingManager bindingManager = new BindingManager();
            View view = new View();
            ViewModel viewModel = new ViewModel();

            var date = new DateTime(1997, 1, 1);
            view.TextProperty.Value = new DateTime(2025, 8, 27).ToString(CultureInfo.InvariantCulture);
            viewModel.DateTimeValue = date;

            bindingManager.Bind(view.TextProperty, viewModel, x => x.DateTimeValue, converter: new ConvertersLibrary.Generic.DateTimeConverter(),
                cultureInfo: CultureInfo.InvariantCulture, ConvertionParam: "MM/dd/yyyy HH:mm:ss");

            Assert.AreEqual(date.ToString(CultureInfo.InvariantCulture), view.TextProperty.Value);
        }

        [TestMethod]
        public void Binding_Valid_ConvertStringToDecimal()
        {
            BindingManager bindingManager = new BindingManager();
            View view = new View();
            ViewModel viewModel = new ViewModel();

            view.TextProperty.Value = "0";
            viewModel.DecimalValue = 123.456m;

            bindingManager.Bind(view.TextProperty, viewModel, x => x.DecimalValue, converter: new ConvertersLibrary.Generic.DecimalConverter());

            Assert.AreEqual("123.456", view.TextProperty.Value);
        }

        [TestMethod]
        public void Binding_Valid_ConvertStringToFloat()
        {
            BindingManager bindingManager = new BindingManager();
            View view = new View();
            ViewModel viewModel = new ViewModel();

            view.TextProperty.Value = "12.34";
            viewModel.FloatValue = 12.34f;

            bindingManager.Bind(view.TextProperty, viewModel, x => x.FloatValue, converter: new FloatConverter());

            Assert.AreEqual("12.34", view.TextProperty.Value);
        }

        [TestMethod]
        public void Binding_Valid_ConvertStringToLong()
        {
            BindingManager bindingManager = new BindingManager();
            View view = new View();
            ViewModel viewModel = new ViewModel();

            view.TextProperty.Value = "0";
            viewModel.LongValue = 1234567890;

            bindingManager.Bind(view.TextProperty, viewModel, x => x.LongValue, converter: new LongConverter());

            Assert.AreEqual("1234567890", view.TextProperty.Value);
        }

        [TestMethod]
        public void Binding_Valid_ConvertStringToShort()
        {
            BindingManager bindingManager = new BindingManager();
            View view = new View();
            ViewModel viewModel = new ViewModel();

            view.TextProperty.Value = "0";
            viewModel.ShortValue = 123;

            bindingManager.Bind(view.TextProperty, viewModel, x => x.ShortValue, converter: new ShortConverter());

            Assert.AreEqual("123", view.TextProperty.Value);
        }

        [TestMethod]
        public void Binding_Valid_ConvertStringToByte()
        {
            BindingManager bindingManager = new BindingManager();
            View view = new View();
            ViewModel viewModel = new ViewModel();

            view.TextProperty.Value = "0";
            viewModel.ByteValue = 255;

            bindingManager.Bind(view.TextProperty, viewModel, x => x.ByteValue, converter: new ConvertersLibrary.Generic.ByteConverter());

            Assert.AreEqual("255", view.TextProperty.Value);
        }
    }
}