using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NoteBunny.Frontend.Wpf.DotNetSix.ExtensionMethods
{
    public static class ObservableCollection
    {
        public static void AddRange<T>(this ObservableCollection<T> collection, IList<T> items)
        {
            foreach (var item in items)
            {
                collection.Add(item);
            }
        }

        public static void RemoveRange<T>(this ObservableCollection<T> collection, IList<T> items)
        {
            foreach (var item in items)
            {
                collection.Remove(item);
            }
        }
    }
}
