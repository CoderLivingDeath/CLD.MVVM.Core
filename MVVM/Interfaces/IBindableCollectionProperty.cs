using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace CLD.MVVM.Interfaces
{
    public interface IBindableCollectionProperty<T> : IBindableProperty<ObservableCollection<T>>
    {
        event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}