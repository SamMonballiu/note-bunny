using NoteBunny.BLL.Models;

namespace NoteBunny.FrontEnd.Wpf.DotNetSix.Helpers
{
    public static class NoteSorter
    {
        public static Sorter<Note, object> Default => new Sorter<Note, object>(x => x.CreatedOn) { SortDirection = SortDirection.Descending };
    }
}
