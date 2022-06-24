using NoteBunny.BLL.Interfaces;
using NoteBunny.BLL.Models;
using NoteBunny.DAL.Json.Models;
using NoteBunny.DAL.NHibernate.Repositories;
using NoteBunny.DAL.Xml.Helpers;

namespace NoteBunny.FrontEnd.Wpf.DotNetSix.Helpers
{
    public static class RepositoryFactory
    {
        public static (ITagRepository tagRepository, INoteRepository noteRepository) GetJsonRepositories()
            => JsonHelpers.GetJsonRepositories("tags.json", "notes.json");
        public static (ITagRepository tagRepository, INoteRepository noteRepository) GetXmlRepositories()
            => XmlHelpers.GetXmlRepositories("tags.xml", "notes.xml");

        public static (ITagRepository tagRepository, INoteRepository noteRepository) GetNHibernateRepositories()
            => XmlHelpers.GetXmlRepositories("tags.xml", "notes.xml");

        public static IRepository<Note> GetNHibRepositoryTest() => new NHibRepository<Note>();
    }
}
