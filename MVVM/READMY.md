Вот краткий список всех названий, сигнатур и описаний методов и классов из представленного кода:

---

### Класс: `PropertyBinder`
- **Пространство имён:** `MVVM`  
- **Назначение:** Позволяет привязывать свойства между объектами с поддержкой различных режимов привязки и конвертации значений. Реализует интерфейс `IPropertyBinder` и интерфейс `IDisposable`.

---

### Конструктор:
```csharp
public PropertyBinder(IFactory<BindingParams, PropertyBinding>? factory = null)
```
- **Описание:** Создаёт новый экземпляр `PropertyBinder`. Опционально принимает фабрику для создания привязок, если не указана — используется стандартная фабрика.

---

### Метод:
```csharp
public PropertyBinding Bind<TSource, TTarget>(
    TSource source,
    Expression<Func<TSource, object>> sourceProperty,
    TTarget target,
    Expression<Func<TTarget, object>> targetProperty,
    BindingWay mode = BindingWay.TwoWay,
    IValueConverter? converter = null)
```
- **Описание:** Создаёт новую привязку свойства `sourceProperty` объекта `source` к свойству `targetProperty` объекта `target`. Поддерживает режимы привязки (`mode`) и конвертацию значений через `converter`.

- **Возвращаемое значение:** Созданный объект привязки типа `PropertyBinding`.

---

### Метод:
```csharp
public void Unbind(PropertyBinding binding)
```
- **Описание:** Удаляет и освобождает указанную привязку `binding`.

---

### Метод:
```csharp
public void Dispose()
```
- **Описание:** Освобождает все привязки, управляемые данным экземпляром, очищая внутренний кеш.

---

### Статический метод:
```csharp
private static PropertyInfo GetPropertyInfo<T>(Expression<Func<T, object>> propertyLambda)
```
- **Описание:** Извлекает информацию о свойстве (`PropertyInfo`) из лямбда-выражения, указывающего на свойство объекта. Обрабатывает случаи с явным приведением типа (`Convert`).

---

### Приватное поле:
```csharp
private readonly HashSet<PropertyBinding> _bindings
```
- Содержит активные привязки.

---

### Приватное поле:
```csharp
private readonly IFactory<BindingParams, PropertyBinding> _factory
```
- Фабрика для создания привязок.

---

Если нужно, могу расписать классы `PropertyBinding`, `BindingParams`, `BindingWay`, `IValueConverter` и интерфейс `IFactory`, хотя их определения в данном коде отсутствуют.