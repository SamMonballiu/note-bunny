using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NoteBunny.BLL.Enums;
using NoteBunny.BLL.Helpers;
using NoteBunny.BLL.Interfaces;
using NoteBunny.BLL.Models;
using NoteBunny.Frontend.Wpf.DotNetSix.Models;
using NoteBunny.FrontEnd.Wpf.DotNetSix.ExtensionMethods;
using NoteBunny.FrontEnd.Wpf.DotNetSix.Viewmodels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using MatchType = NoteBunny.BLL.Enums.MatchType;

namespace NoteBunny.Frontend.Wpf.DotNetSix.Viewmodels
{
    internal partial class ExportNotesViewModel : SortableNoteViewModelBase
    {
        private readonly INoteRepository _noteRepository;
        private readonly List<Note> _cachedNotes;

        [ObservableProperty]
        private ObservableCollection<NoteViewModel> _exportCandidates = new();

        [ObservableProperty]
        private string _searchTerm = string.Empty;

        [ObservableProperty]
        private MatchType _match = MatchType.Any;

        private NoteFilterType _filterOn = NoteFilterType.Subject;
        public NoteFilterType FilterOn
        {
            get => _filterOn;
            set
            {
                SetProperty(ref _filterOn, value);
                if (value == NoteFilterType.All)
                {
                    SetProperty(ref _match, MatchType.Any);
                }
                OnPropertyChanged(nameof(IsNotFilterAll));
            }
        }

        public bool IsNotFilterAll => !FilterOn.Equals(NoteFilterType.All);

        public RelayCommand OnSearch { get; private set; }
        public RelayCommand<string> OnConfirm { get; private set; }

        public ExportNotesViewModel(INoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
            OnSearch = new RelayCommand(FilterNotes);
            OnConfirm = new RelayCommand<string>(ExportNotes);
            _cachedNotes = _noteRepository.GetNotesWithTags().ToList();
            Notes = Sorter.Sort(_cachedNotes).ToObservableCollection();
        }

        private void FilterNotes()
        {
            Notes = string.IsNullOrWhiteSpace(SearchTerm)
            ? Sorter.Sort(_cachedNotes).ToObservableCollection()
            : Sorter.Sort(NoteFilter.BySearchTerm(_cachedNotes, SearchTerm, _filterOn, _match)).ToObservableCollection();
        }

        private void ExportNotes(string filename)
        {
            if (!ExportCandidates.Any())
            {
                return;
            }

            var candidateIds = _exportCandidates.Select(x => x.Id);

            if (!candidateIds.Any())
            {
                return;
            }

            var exportNotes = _cachedNotes
                .Where(n => candidateIds.Contains(n.Id))
                .Select(NoteDto.FromNote);

            var json = JsonConvert.SerializeObject(
                exportNotes,
                Formatting.Indented,
                new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

            File.WriteAllText(filename, json);
            _exportCandidates.Clear();
        }
    }
}
