using System;
using System.Collections.Generic;
using System.Linq;

// TODO Separate these into their own files
namespace NoteBunny.FrontEnd.Wpf.Helpers
{
    public abstract class Sorter<T, TKey>
    {
        public Func<T, TKey> SorterFunc { get; private set; }
        public SortDirection SortDirection { get; set; }

        public void SetSorter(Func<T, TKey> sorter)
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

    public class NoteSorter<Note, TKey> : Sorter<Note, TKey>
    {
        public Func<Note, TKey> Sorter { get; private set; }
    }

    public enum SortDirection
    {
        Ascending,
        Descending
    }
}
