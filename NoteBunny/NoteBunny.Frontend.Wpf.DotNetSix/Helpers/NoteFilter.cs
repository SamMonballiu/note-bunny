using NoteBunny.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NoteBunny.Frontend.Wpf.DotNetSix.Helpers
{
    internal class NoteFilter
    {
        public static List<Note> BySearchTerm(List<Note> notes, string searchTerm)
        {
            var noteResults = new List<Note>();
            var term = searchTerm.ToLower();

            // Notes whose content or subject matches the term
            Func<Note, bool> MatchesContent = note => note.Content.ToLower().Contains(term);
            var contentMatches = notes.Where(MatchesContent).ToList();

            Func<Note, bool> MatchesSubject = note => note.Subject.ToLower().Contains(term);
            var subjectMatches = notes.Where(MatchesSubject).ToList();

            // Notes that have a tag matching the term
            Func<Note, bool> MatchesTags = note => note.Tags.Select(x => x.Name.ToLower()).Any(name => name.Contains(term));
            var tagMatches = notes.Where(MatchesTags).ToList();

            return contentMatches
                .Union(subjectMatches)
                .Union(tagMatches)
                .DistinctBy(x => x.Id)
                .ToList();
        }
    }
}
