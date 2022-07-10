using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using NoteBunny.BLL.Interfaces;
using NoteBunny.BLL.Models;
using System;
using System.Linq;
using System.Windows.Input;

namespace NoteBunny.FrontEnd.Wpf.DotNetSix.Viewmodels
{
    public partial class NewNoteViewModel : ObservableObject
    {
        private readonly INoteRepository _noteRepository;
        private readonly ITagRepository _tagRepository;

        private readonly Note? _existingNote;

        public event Action OnSave;

        public string Title => _existingNote is null ? "New note" : "Edit note";

        public bool IsDirty => !_canClose && _existingNote is null
            ? !string.IsNullOrEmpty(Subject) || !string.IsNullOrEmpty(Content) || !string.IsNullOrEmpty(Tags)
            : Subject != _existingNote.Subject || Content != _existingNote.Content;

        [ObservableProperty]
        private bool _useMonospace = true;

        [ObservableProperty]
        private string _subject;

        [ObservableProperty]
        private string _content;

        [ObservableProperty]
        private string _tags;

        [ObservableProperty]
        private bool _canClose;

        private void Save()
        {
            _tagRepository.AddTagsFromString(Tags);
            var tags = _tagRepository.GetTagIdsFromNames(Tags);

            var note = new Note()
            {
                Subject = Subject,
                Content = Content,
                TagIds = tags,
                IsPinned = _existingNote?.IsPinned ?? false,
                CreatedOn = _existingNote?.CreatedOn ?? DateTime.Now,
                Id = _existingNote?.Id ?? Guid.NewGuid().ToString()
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
            CanClose = true;
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
