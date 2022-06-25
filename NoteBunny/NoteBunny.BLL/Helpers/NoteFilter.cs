using NoteBunny.BLL.Enums;
using On = NoteBunny.BLL.Enums.NoteFilterType;
using Match = NoteBunny.BLL.Enums.MatchType;
using NoteBunny.BLL.Extensions;
using NoteBunny.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NoteBunny.BLL.Helpers
{
    public class NoteFilter
    {
        public static List<Note> BySearchTerm(List<Note> notes, string searchTerm, On searchType, Match matchType = Match.Any)
        {
            var noteResults = new List<Note>();
            var terms = searchTerm.ToLower().Split(",").Select(x => x.Trim());

            bool Is(params On[] types) => types.Any(t => searchType.Equals(t));
            bool matchAll = matchType.Equals(Match.All);

            // Notes whose content or subject matches the term
            Func<Note, bool> MatchesContent = note => note.Content.Contains(terms, matchType);
            var contentMatches = Is(On.All, On.Content)
                ? notes.Where(MatchesContent) 
                : Enumerable.Empty<Note>();

            Func<Note, bool> MatchesSubject = note => note.Subject.Contains(terms, matchType);
            var subjectMatches = Is(On.All, On.Subject)
                ? notes.Where(MatchesSubject) 
                : Enumerable.Empty<Note>();

            // Notes that have a tag matching the term
            Func<Note, bool> MatchesTags = note => {
                var tagNames = note.Tags?.Select(x => x.Name.ToLowerInvariant()) ?? Enumerable.Empty<string>();
                bool TermIsInTags(string x) => tagNames!.Contains(x.ToLowerInvariant());
                return matchAll
                    ? terms.All(TermIsInTags)
                    : terms.Any(TermIsInTags);
                };
            var tagMatches = Is(On.All, On.Tags)
                ? notes.Where(MatchesTags) 
                : Enumerable.Empty<Note>();

            return contentMatches
                .Union(subjectMatches)
                .Union(tagMatches)
                .DistinctBy(x => x.Id)
                .ToList();
        }
    }
}
