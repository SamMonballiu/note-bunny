using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NoteBunny.FrontEnd.Wpf.DotNetSix.ExtensionMethods
{
    public static class Extensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this List<T> list)
        {
            return new ObservableCollection<T>(list);
        }

        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> collection)
        {
            return new ObservableCollection<T>(collection);
        }
    }
}
