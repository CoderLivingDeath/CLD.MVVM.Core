using CLD.MVVM.Core.Tests.Components;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace CLD.MVVM.Core.Tests
{
    [TestClass]
    public class BindablePropertiesTests
    {
        // Реализация ViewModel с использованием компонентов
        public class MyViewModel : ViewModelBase
        {
            private string _someText;

            public MyViewModel()
            {
                _someText = string.Empty;
            }

            public string SomeText
            {
                get => _someText;
                set => Set(ref _someText, value);
            }
        }

        // Реализация View с инъекцией зависимостей
        public class View
        {
            public View(TextProperty textProperty, CollectionProperty<int> numsList)
            {
                _textProperty = textProperty;
                _numsList = numsList;
            }

            private TextProperty _textProperty { get; }
            private CollectionProperty<int> _numsList { get; }

            public TextProperty TextProperty => _textProperty;
            public CollectionProperty<int> NumsList => _numsList;
        }

        // Тесты для TextProperty
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
            string currentValue = null;


            textProperty.ValueChanged += (s, e) =>
            {
                eventRaised = true;
                currentValue = e.NewValue;
            };

            // Act
            textProperty.Value = "NewValue";

            // Assert
            Assert.IsTrue(eventRaised, "Событие ValueChanged должно быть вызвано при изменении значения");
            Assert.AreEqual("NewValue", currentValue, "Новое значение в событии должно совпадать с установленным");
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
            textProperty.Value = "SameValue";

            // Assert
            Assert.IsFalse(eventRaised, "Событие ValueChanged не должно вызываться при установке того же значения");
        }

        [TestMethod]
        public void TextProperty_SetValueMethod_ShouldUpdateValueAndRaiseEvent()
        {
            // Arrange
            var textProperty = new TextProperty();
            bool eventRaised = false;
            string currentValue = null;

            textProperty.ValueChanged += (s, e) =>
            {
                eventRaised = true;
                currentValue = e.NewValue;
            };

            // Act
            bool changed = textProperty.SetValue("TestValue");

            // Assert
            Assert.IsTrue(changed, "Метод SetValue должен вернуть true при изменении значения");
            Assert.IsTrue(eventRaised, "Событие ValueChanged должно быть вызвано при вызове SetValue");
            Assert.AreEqual("TestValue", currentValue, "Новое значение в событии должно совпадать с установленным");
            Assert.AreEqual("TestValue", textProperty.Value, "Значение свойства Value должно совпадать с установленным");
        }

        [TestMethod]
        public void TextProperty_SetValueMethod_SameValue_ShouldNotRaiseEvent()
        {
            // Arrange
            var textProperty = new TextProperty();
            textProperty.SetValue("InitialValue");
            bool eventRaised = false;

            textProperty.ValueChanged += (s, e) =>
            {
                eventRaised = true;
            };

            // Act
            bool changed = textProperty.SetValue("InitialValue");

            // Assert
            Assert.IsFalse(changed, "Метод SetValue должен вернуть false при установке того же значения");
            Assert.IsFalse(eventRaised, "Событие ValueChanged не должно быть вызвано при установке того же значения");
        }

        [TestMethod]
        public void TextProperty_SetValueWithoutEmit_ShouldNotRaiseEvent()
        {
            // Arrange
            var textProperty = new TextProperty();
            bool eventRaised = false;

            textProperty.ValueChanged += (s, e) =>
            {
                eventRaised = true;
            };

            // Act
            textProperty.SetValue("NewValue", emitEvent: false);

            // Assert
            Assert.IsFalse(eventRaised, "Событие ValueChanged не должно быть вызвано при emitEvent = false");
            Assert.AreEqual("NewValue", textProperty.Value, "Значение должно обновиться без вызова события");
        }

        // Тесты для CollectionProperty
        [TestMethod]
        public void CollectionProperty_InitialValue_ShouldBeEmptyObservableCollection()
        {
            // Arrange
            var collectionProperty = new CollectionProperty<int>();

            // Assert
            Assert.IsNotNull(collectionProperty.Value, "Значение должно быть инициализировано");
            Assert.AreEqual(0, collectionProperty.Value.Count, "Начальная коллекция должна быть пустой");
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
                Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action, "Действие события должно быть Add");
                Assert.AreEqual(1, e.NewItems.Count, "Должен быть добавлен один элемент");
                Assert.AreEqual(42, e.NewItems[0], "Добавленный элемент должен быть 42");
            };

            // Act
            collectionProperty.Value.Add(42);

            // Assert
            Assert.IsTrue(collectionChangedRaised, "Событие CollectionChanged должно быть вызвано при добавлении элемента");
        }

        [TestMethod]
        public void CollectionProperty_RemoveItem_ShouldRaiseCollectionChangedEvent()
        {
            // Arrange
            var collectionProperty = new CollectionProperty<int>();
            collectionProperty.Value.Add(42);
            bool collectionChangedRaised = false;

            collectionProperty.CollectionChanged += (sender, e) =>
            {
                collectionChangedRaised = true;
                Assert.AreEqual(NotifyCollectionChangedAction.Remove, e.Action, "Действие события должно быть Remove");
                Assert.AreEqual(1, e.OldItems.Count, "Должен быть удален один элемент");
                Assert.AreEqual(42, e.OldItems[0], "Удаленный элемент должен быть 42");
            };

            // Act
            collectionProperty.Value.Remove(42);

            // Assert
            Assert.IsTrue(collectionChangedRaised, "Событие CollectionChanged должно быть вызвано при удалении элемента");
        }

        [TestMethod]
        public void CollectionProperty_Clear_ShouldRaiseCollectionChangedEvent()
        {
            // Arrange
            var collectionProperty = new CollectionProperty<int>();
            collectionProperty.Value.Add(1);
            collectionProperty.Value.Add(2);
            bool collectionChangedRaised = false;

            collectionProperty.CollectionChanged += (sender, e) =>
            {
                collectionChangedRaised = true;
                Assert.AreEqual(NotifyCollectionChangedAction.Reset, e.Action, "Действие события должно быть Reset при очистке");
            };

            // Act
            collectionProperty.Value.Clear();

            // Assert
            Assert.IsTrue(collectionChangedRaised, "Событие CollectionChanged должно быть вызвано при очистке коллекции");
        }

        [TestMethod]
        public void CollectionProperty_SetNewCollection_ShouldRaiseValueChangedAndSubscribeCollectionChanged()
        {
            // Arrange
            var collectionProperty = new CollectionProperty<int>();
            bool valueChangedRaised = false;
            bool collectionChangedRaised = false;

            collectionProperty.ValueChanged += (s, e) =>
            {
                valueChangedRaised = true;
                Assert.IsNotNull(e.NewValue, "Новое значение в событии не должно быть null");
            };
            collectionProperty.CollectionChanged += (s, e) => collectionChangedRaised = true;

            // Act
            var newCollection = new ObservableCollection<int>();
            collectionProperty.Value = newCollection;
            newCollection.Add(5);

            // Assert
            Assert.IsTrue(valueChangedRaised, "Событие ValueChanged должно быть вызвано при установке новой коллекции");
            Assert.IsTrue(collectionChangedRaised, "Событие CollectionChanged должно быть вызвано при добавлении в новую коллекцию");
            Assert.AreSame(newCollection, collectionProperty.Value, "Значение должно быть обновлено на новую коллекцию");
        }

        [TestMethod]
        public void CollectionProperty_SetNullCollection_ShouldSetEmptyCollectionAndRaiseValueChanged()
        {
            // Arrange
            var collectionProperty = new CollectionProperty<int>();
            bool valueChangedRaised = false;

            collectionProperty.ValueChanged += (s, e) => valueChangedRaised = true;

            // Act
            collectionProperty.Value = null;

            // Assert
            Assert.IsTrue(valueChangedRaised, "Событие ValueChanged должно быть вызвано при установке null");
            Assert.IsNotNull(collectionProperty.Value, "Значение не должно быть null после установки");
            Assert.AreEqual(0, collectionProperty.Value.Count, "Должна быть создана новая пустая коллекция");
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
            collectionProperty.Value = currentCollection;

            // Assert
            Assert.IsFalse(eventRaised, "Событие ValueChanged не должно быть вызвано при установке той же коллекции");
        }

        [TestMethod]
        public void CollectionProperty_SetValueMethod_WithoutEmit_ShouldNotRaiseValueChanged()
        {
            // Arrange
            var collectionProperty = new CollectionProperty<int>();
            bool eventRaised = false;

            collectionProperty.ValueChanged += (s, e) => eventRaised = true;

            // Act
            var newCollection = new ObservableCollection<int>();
            bool changed = collectionProperty.SetValue(newCollection, emitEvent: false);

            // Assert
            Assert.IsTrue(changed, "Метод SetValue должен вернуть true при изменении коллекции");
            Assert.IsFalse(eventRaised, "Событие ValueChanged не должно быть вызвано при emitEvent = false");
            Assert.AreSame(newCollection, collectionProperty.Value, "Значение должно быть обновлено");
        }

        // Тесты для ViewModel
        [TestMethod]
        public void ViewModel_SetSomeText_ShouldRaisePropertyChanged()
        {
            // Arrange
            var viewModel = new MyViewModel();
            bool propertyChangedRaised = false;
            string propertyName = null;

            viewModel.PropertyChanged += (s, e) =>
            {
                propertyChangedRaised = true;
                propertyName = e.PropertyName;
            };

            // Act
            viewModel.SomeText = "NewText";

            // Assertions
            Assert.IsTrue(propertyChangedRaised, "Событие PropertyChanged должно быть вызвано при изменении SomeText");
            Assert.AreEqual("SomeText", propertyName, "Имя свойства в событии должно быть 'SomeText'");
            Assert.AreEqual("NewText", viewModel.SomeText, "Значение SomeText должно быть обновлено");
        }

        [TestMethod]
        public void ViewModel_SetSameSomeText_ShouldNotRaisePropertyChanged()
        {
            // Arrange
            var viewModel = new MyViewModel();
            viewModel.SomeText = "SameText";
            bool propertyChangedRaised = false;

            viewModel.PropertyChanged += (s, e) => propertyChangedRaised = true;

            // Act
            viewModel.SomeText = "SameText";

            // Assert
            Assert.IsFalse(propertyChangedRaised, "Событие PropertyChanged не должно быть вызвано при установке того же значения");
        }

        // Тесты для View
        [TestMethod]
        public void View_Initialization_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var textProperty = new TextProperty();
            var numsList = new CollectionProperty<int>();

            // Act
            var view = new View(textProperty, numsList);

            // Assert
            Assert.AreSame(textProperty, view.TextProperty, "TextProperty должно быть установлено из конструктора");
            Assert.AreSame(numsList, view.NumsList, "NumsList должно быть установлено из конструктора");
        }

        [TestMethod]
        public void View_TextProperty_SetValue_ShouldRaiseValueChangedEvent()
        {
            // Arrange
            var textProperty = new TextProperty();
            var numsList = new CollectionProperty<int>();
            var view = new View(textProperty, numsList);
            bool eventRaised = false;
            string currentValue = null;

            view.TextProperty.ValueChanged += (s, e) =>
            {
                eventRaised = true;
                currentValue = e.NewValue;
            };

            // Act
            view.TextProperty.Value = "NewValue";

            // Assert
            Assert.IsTrue(eventRaised, "Событие ValueChanged должно быть вызвано при изменении TextProperty в View");
            Assert.AreEqual("NewValue", currentValue, "Новое значение в событии должно совпадать с установленным");
            Assert.AreEqual("NewValue", view.TextProperty.Value, "Значение TextProperty должно совпадать с установленным");
        }

        [TestMethod]
        public void View_CollectionProperty_AddItem_ShouldRaiseCollectionChangedEvent()
        {
            // Arrange
            var textProperty = new TextProperty();
            var numsList = new CollectionProperty<int>();
            var view = new View(textProperty, numsList);
            bool collectionChangedRaised = false;

            view.NumsList.CollectionChanged += (sender, e) =>
            {
                collectionChangedRaised = true;
                Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action, "Действие события должно быть Add");
                Assert.AreEqual(1, e.NewItems.Count, "Должен быть добавлен один элемент");
                Assert.AreEqual(42, e.NewItems[0], "Добавленный элемент должен быть 42");
            };

            // Act
            view.NumsList.Value.Add(42);

            // Assert
            Assert.IsTrue(collectionChangedRaised, "Событие CollectionChanged должно быть вызвано при добавлении элемента в NumsList");
        }

        [TestMethod]
        public void View_CollectionProperty_SetNewCollection_ShouldRaiseValueChanged()
        {
            // Arrange
            var textProperty = new TextProperty();
            var numsList = new CollectionProperty<int>();
            var view = new View(textProperty, numsList);
            bool valueChangedRaised = false;

            view.NumsList.ValueChanged += (s, e) => valueChangedRaised = true;

            // Act
            var newCollection = new ObservableCollection<int> { 1, 2, 3 };
            view.NumsList.Value = newCollection;

            // Assert
            Assert.IsTrue(valueChangedRaised, "Событие ValueChanged должно сработать при замене коллекции в View");
            CollectionAssert.AreEqual(new int[] { 1, 2, 3 }, view.NumsList.Value, "Коллекция должна содержать элементы новой коллекции");
        }

        [TestMethod]
        public void View_CollectionProperty_SetSameCollection_DoesNotRaiseValueChanged()
        {
            // Arrange
            var textProperty = new TextProperty();
            var numsList = new CollectionProperty<int>();
            var view = new View(textProperty, numsList);
            bool valueChangedRaised = false;

            var currentCollection = view.NumsList.Value;
            view.NumsList.ValueChanged += (s, e) => valueChangedRaised = true;

            // Act
            view.NumsList.Value = currentCollection;

            // Assert
            Assert.IsFalse(valueChangedRaised, "Событие ValueChanged не должно сработать при присвоении той же коллекции в View");
        }

        [TestMethod]
        public void View_CollectionProperty_SetValueWithoutEmit_ShouldNotRaiseValueChanged()
        {
            // Arrange
            var textProperty = new TextProperty();
            var numsList = new CollectionProperty<int>();
            var view = new View(textProperty, numsList);
            bool eventRaised = false;

            view.NumsList.ValueChanged += (s, e) => eventRaised = true;

            // Act
            var newCollection = new ObservableCollection<int>();
            bool changed = view.NumsList.SetValue(newCollection, emitEvent: false);

            // Assert
            Assert.IsTrue(changed, "Метод SetValue должен вернуть true при изменении коллекции");
            Assert.IsFalse(eventRaised, "Событие ValueChanged не должно быть вызвано при emitEvent = false");
            Assert.AreSame(newCollection, view.NumsList.Value, "Значение должно быть обновлено");
        }
    }
}