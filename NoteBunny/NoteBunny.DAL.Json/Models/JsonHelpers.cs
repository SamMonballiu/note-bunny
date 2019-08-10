using NoteBunny.BLL.Models;
using NoteBunny.BLL.Repositories;
using NoteBunny.DAL.Json.Repositories;

namespace NoteBunny.DAL.Json.Models
{
    public class JsonHelpers
    {
        public static (TagRepository tagRepository, NoteRepository noteRepository) GetJsonRepositories(string tagsFilename, string notesFilename)
        {
            var tagsRepoXml = new JsonRepository<Tag>(tagsFilename);
            var tagRepository = new TagRepository(tagsRepoXml);
            var noteRepoXml = new JsonRepository<Note>(notesFilename);
            var noteRepository = new NoteRepository(noteRepoXml, tagRepository);

            return (tagRepository, noteRepository);
        }
    }
}
