using NoteBunny.BLL.Models;
using NoteBunny.BLL.Repositories;
using NoteBunny.DAL.Xml.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteBunny.ConsoleTest
{
    class Program
    {
        
        static void Main(string[] args)
        {
            

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("? ");
                Console.ResetColor();
                var input = Console.ReadLine();
                switch (input)
                {
                    case "quit":
                    case "q":
                        Environment.Exit(0);
                        break;
                    case "tagnew":
                    case "newtag":
                        ShowNewTagMenu();
                        break;
                    case "tagsearch":
                        ShowTagIdSearchMenu();
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

        private static void ShowTagIdSearchMenu()
        {
            var repos = GetStandardRepositories();
            Console.WriteLine("[SEARCH TAGS BASED ON NAMES]");
            Console.Write("? ");
            var input = Console.ReadLine();
            var searchTerms = input.Replace(" ", String.Empty).Split(',').ToList();
            var results = repos.tagRepository.GetTagIdsFromNames(searchTerms);
            results.ForEach(id => Console.WriteLine(id));
        }

        private static (XmlRepository<Tag> tagsRepoXml, TagRepository tagRepository, XmlRepository<Note> noteRepository) GetStandardRepositories()
        {
            var tagsRepoXml = new XmlRepository<Tag>("tags.xml");
            var tagRepository = new TagRepository(tagsRepoXml);
            var noteRepository = new XmlRepository<Note>("notes.xml");

            return (tagsRepoXml, tagRepository, noteRepository);
        }

        private static void ShowNewTagMenu()
        {
            var repos = GetStandardRepositories();
            Console.WriteLine("[NEW TAG]");
            Console.Write("? ");
            var input = Console.ReadLine();
            repos.tagRepository.AddTagsFromString(input);
        }

        private static void ShowNewNoteMenu()
        {
            var tagsRepoXml = new XmlRepository<Tag>("tags.xml");
            var tagRepository = new TagRepository(tagsRepoXml);
            var noteRepository = new XmlRepository<Note>("notes.xml");

            Console.WriteLine("[New note]");
            Console.Write("Content: ");
            var content = Console.ReadLine();
            Console.Write("Tags: ");
            var tags = tagRepository.GetTagsFromString(Console.ReadLine());

            var note = new Note()
            {
                Content = content,
                Tags = tags
            };

            noteRepository.Add(note);
            noteRepository.Save();
        }
    }
}
