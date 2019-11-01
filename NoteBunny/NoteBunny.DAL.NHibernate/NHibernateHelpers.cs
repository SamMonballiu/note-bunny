using NoteBunny.BLL.Interfaces;
using NoteBunny.BLL.Models;
using NoteBunny.BLL.Repositories;
using NoteBunny.DAL.NHibernate.Repositories;

namespace NoteBunny.DAL.NHibernate
{
    public class NHibernateHelpers
    {
        public static (ITagRepository tagRepository, INoteRepository noteRepository) GetNhRepositories()
        {
            var tagsRepoNh = new NHibRepository<Tag>();
            var tagRepository = new TagRepository(tagsRepoNh);
            var noteRepoNh = new NHibRepository<Note>();
            var noteRepository = new NoteRepository(noteRepoNh, tagRepository);

            return (tagRepository, noteRepository);
        }
    }
}
