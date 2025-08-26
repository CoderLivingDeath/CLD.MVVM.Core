using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel;
using System.Reflection;
using System;
using System.Runtime.CompilerServices;

namespace CLD.MVVM.tests
{
    // Простые классы с INotifyPropertyChanged для тестов
    class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            RaisePropertyChanged(propertyName);
            return true;
        }

        protected void RaisePropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }

    class SourceClass : NotifyPropertyChangedBase
    {
        private string _name = "initial";

        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value);
            }
        }

        // Для проверки чтения и установки без уведомлений
        private string _noNotify = "initial";
        public string NoNotify
        {
            get => _noNotify;
            set
            {
                _noNotify = value;
            }
        }
        
        public int Number { get; set; }  // Новое свойство с типом int
    }

    class TargetClass : NotifyPropertyChangedBase
    {
        private string _text = "";

        public string Text
        {
            get => _text;
            set
            {
                SetProperty(ref _text, value);
            }
        }
    }

    [TestClass]
    public class PropertyBindingTests
    {
        // Для простого биндинга OneWay: изменение в источнике отражается в цели
        [TestMethod]
        public void OneWayBinding_UpdatesTargetOnSourceChange()
        {
            var source = new SourceClass { Name = "Initial" };
            var target = new TargetClass { Text = "Old" };

            var binding = new PropertyBinding(
                source,
                typeof(SourceClass).GetProperty(nameof(SourceClass.Name))!,
                target,
                typeof(TargetClass).GetProperty(nameof(TargetClass.Text))!,
                BindingWay.OneWay);

            // После биндинга target должен принять значение source
            Assert.AreEqual("Initial", target.Text);

            source.Name = "Updated";

            Assert.AreEqual("Updated", target.Text);

            binding.Dispose();
        }

        // Двусторонняя связь, изменения в обоих объектах синхронизируются
        [TestMethod]
        public void TwoWayBinding_UpdatesTargetAndSource()
        {
            var source = new SourceClass { Name = "Initial" };
            var target = new TargetClass { Text = "Old" };

            var binding = new PropertyBinding(
                source,
                typeof(SourceClass).GetProperty(nameof(SourceClass.Name))!,
                target,
                typeof(TargetClass).GetProperty(nameof(TargetClass.Text))!,
                BindingWay.TwoWay);

            // Инициализация
            Assert.AreEqual("Initial", target.Text);

            // Изменим источник, ожидаем обновления цели
            source.Name = "FromSource";
            Assert.AreEqual("FromSource", target.Text);

            // Изменим цель, ожидаем обновления источника
            target.Text = "FromTarget";
            Assert.AreEqual("FromTarget", source.Name);

            binding.Dispose();
        }

        // OneWayToSource — обновления идут от цели к источнику
        [TestMethod]
        public void OneWayToSourceBinding_UpdatesSourceOnTargetChange()
        {
            var source = new SourceClass { Name = "Initial" };
            var target = new TargetClass { Text = "Old" };

            var binding = new PropertyBinding(
                source,
                typeof(SourceClass).GetProperty(nameof(SourceClass.Name))!,
                target,
                typeof(TargetClass).GetProperty(nameof(TargetClass.Text))!,
                BindingWay.OneWayToSource);

            // Инициализация при таком режиме — исходное значение копируется в источник с цели
            Assert.AreEqual("Old", source.Name);

            // Изменим цель, обновим источник
            target.Text = "Changed";
            Assert.AreEqual("Changed", source.Name);

            // Если изменим источник — target не должен меняться (OneWayToSource)
            source.Name = "SourceChange";
            Assert.AreEqual("Changed", target.Text);

            binding.Dispose();
        }

        // Проверяем, что Dispose корректно отписывается и обновления уже не идут
        [TestMethod]
        public void Dispose_UnsubscribesFromEvents()
        {
            var source = new SourceClass { Name = "Initial" };
            var target = new TargetClass { Text = "Old" }; // начальное значение для target

            var binding = new PropertyBinding(
                source,
                typeof(SourceClass).GetProperty(nameof(SourceClass.Name))!,
                target,
                typeof(TargetClass).GetProperty(nameof(TargetClass.Text))!,
                BindingWay.TwoWay);

            // На момент создания биндинга target.Text синхронизируется с source.Name ("Initial")
            Assert.AreEqual("Initial", target.Text);

            binding.Dispose();

            // Меняем source.Name
            source.Name = "Changed";

            // target.Text не должен обновляться, оставаясь равным "Initial"
            Assert.AreEqual("Changed", source.Name);
            Assert.AreEqual("Initial", target.Text);
        }


        // Проверяем, что при несовместимых типах без конвертера выбрасывается исключение
        [TestMethod]
        public void Constructor_Throws_OnIncompatibleTypes()
        {
            var source = new SourceClass();
            var target = new TargetClass();

            var intProperty = typeof(SourceClass).GetProperty(nameof(SourceClass.Number))!;    // int property
            var textProperty = typeof(TargetClass).GetProperty(nameof(TargetClass.Text))!;     // string property

            Assert.ThrowsExactly<ArgumentException>(() =>
            {
                var binding = new PropertyBinding(
                    source,
                    intProperty,
                    target,
                    textProperty,
                    BindingWay.OneWay);
            });
        }

    }
}
