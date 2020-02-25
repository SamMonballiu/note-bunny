using System;
using System.Collections.Generic;
using System.Linq;
using NoteBunny.BLL.Models;

namespace NoteBunny.BLL.Interfaces
{
    public interface INoteRepository
    {
        void Add(Note record);
        bool Delete(Note record);
        bool Delete(Predicate<Note> condition);
        Note FindById(string id);
        Note First(Predicate<Note> condition);
        IQueryable<Note> GetAll();
        IEnumerable<Note> GetNotesWithTags();
        Note GetNoteWithTags(Note note);
        void Save();
        void Update(Note record);
        void TogglePinned(Note note);
    }
}