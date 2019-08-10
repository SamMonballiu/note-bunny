using NoteBunny.BLL.Models;
using NoteBunny.BLL.Repositories;
using NoteBunny.DAL.Xml.Repositories;

namespace NoteBunny.DAL.Xml.Helpers
{
    public class XmlHelpers
    {
        public static (TagRepository tagRepository, NoteRepository noteRepository) GetXmlRepositories(string tagsFilename, string notesFilename)
        {
            var tagsRepoXml = new XmlRepository<Tag>(tagsFilename);
            var tagRepository = new TagRepository(tagsRepoXml);
            var noteRepoXml = new XmlRepository<Note>(notesFilename);
            var noteRepository = new NoteRepository(noteRepoXml, tagRepository);

            return (tagRepository, noteRepository);
        }
    }
}
