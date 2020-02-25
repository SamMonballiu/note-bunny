using NoteBunny.BLL.Interfaces;
using NoteBunny.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NoteBunny.BLL.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private IRepository<Note> _noteRepo;
        private TagRepository _tagRepo;

        public NoteRepository(IRepository<Note> noteRepo, TagRepository tagRepo)
        {
            _noteRepo = noteRepo;
            _tagRepo = tagRepo;
        }

        public void Add(Note record) => _noteRepo.Add(record);
        public bool Delete(Note record) => _noteRepo.Delete(record);
        public bool Delete(Predicate<Note> condition) => _noteRepo.Delete(condition);
        public Note FindById(string id) => GetNoteWithTags(_noteRepo.FindById(id));
        public Note First(Predicate<Note> condition) => _noteRepo.First(condition);
        //public IQueryable<Note> GetAll() => GetNotesWithTags().AsQueryable();
        public IQueryable<Note> GetAll() => _noteRepo.GetAll();
        public void Save() => _noteRepo.Save();
        public void Update(Note record) => _noteRepo.Update(record);

        public void TogglePinned(Note note)
        {
            note.IsPinned = !note.IsPinned;
            _noteRepo.Update(note);
            _noteRepo.Save();
        }
        public IEnumerable<Note> GetNotesWithTags()
        {
            var notes = _noteRepo.GetAll();
            var enrichedNotes = new List<Note>();
            foreach (var note in notes)
            {
                note.Tags = _tagRepo.GetTagsFromIds(note.TagIds?.ToList());
            }
            return notes;
        }

        public Note GetNoteWithTags(Note note)
        {
            note.Tags = _tagRepo.GetTagsFromIds(note.TagIds?.ToList());
            return note;
        }

        public IEnumerable<string> GetNoteSubjects()
        {
            return _noteRepo.GetAll().Select(p => p.Subject);
        }
    }
}
