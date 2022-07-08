using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Win32;
using Newtonsoft.Json;
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
using System.Windows;

namespace NoteBunny.Frontend.Wpf.DotNetSix.Viewmodels
{
    public record NoteImportResult
    {
        public bool Success { get; init; }
        public IEnumerable<NoteViewModel> NotImported { get; init; } = new List<NoteViewModel>();
        public IEnumerable<NoteViewModel> Imported { get; init; } = new List<NoteViewModel>();
    }

    public partial class ImportNotesViewModel : ObservableObject
    {
        private readonly INoteRepository _noteRepository;
        private readonly ITagRepository _tagRepository;
        private List<NoteDto> _imported = new();

        [ObservableProperty]
        private ObservableCollection<NoteViewModel> _importCandidates = new();

        [ObservableProperty]
        private ObservableCollection<NoteViewModel> _loadedFromFile = new();

        public event Action<NoteImportResult>? OnNotesImported;
        
        public ImportNotesViewModel(INoteRepository noteRepository, ITagRepository tagRepository)
        {
            _noteRepository = noteRepository;
            _tagRepository = tagRepository;
        }

        [ICommand]
        private void LoadNotes(string filename)
        {
            var noteDtos = JsonConvert.DeserializeObject<ObservableCollection<NoteDto>>(File.ReadAllText(filename));
            _imported = noteDtos.ToList();
            LoadedFromFile = noteDtos.Select(NoteViewModel.FromNoteDto).ToObservableCollection();
        }

        [ICommand]
        private void Confirm()
        {
            try
            {
                var existingIds = _noteRepository.GetAll().Select(x => x.Id);
                var willImport = _importCandidates.Where(x => !existingIds.Contains(x.Id));
                var willNotImport = _importCandidates.Except(willImport);
                
                var tags = willImport.Select(x => x.Tags).Distinct();
                _tagRepository.AddTagsFromStrings(tags);

                var notes = GetNotesFromModels(willImport).ToList();
                notes.ForEach(_noteRepository.Add);
                _noteRepository.Save();

                OnNotesImported?.Invoke(new NoteImportResult { Success = true, Imported = willImport, NotImported = willNotImport });
            } catch
            {
                OnNotesImported?.Invoke(new NoteImportResult { Success = false });
            }
        }

        private IEnumerable<Note> GetNotesFromModels(IEnumerable<NoteViewModel> models)
        {
            foreach (var vm in models.ToList())
            {
                var dto = _imported.Single(d => d.Id == vm.Id);
                yield return new Note()
                {
                    Subject = vm.Subject,
                    Id = dto.Id,
                    Content = dto.Content,
                    CreatedOn = DateTime.Now,
                    TagIds = dto.Tags switch
                    {
                        null => new List<string>(),
                        not null => _tagRepository.GetTagIdsFromNames(dto.Tags)
                    },
                    IsPinned = null,
                };
            }
        }
    }
}
