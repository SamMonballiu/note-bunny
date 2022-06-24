﻿using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using NoteBunny.BLL.Interfaces;
using NoteBunny.BLL.Models;
using NoteBunny.Frontend.Wpf.DotNetSix.ExtensionMethods;
using NoteBunny.Frontend.Wpf.DotNetSix.Helpers;
using NoteBunny.FrontEnd.Wpf.DotNetSix.Context;
using NoteBunny.FrontEnd.Wpf.DotNetSix.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace NoteBunny.FrontEnd.Wpf.DotNetSix.Viewmodels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly INoteRepository _noteRepository;
        private List<Note> _cachedNotes;
        private Sorter<Note, object> _noteSorter = NoteSorter.Default;

        [ObservableProperty, AlsoNotifyChangeFor(nameof(NoteModels))]
        private ObservableCollection<Note> _notes;

        public List<NoteViewModel> NoteModels
            => _noteSorter.Sort(_notes).Select(x => new NoteViewModel(x.Subject, x.Id)).ToList();

        [ObservableProperty]
        private string _searchTerm = string.Empty;

        public RelayCommand OnGetData { get; init; }
        public RelayCommand OnSearch { get; init; }
        public RelayCommand<string> OnSetSelectedNote { get; init; }
        public RelayCommand<NoteSortOptions> OnSetSortProperty { get; init; }
        public RelayCommand<SortDirection> OnSetSortDirection { get; init; }
        public RelayCommand OnDeleteSelectedNote { get; init; }

        public MainViewModel(INoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
            GetData();

            OnSearch = new RelayCommand(SearchNotes);
            OnSetSelectedNote = new RelayCommand<string>(SetSelectedNote);
            OnSetSortProperty = new RelayCommand<NoteSortOptions>(SetSortProperty);
            OnSetSortDirection = new RelayCommand<SortDirection>(SetSortDirection);
            OnDeleteSelectedNote = new RelayCommand(DeleteSelectedNote);
            OnGetData = new RelayCommand(GetData);
        }

        private void GetData()
        {
            _cachedNotes = _noteRepository.GetNotesWithTags().ToList();
            _notes = _cachedNotes.ToObservableCollection();
            OnPropertyChanged(nameof(NoteModels));
        }

        private void SearchNotes()
        {
            _notes = string.IsNullOrWhiteSpace(SearchTerm)
                ? _noteRepository.GetNotesWithTags().ToObservableCollection()
                : NoteFilter.BySearchTerm(_notes.ToList(), SearchTerm).ToObservableCollection();
            OnPropertyChanged(nameof(NoteModels));
        }

        private void SetSelectedNote(string? noteId)
        {
            SelectedNoteContext.SelectedNote = _notes.ToList().Find(x => x.Id == noteId);
        }

        private void SetSortProperty(NoteSortOptions sortProperty)
        {
            var sortPropertyMap = new Dictionary<NoteSortOptions, Func<Note, object>>()
            {
                { NoteSortOptions.CreatedOn, x => x.CreatedOn },
                { NoteSortOptions.Subject, x => x.Subject },
                { NoteSortOptions.NumberOfTags, x => x.Tags.Count },
                { NoteSortOptions.Id, x => x.Id }
            };

            _noteSorter.SetSortFunc(sortPropertyMap[sortProperty]);
        }

        private void SetSortDirection(SortDirection sortDirection)
        {
            _noteSorter.SortDirection = sortDirection;
            OnPropertyChanged(nameof(NoteModels));
        }

        private void DeleteSelectedNote()
        {
            var note = SelectedNoteContext.SelectedNote;
            if (note is null) return;
            if (MessageBox.Show("Confirm", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                _noteRepository.Delete(note);
                _noteRepository.Save();
                _notes.Remove(note);
                OnPropertyChanged(nameof(NoteModels));
            }
        }
    }
}
