using NoteBunny.BLL.Models;

namespace NoteBunny.Frontend.Wpf.DotNetSix.Models
{
    public record NoteDto
    {
        public string Subject { get; init; } = string.Empty;
        public string Id { get; init; }
        public string? Tags { get; init; }
        public string Content { get; init; } = string.Empty;

        public static NoteDto FromNote(Note note) => new()
        {
            Subject = note.Subject,
            Content = note.Content,
            Id = note.Id,
            Tags = note.Tags switch
            {
                null => null,
                not null => string.Join(",", note.Tags),
            },
        };
    }
}
