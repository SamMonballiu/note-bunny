using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using NoteBunny.BLL.Interfaces;
using NoteBunny.BLL.Models;
using System;
using System.Linq;
using System.Windows.Input;

namespace NoteBunny.FrontEnd.Wpf.DotNetSix.Viewmodels
{
    public partial class NewNoteViewModel: ObservableObject
    {
        private readonly INoteRepository _noteRepository;
        private readonly ITagRepository _tagRepository;

        private readonly Note? _existingNote;
        
        public event Action OnSave;

        public string Title => _existingNote is null ? "New note" : "Edit note";

        [ObservableProperty]
        private string _subject;

        [ObservableProperty]
        private string _content;

        [ObservableProperty]
        private string _tags;

        private void Save()
        {
            _tagRepository.AddTagsFromString(Tags);
            var tags = _tagRepository.GetTagIdsFromNames(Tags);

            var note = new Note()
            {
                Subject = Subject,
                Content = Content,
                TagIds = tags,
                IsPinned = _existingNote?.IsPinned ?? false
            };

            if (_existingNote is null)
            {
                _noteRepository.Add(note);
            } else
            {
                note.Id = _existingNote.Id;
                _noteRepository.Update(note);
            }

            _noteRepository.Save();
            OnSave?.Invoke();
        }

        public ICommand HandleSave { get; set; }

        public NewNoteViewModel(INoteRepository noteRepository, ITagRepository tagRepository, Note? existing = null)
        {
            _noteRepository = noteRepository;
            _tagRepository = tagRepository;
            HandleSave = new RelayCommand(Save);
            
            _existingNote = existing;
            _subject = _existingNote?.Subject ?? string.Empty;
            _content = _existingNote?.Content ?? string.Empty;
            _tags = _existingNote is not null
                ? string.Join(", ", _existingNote.Tags.Select(x => x.Name))
                : string.Empty;
        }
    }
}
