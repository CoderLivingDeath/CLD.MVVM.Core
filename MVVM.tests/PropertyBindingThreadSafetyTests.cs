using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVVM.ConvertersLibrary.Generic;
using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MVVM.tests
{
    [TestClass]
    public class PropertyBindingThreadSafetyTests
    {
        private class TestSource : INotifyPropertyChanged
        {
            private int _value;

            public int Value
            {
                get => _value;
                set
                {
                    if (_value != value)
                    {
                        _value = value;
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
                    }
                }
            }

            public event PropertyChangedEventHandler? PropertyChanged;
        }

        private class TestProperty : IBindableProperty<string>
        {
            private string _value;
            public event EventHandler? ValueChanged;

            public string Value
            {
                get => _value;
                set
                {
                    if (_value != value)
                    {
                        _value = value;
                        ValueChanged?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }

        [TestMethod]
        public void PropertyBinding_ShouldBeThreadSafe()
        {
            var source = new TestSource();
            var property = new TestProperty();

            var binding = new PropertyBinding<string, TestSource, int>(
                property,
                source,
                s => s.Value,
                BindingMode.TwoWay, converter: new IntConverter());

            binding.Bind();

            const int threadCount = 10;
            const int iterations = 1000;

            var tasks = new Task[threadCount * 2];

            // Запускаем несколько задач, которые меняют значение в Source
            for (int i = 0; i < threadCount; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    for (int j = 0; j < iterations; j++)
                    {
                        source.Value = j;
                    }
                });
            }

            // Запускаем несколько задач, которые меняют значение в Property
            for (int i = 0; i < threadCount; i++)
            {
                tasks[threadCount + i] = Task.Run(() =>
                {
                    for (int j = 0; j < iterations; j++)
                    {
                        property.Value = j.ToString();
                    }
                });
            }

            Task.WaitAll(tasks);

            // Проверяем, что по окончании нет исключений и состояние консистентно
            // Например, значения Property и Source должны быть синхронизированы
            Assert.AreEqual(source.Value.ToString(), property.Value);

            binding.Dispose();
        }

        [TestMethod]
        public async Task PropertyBinding_ShouldRemainConsistent_UnderHighConcurrencyAndAsync()
        {
            var source = new TestSource();
            var property = new TestProperty();

            var binding = new PropertyBinding<string, TestSource, int>(
                property,
                source,
                s => s.Value,
                BindingMode.TwoWay, converter: new IntConverter());

            binding.Bind();

            const int taskCount = 20;
            const int iterationsPerTask = 500;

            var rand = new Random();

            var tasks = new Task[taskCount * 2];

            // Задачи, обновляющие Source с рандомными задержками
            for (int i = 0; i < taskCount; i++)
            {
                tasks[i] = Task.Run(async () =>
                {
                    for (int j = 0; j < iterationsPerTask; j++)
                    {
                        source.Value = j;
                        await Task.Delay(rand.Next(1, 5)); // случайная задержка 1-5 мс
                    }
                });
            }

            // Задачи, обновляющие Property с рандомными задержками
            for (int i = 0; i < taskCount; i++)
            {
                tasks[taskCount + i] = Task.Run(async () =>
                {
                    for (int j = 0; j < iterationsPerTask; j++)
                    {
                        property.Value = j.ToString();
                        await Task.Delay(rand.Next(1, 5)); // случайная задержка 1-5 мс
                    }
                });
            }

            // Запускать все задачи параллельно и ждать завершения
            await Task.WhenAll(tasks);

            // Проверяем консистентность: Property и Source остались синхронизированы
            Assert.AreEqual(source.Value.ToString(), property.Value, "Source and Property values are not synchronized after concurrent updates.");

            binding.Dispose();
        }

    }
}
