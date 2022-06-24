using NoteBunny.BLL.Models;
using NoteBunny.BLL.Repositories;
using NoteBunny.DAL.Json.Models;
using NoteBunny.DAL.NHibernate;
using NoteBunny.DAL.NHibernate.Models;
using NoteBunny.DAL.NHibernate.Repositories;
using NoteBunny.DAL.Xml.Repositories;
using NoteBunny.Frontend.Wpf.DotNetSix.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteBunny.ConsoleTest
{
    class Program
    {
        public enum RepositoryType { Xml, Json, NHibernate }
        private static RepositoryType repoType = RepositoryType.NHibernate;
        static void Main(string[] args)
        {
            //using (var session = SessionFactory.OpenSession())
            //{
            //    using (var transaction = session.BeginTransaction())
            //    {
            //        var notes = session.Query<Note>();
            //        var noteb = session.Load<Note>("1");
            //        foreach (var note in notes)
            //        {
            //            Console.WriteLine(note.Subject);
            //        }
            //    }
            //}

            //var noteRepo = new NHibRepository<Note>();
            //var tagRepo = new NHibRepository<Tag>();
            //var tags = tagRepo.GetAll();
            //var notes = noteRepo.GetAll();

            //foreach (var note in notes)
            //{
            //    Console.WriteLine(note);
            //    note.Tags.ToList().ForEach(x => Console.WriteLine(x));
            //}

            //return;

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("? ");
                Console.ResetColor();
                var input = Console.ReadLine();
                switch (input)
                {
                    case "":
                        break;
                    case "quit":
                    case "q":
                        Environment.Exit(0);
                        break;
                    case "tagnew":
                    case "newtag":
                        ShowNewTagMenu();
                        break;
                    case "tagsearch":
                    case "searchtags":
                        ShowTagIdSearchMenu();
                        break;
                    case "shownotes":
                        ShowNoteDetails();
                        break;
                    case "showtags":
                        var repos = GetStandardRepositories(repoType);
                        repos.tagRepository.GetAll().ToList().ForEach(x => Console.WriteLine(x.Name));
                        break;
                    case "newnote":
                        ShowNewNoteMenu();
                        break;
                    default:
                        Console.WriteLine("unknown command");
                        break;
                }
            }
        }

        private static void ShowNoteDetails()
        {
            var repos = GetStandardRepositories(repoType);
            foreach (var note in repos.noteRepository.GetNotesWithTags())
            {
                Console.WriteLine("[Content] " + note.Content);
                Console.WriteLine("[Tags]");
                foreach (var tag in note.Tags)
                {
                    Console.WriteLine(tag.Name);
                }
            }
        }

        private static void ShowTagIdSearchMenu()
        {
            var repos = GetStandardRepositories(repoType);
            Console.WriteLine("[SEARCH TAGS BASED ON NAMES]");
            Console.Write("? ");
            var input = Console.ReadLine();
            var searchTerms = input.Split(',').ToList();
            var results = repos.tagRepository.GetTagIdsFromNames(searchTerms);
            results.ForEach(id => Console.WriteLine(id));
        }

        private static (TagRepository tagRepository, NoteRepository noteRepository) GetStandardRepositories(RepositoryType type = RepositoryType.Xml)
        {
            switch (type)
            {
                case RepositoryType.Xml:
                    var tagsRepoXml = new XmlRepository<Tag>("tags.xml");
                    var tagRepository = new TagRepository(tagsRepoXml);
                    var noteRepoXml = new XmlRepository<Note>("notes.xml");
                    var noteRepository = new NoteRepository(noteRepoXml, tagRepository);
                    return (tagRepository, noteRepository);
                case RepositoryType.Json:
                    var repos = RepositoryFactory.GetJsonRepositories();
                    return ((TagRepository)repos.tagRepository, (NoteRepository)repos.noteRepository);
                case RepositoryType.NHibernate:
                    var nhRepos = NHibernateHelpers.GetNhRepositories();
                    return ((TagRepository)nhRepos.tagRepository, (NoteRepository)nhRepos.noteRepository);
                default:
                    throw new Exception("Standard repositories don't exist for this type yet.");
            }
        }

        private static void ShowNewTagMenu()
        {
            var repos = GetStandardRepositories(repoType);
            Console.WriteLine("[NEW TAG]");
            Console.Write("? ");
            var input = Console.ReadLine();
            repos.tagRepository.AddTagsFromString(input);
        }

        private static void ShowNewNoteMenu()
        {
            //var repos = JsonHelpers.GetJsonRepositories("tags.json", "notes.json");
            var repos = GetStandardRepositories(repoType);
            Console.WriteLine("[New note]");
            Console.Write("Content: ");
            var content = Console.ReadLine();
            Console.Write("Tags: ");
            var tags = repos.tagRepository.GetTagsFromString(Console.ReadLine());

            var note = new Note()
            {
                Content = content,
                TagIds = repos.tagRepository.GetTagIdsFromNames(tags.Select(x => x.Name).ToList())
            };

            repos.noteRepository.Add(note);
            repos.noteRepository.Save();
        }
    }
}
