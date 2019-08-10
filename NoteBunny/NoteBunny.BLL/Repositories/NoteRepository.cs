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
    }
}
