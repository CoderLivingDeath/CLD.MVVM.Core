[Flags]
public enum BindingWay
{
    None = 0,
    OneWay = 1,        // Привязка из ViewModel в View
    TwoWay = 2,        // Двунаправленная привязка
    OneWayToSource = 4 // Из View в ViewModel (редко используется)
}
