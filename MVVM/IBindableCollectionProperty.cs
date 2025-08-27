using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace MVVM
{
    public interface IBindableCollectionProperty<T> : IBindableProperty<ObservableCollection<T>>
    {
        event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}