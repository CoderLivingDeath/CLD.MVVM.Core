��� ������� ������ ���� ��������, �������� � �������� ������� � ������� �� ��������������� ����:

---

### �����: `PropertyBinder`
- **������������ ���:** `MVVM`  
- **����������:** ��������� ����������� �������� ����� ��������� � ���������� ��������� ������� �������� � ����������� ��������. ��������� ��������� `IPropertyBinder` � ��������� `IDisposable`.

---

### �����������:
```csharp
public PropertyBinder(IFactory<BindingParams, PropertyBinding>? factory = null)
```
- **��������:** ������ ����� ��������� `PropertyBinder`. ����������� ��������� ������� ��� �������� ��������, ���� �� ������� � ������������ ����������� �������.

---

### �����:
```csharp
public PropertyBinding Bind<TSource, TTarget>(
    TSource source,
    Expression<Func<TSource, object>> sourceProperty,
    TTarget target,
    Expression<Func<TTarget, object>> targetProperty,
    BindingWay mode = BindingWay.TwoWay,
    IValueConverter? converter = null)
```
- **��������:** ������ ����� �������� �������� `sourceProperty` ������� `source` � �������� `targetProperty` ������� `target`. ������������ ������ �������� (`mode`) � ����������� �������� ����� `converter`.

- **������������ ��������:** ��������� ������ �������� ���� `PropertyBinding`.

---

### �����:
```csharp
public void Unbind(PropertyBinding binding)
```
- **��������:** ������� � ����������� ��������� �������� `binding`.

---

### �����:
```csharp
public void Dispose()
```
- **��������:** ����������� ��� ��������, ����������� ������ �����������, ������ ���������� ���.

---

### ����������� �����:
```csharp
private static PropertyInfo GetPropertyInfo<T>(Expression<Func<T, object>> propertyLambda)
```
- **��������:** ��������� ���������� � �������� (`PropertyInfo`) �� ������-���������, ������������ �� �������� �������. ������������ ������ � ����� ����������� ���� (`Convert`).

---

### ��������� ����:
```csharp
private readonly HashSet<PropertyBinding> _bindings
```
- �������� �������� ��������.

---

### ��������� ����:
```csharp
private readonly IFactory<BindingParams, PropertyBinding> _factory
```
- ������� ��� �������� ��������.

---

���� �����, ���� ��������� ������ `PropertyBinding`, `BindingParams`, `BindingWay`, `IValueConverter` � ��������� `IFactory`, ���� �� ����������� � ������ ���� �����������.