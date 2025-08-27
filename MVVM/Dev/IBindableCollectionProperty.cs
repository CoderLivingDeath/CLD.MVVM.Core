using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace MVVM.Dev
{
    public interface IBindableCollectionProperty<T> : IBindableProperty<ObservableCollection<T>>
    {
        event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}