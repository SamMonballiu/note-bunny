using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NoteBunny.BLL.Enums;
using NoteBunny.BLL.Helpers;
using NoteBunny.BLL.Interfaces;
using NoteBunny.BLL.Models;
using NoteBunny.Frontend.Wpf.DotNetSix.ExtensionMethods;
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
    public record NoteDto
    {
        public string Subject { get; init; } = string.Empty;
        public string? Tags { get; init; }
        public string Content { get; init; } = string.Empty;

        public static NoteDto FromNote(Note note) => new()
        {
            Subject = note.Subject,
            Content = note.Content,
            Tags = note.Tags switch
            {
                null => null,
                not null => string.Join(",", note.Tags),
            },
        };
    }

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
        public RelayCommand OnConfirm { get; private set; }

        public ExportNotesViewModel(INoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
            OnSearch = new RelayCommand(FilterNotes);
            OnConfirm = new RelayCommand(ExportNotes);
            _cachedNotes = _noteRepository.GetNotesWithTags().ToList();
            Notes = Sorter.Sort(_cachedNotes).ToObservableCollection();
        }

        private void FilterNotes()
        {
            Notes = string.IsNullOrWhiteSpace(SearchTerm)
            ? Sorter.Sort(_cachedNotes).ToObservableCollection()    
            : Sorter.Sort(NoteFilter.BySearchTerm(_cachedNotes, SearchTerm, _filterOn, _match)).ToObservableCollection();
        }

        private void ExportNotes()
        {
            var candidateIds = _exportCandidates.Select(x => x.Id);

            if (!candidateIds.Any())
            {
                return;
            }

            var date = $"{DateTime.Now.ToShortDateString()}_{DateTime.Now.ToShortTimeString()}".ReplaceAlphanumeric('_');

            SaveFileDialog saveFileDialog = new()
            {
                Filter = "JSON file (*.json)|*.json",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                FileName = $"export_{date}.json"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                var exportNotes = _cachedNotes
                    .Where(n => candidateIds.Contains(n.Id))
                    .Select(NoteDto.FromNote);

                var json = JsonConvert.SerializeObject(
                    exportNotes, 
                    Formatting.Indented, 
                    new JsonSerializerSettings() { 
                        ContractResolver = new CamelCasePropertyNamesContractResolver() 
                    });

                File.WriteAllText(saveFileDialog.FileName, json);
            }
        }
    }
}
