using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using NoteBunny.BLL.Enums;
using NoteBunny.BLL.Models;
using NoteBunny.FrontEnd.Wpf.DotNetSix.ExtensionMethods;
using NoteBunny.FrontEnd.Wpf.DotNetSix.Helpers;
using NoteBunny.FrontEnd.Wpf.DotNetSix.Viewmodels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NoteBunny.Frontend.Wpf.DotNetSix.Viewmodels
{
    public partial class SortableNoteViewModelBase : ObservableObject
    {
        public RelayCommand<NoteSortOptions> OnSetSortProperty { get; init; }
        public RelayCommand<SortDirection> OnSetSortDirection { get; init; }
        
        public static Sorter<Note, object> Sorter { get; private set; } = NoteSorter.Default;

        private static readonly Dictionary<NoteSortOptions, Func<Note, object>> _sortPropertyMap = new()
            {
                { NoteSortOptions.CreatedOn, x => x.CreatedOn },
                { NoteSortOptions.Subject, x => x.Subject },
                { NoteSortOptions.NumberOfTags, x => x.Tags.Count },
                { NoteSortOptions.Id, x => x.Id },
                { NoteSortOptions.Pinned, x => x.IsPinned ?? false }
            };

        [ObservableProperty, AlsoNotifyChangeFor(nameof(NoteModels))]
        private ObservableCollection<Note> _notes = new();

        public virtual ObservableCollection<NoteViewModel> NoteModels => _notes.Select(note => NoteViewModel.FromNote(note)).ToObservableCollection();

        private void SetSortProperty(NoteSortOptions sortProperty)
        {
            if (!_sortPropertyMap.ContainsKey(sortProperty))
            {
                throw new ArgumentException("No sort function found for property.");
            }

            Sorter.SetSortFunc(_sortPropertyMap[sortProperty]);
            Notes = Sorter.Sort(Notes).ToObservableCollection();
        }

        private void SetSortDirection(SortDirection sortDirection)
        {
            Sorter.SortDirection = sortDirection;
            Notes = Sorter.Sort(Notes).ToObservableCollection();
        }

        public SortableNoteViewModelBase()
        {
            OnSetSortProperty = new RelayCommand<NoteSortOptions>(SetSortProperty);
            OnSetSortDirection = new RelayCommand<SortDirection>(SetSortDirection);
        }
    }
}
