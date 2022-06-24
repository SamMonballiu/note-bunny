using System;
using System.Collections.Generic;
using System.Linq;

// TODO Separate these into their own files
namespace NoteBunny.FrontEnd.Wpf.DotNetSix.Helpers
{
    public class Sorter<T, TKey>
    {
        public Func<T, TKey> SorterFunc { get; private set; }
        public SortDirection SortDirection { get; set; }

        public Sorter(Func<T, TKey> sorter)
        {
            SorterFunc = sorter;
        }

        public void SetSortFunc(Func<T, TKey> sorter)
        {
            SorterFunc = sorter;
        }

        public IEnumerable<T> Sort(IEnumerable<T> collection)
        {
            if (SorterFunc is null)
            {
                return collection;
            }

            switch (SortDirection)
            {
                case SortDirection.Ascending:
                    return collection.OrderBy(SorterFunc);
                case SortDirection.Descending:
                    return collection.OrderByDescending(SorterFunc);
                default:
                    return collection;
            }
        }
    }
}
