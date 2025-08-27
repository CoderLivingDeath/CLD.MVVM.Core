using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace MVVM.tests
{
    [TestClass]
    public class BindablePropertiesTests
    {
        public BindablePropertiesTests() { }

        [TestMethod]
        public void TextProperty_InitialValue_ShouldBeEmptyString()
        {
            // Arrange
            var textProperty = new TextProperty();

            // Assert
            Assert.AreEqual(string.Empty, textProperty.Value, "Начальное значение должно быть пустой строкой");
        }

        [TestMethod]
        public void TextProperty_SetValue_ShouldRaiseValueChangedEvent()
        {
            // Arrange
            var textProperty = new TextProperty();
            bool eventRaised = false;
            string? currentValue = null;

            textProperty.ValueChanged += (s, e) =>
            {
                eventRaised = true;
                currentValue = textProperty.Value;
            };

            // Act
            textProperty.Value = "NewValue";

            // Assert
            Assert.IsTrue(eventRaised, "Событие ValueChanged должно быть вызвано при изменении значения");
            Assert.AreEqual("NewValue", currentValue);
            Assert.AreEqual("NewValue", textProperty.Value, "Значение свойства Value должно совпадать с установленным");
        }

        [TestMethod]
        public void TextProperty_SetSameValue_ShouldNotRaiseValueChangedEvent()
        {
            // Arrange
            var textProperty = new TextProperty();
            textProperty.Value = "SameValue";

            bool eventRaised = false;
            textProperty.ValueChanged += (s, e) =>
            {
                eventRaised = true;
            };

            // Act
            textProperty.Value = "SameValue"; // то же значение

            // Assert
            Assert.IsFalse(eventRaised, "Событие ValueChanged не должно вызываться при установке того же значения");
        }

        [TestMethod]
        public void TextProperty_InvokeChange_ShouldRaiseValueChangedEvent()
        {
            // Arrange
            var textProperty = new TextProperty();

            bool eventRaised = false;
            textProperty.ValueChanged += (s, e) =>
            {
                eventRaised = true;
            };

            // Act
            textProperty.InvokeChange();

            // Assert
            Assert.IsTrue(eventRaised, "Событие ValueChanged должно быть вызвано при вызове InvokeChange");
        }

        [TestMethod]
        public void TextProperty_SetValueMethod_ShouldUpdateValueAndRaiseEvent()
        {
            // Arrange
            var textProperty = new TextProperty();

            bool eventRaised = false;
            string? currentValue = null;

            textProperty.ValueChanged += (s, e) =>
            {
                eventRaised = true;
                currentValue = textProperty.Value;
            };

            // Act
            textProperty.SetValue("TestValue");

            // Assert
            Assert.IsTrue(eventRaised, "Событие ValueChanged должно быть вызвано при вызове SetValue");
            Assert.AreEqual("TestValue", currentValue);
            Assert.AreEqual("TestValue", textProperty.Value);
        }

        [TestMethod]
        public void CollectionProperty_AddItem_ShouldRaiseCollectionChangedEvent()
        {
            // Arrange
            var collectionProperty = new CollectionProperty<int>();
            bool collectionChangedRaised = false;

            collectionProperty.CollectionChanged += (sender, e) =>
            {
                collectionChangedRaised = true;
                Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                Assert.AreEqual(1, e.NewItems.Count);
                Assert.AreEqual(42, e.NewItems[0]);
            };

            // Act
            collectionProperty.Value.Add(42);

            // Assert
            Assert.IsTrue(collectionChangedRaised, "CollectionChanged event was not raised on Add");
        }

        [TestMethod]
        public void CollectionProperty_SetNewCollection_ShouldRaiseValueChangedAndSubscribeCollectionChanged()
        {
            // Arrange
            var collectionProperty = new CollectionProperty<int>();

            bool valueChangedRaised = false;
            bool collectionChangedRaised = false;

            collectionProperty.Value = new ObservableCollection<int>();

            collectionProperty.Value.Add(1); // To test event is working on initial collection

            collectionProperty.ValueChanged += (s, e) => valueChangedRaised = true;
            collectionProperty.CollectionChanged += (s, e) => collectionChangedRaised = true;

            // Act
            var newCollection = new System.Collections.ObjectModel.ObservableCollection<int>();
            collectionProperty.Value = newCollection; // This should trigger ValueChanged

            newCollection.Add(5); // This should trigger CollectionChanged

            // Assert
            Assert.IsTrue(valueChangedRaised, "ValueChanged event was not raised when setting new collection");
            Assert.IsTrue(collectionChangedRaised, "CollectionChanged event was not raised when adding to new collection");
        }

        [TestMethod]
        public void CollectionProperty_SetSameCollection_ShouldNotRaiseValueChangedEvent()
        {
            // Arrange
            var collectionProperty = new CollectionProperty<int>();
            bool eventRaised = false;

            collectionProperty.ValueChanged += (s, e) => eventRaised = true;

            var currentCollection = collectionProperty.Value;

            // Act
            collectionProperty.Value = currentCollection; // Setter with same reference

            // Assert
            Assert.IsFalse(eventRaised, "ValueChanged event should not be raised if the same collection is assigned");
        }
    }


    [TestClass]
    public class ViewSampleTests
    {
        // Тесты для Field и CollectionProperty внутри View

        [TestMethod]
        public void Field_TextProperty_ShouldRaiseTextChangedWhenTextChanges()
        {
            var view = new View();
            bool eventRaised = false;

            view.field.TextChanged += (s, e) => eventRaised = true;

            view.field.Text = "new text";

            Assert.IsTrue(eventRaised, "TextChanged event должен сработать при изменении Text.");
            Assert.AreEqual("new text", view.field.Text);
        }

        [TestMethod]
        public void Field_TextProperty_ShouldNotRaiseTextChangedWhenTextSetSameValue()
        {
            var view = new View();

            bool eventRaised = false;
            view.field.TextChanged += (s, e) => eventRaised = true;

            view.field.Text = "same text";

            eventRaised = false;

            view.field.Text = "same text";

            Assert.IsFalse(eventRaised, "TextChanged событие не должно вызваться, если текст не изменился.");
        }

        [TestMethod]
        public void CollectionProperty_ShouldRaiseCollectionChanged_AndValueChanged()
        {
            var view = new View();

            bool collectionChangedRaised = false;
            bool valueChangedRaised = false;

            view.NumsList.CollectionChanged += (s, e) =>
            {
                collectionChangedRaised = true;
                Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                Assert.AreEqual(1, e.NewItems.Count);
                Assert.AreEqual(42, e.NewItems[0]);
            };

            view.NumsList.ValueChanged += (s, e) => valueChangedRaised = true;

            view.NumsList.Value.Add(42);

            Assert.IsTrue(collectionChangedRaised, "CollectionChanged событие должно вызваться при добавлении элемента.");
            Assert.IsFalse(valueChangedRaised, "ValueChanged событие не вызывается при изменении содержимого коллекции.");
        }

        [TestMethod]
        public void CollectionProperty_SetNewCollection_ShouldRaiseValueChanged()
        {
            var view = new View();

            bool valueChangedRaised = false;
            view.NumsList.ValueChanged += (s, e) => valueChangedRaised = true;

            var newCollection = new System.Collections.ObjectModel.ObservableCollection<int>() { 1, 2, 3 };
            view.NumsList.Value = newCollection;

            Assert.IsTrue(valueChangedRaised, "ValueChanged должна сработать при замене коллекции.");
            CollectionAssert.AreEqual(new int[] { 1, 2, 3 }, view.NumsList.Value);
        }

        [TestMethod]
        public void CollectionProperty_SetSameCollection_DoesNotRaiseValueChanged()
        {
            var view = new View();
            bool valueChangedRaised = false;

            var currentCollection = view.NumsList.Value;
            view.NumsList.ValueChanged += (s, e) => valueChangedRaised = true;

            view.NumsList.Value = currentCollection;

            Assert.IsFalse(valueChangedRaised, "ValueChanged не должна сработать при присвоении той же коллекции.");
        }

        // Тесты для SubmitField

        [TestMethod]
        public void Submit_ShouldRaiseTextChangedEvent()
        {
            var submitField = new SubmitField();
            bool eventRaised = false;

            submitField.TextChanged += (s, e) => eventRaised = true;

            submitField.Submit();

            Assert.IsTrue(eventRaised, "Событие TextChanged должен быть вызвано при вызове Submit()");
        }

        [TestMethod]
        public void SettingText_ShouldChangeTextPropertyValue()
        {
            var submitField = new SubmitField();

            submitField.Text = "Hello";

            Assert.AreEqual("Hello", submitField.TextProperty.Value);
            Assert.AreEqual("Hello", submitField.Text);
        }

        [TestMethod]
        public void ChangingText_DoesNotRaiseTextChangedEvent()
        {
            var submitField = new SubmitField();
            bool eventRaised = false;

            submitField.TextChanged += (s, e) => eventRaised = true;

            submitField.Text = "Hi";

            Assert.IsFalse(eventRaised, "Событие TextChanged не должно вызываться при изменении текста напрямую");
        }

        // Тесты интеграции SubmitField внутри View

        [TestMethod]
        public void View_SubmitField_ShouldBeInitialized()
        {
            var view = new View();

            Assert.IsNotNull(view.SubmitField);
            Assert.IsNotNull(view.SubmitField.TextProperty);
        }

        [TestMethod]
        public void View_SubmitField_SubmitEventTriggered()
        {
            var view = new View();
            bool eventRaised = false;

            view.SubmitField.TextChanged += (s, e) => eventRaised = true;

            Assert.IsFalse(eventRaised);

            view.SubmitField.Submit();

            Assert.IsTrue(eventRaised, "Событие TextChanged SubmitField должно сработать из View");
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