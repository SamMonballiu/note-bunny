using NoteBunny.BLL.Interfaces;
using NoteBunny.DAL.Json.Models;
using NoteBunny.DAL.Xml.Helpers;

namespace NoteBunny.FrontEnd.Wpf.Helpers
{
    public static class RepositoryFactory
    {
        public static (ITagRepository tagRepository, INoteRepository noteRepository) GetJsonRepositories()
            => JsonHelpers.GetJsonRepositories("tags.json", "notes.json");
        public static (ITagRepository tagRepository, INoteRepository noteRepository) GetXmlRepositories()
            => XmlHelpers.GetXmlRepositories("tags.xml", "notes.xml");
    }
}
