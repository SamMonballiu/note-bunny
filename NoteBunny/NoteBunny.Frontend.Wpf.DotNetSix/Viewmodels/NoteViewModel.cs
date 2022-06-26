using NoteBunny.BLL.Models;

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

        public static NoteViewModel FromNote(Note note) => new(note.Subject, note.Id, note.IsPinned, note.IsCode);
    }
}
