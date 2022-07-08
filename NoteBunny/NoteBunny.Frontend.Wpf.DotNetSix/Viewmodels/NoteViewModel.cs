using NoteBunny.BLL.Models;
using NoteBunny.Frontend.Wpf.DotNetSix.Models;
using System;

namespace NoteBunny.FrontEnd.Wpf.DotNetSix.Viewmodels
{
    public record NoteViewModel
    {
        public NoteViewModel(string subject, string id, bool? isPinned, bool isCode)
        {
            Subject = subject;
            Id = id;
            IsPinned = isPinned;
            IsCode = isCode;
        }

        public string Subject { get; init; }
        public string Id { get; init; }
        public bool? IsPinned { get; init; }
        public bool IsCode { get; init; }
        public string? Tags { get; init; }

        public static NoteViewModel FromNote(Note note) => new(note.Subject, note.Id, note.IsPinned, note.IsCode);
        public static NoteViewModel FromNoteDto(NoteDto dto) => new(dto.Subject, dto.Id, false, false) { Tags = dto.Tags };
    }
}
