﻿using NoteBunny.BLL.Models;

namespace NoteBunny.FrontEnd.Wpf.DotNetSix.Viewmodels
{
    public record NoteViewModel
    {
        public NoteViewModel(string subject, string id, bool? isPinned)
        {
            Subject = subject;
            Id = id;
            IsPinned = isPinned;
        }

        public string Subject { get; init; }
        public string Id { get; init; }
        public bool? IsPinned { get; init; }

        public static NoteViewModel FromNote(Note note) => new(note.Subject, note.Id, note.IsPinned);
    }
}
