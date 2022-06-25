using FluentAssertions;
using NoteBunny.BLL.Helpers;
using NoteBunny.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteBunny.BLL.Test
{
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
            var result = NoteFilter.BySearchTerm(notes, searchTerm);

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
            var result = NoteFilter.BySearchTerm(notes, searchTerm);

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
            var result = NoteFilter.BySearchTerm(notes, searchTerm);

            // Assert
            result.Should().HaveCount(2);
            result.Should().OnlyHaveUniqueItems();
            result.Any(x => x.Tags.Single().Name == "something-else").Should().BeFalse();
        }
    }
}
