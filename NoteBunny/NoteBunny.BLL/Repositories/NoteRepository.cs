using NoteBunny.BLL.Interfaces;
using NoteBunny.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteBunny.BLL.Repositories
{
    public class NoteRepository
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
        public Note FindById(string id) => _noteRepo.FindById(id);
        public Note First(Predicate<Note> condition) => _noteRepo.First(condition);
        public IQueryable<Note> GetAll() => GetNotesWithTags().AsQueryable();

        public IEnumerable<Note> GetNotesWithTags()
        {
            var notes = _noteRepo.GetAll();
            var enrichedNotes = new List<Note>();
            foreach (var note in notes)
            {
                note.Tags = _tagRepo.GetTagsFromIds(note.TagIds);
            }
            return notes;
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Update(Note record)
        {
            throw new NotImplementedException();
        }
    }
}
