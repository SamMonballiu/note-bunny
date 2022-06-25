using FluentAssertions;
using NoteBunny.BLL.Helpers;
using NoteBunny.BLL.Models;
using NoteBunny.BLL.Enums;
using MatchType = NoteBunny.BLL.Enums.MatchType;

namespace NoteBunny.BLL.Test;

[TestFixture]
internal class TestFixtureNoteFilter
{
    private IEnumerable<Note> GetNotes(params string[] subjects)
    {
        foreach (var subject in subjects)
        {
            yield return new Note
            {
                Tags = new List<Tag>(),
                Subject = subject,
                Content = string.Empty
            };
        }
    }

    [Test]
    public void Test_NoteFilter_BySearchTerm()
    {
        // Arrange
        var notes = GetNotes("Abraham", "Sophie").ToList();
        const string searchTerm = "sophie";

        // Act
        var result = NoteFilter.BySearchTerm(notes, searchTerm, NoteFilterType.Subject);

        // Assert
        result.Should().HaveCount(1);
        result.Single().Subject.Should().Be("Sophie");
    }

    [Test]
    public void Test_NoteFilter_BySearchTerm_Multiple_Subject()
    {
        // Arrange
        var notes = GetNotes("Abraham", "Sophie", "Tim").ToList();
        const string searchTerm = "soph, abr";

        // Act
        var result = NoteFilter.BySearchTerm(notes, searchTerm, NoteFilterType.All);

        // Assert
        result.Should().HaveCount(2);
        result.Should().OnlyHaveUniqueItems();
        result.Any(x => x.Subject == "Tim").Should().BeFalse();
    }

    [Test]
    public void Test_NoteFilter_BySearchTerm_Multiple_TagMatch()
    {
        // Arrange
        var notes = GetNotes("First", "Second", "Third").ToList();
        notes[0].Tags.Add(new Tag("business"));
        notes[1].Tags.Add(new Tag("pleasure"));
        notes[2].Tags.Add(new Tag("something-else"));

        const string searchTerm = "business, pleasure";

        // Act
        var result = NoteFilter.BySearchTerm(notes, searchTerm, NoteFilterType.All);

        // Assert
        result.Should().HaveCount(2);
        result.Should().OnlyHaveUniqueItems();
        result.Any(x => x.Tags.Single().Name == "something-else").Should().BeFalse();
    }

    [Test]
    public void Test_NoteFilter_BySearchTerm_MatchAll()
    {
        // Arrange
        var first = new Note
        {
            Id = "first",
            Subject = "Food",
            Content = "pizza canneloni spaghetti",
            Tags = new List<Tag>()
        };

        var second = new Note
        {
            Id = "second",
            Subject = "Food #2",
            Content = "spaghetti",
            Tags = new List<Tag>()
        };

        const string searchTerm = "pizza, spaghetti";

        // Act
        var result = NoteFilter.BySearchTerm(new List<Note> { first, second }, searchTerm, NoteFilterType.Content, MatchType.All);

        // Assert
        result.Should().HaveCount(1);
        result.Single().Should().BeEquivalentTo(first);
    }
}
