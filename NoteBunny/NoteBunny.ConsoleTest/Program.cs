using NoteBunny.BLL.Models;
using NoteBunny.BLL.Repositories;
using NoteBunny.DAL.Json.Models;
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
                        ShowTagIdSearchMenu();
                        break;
                    case "shownotes":
                        ShowNoteDetails();
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
            var repos = JsonHelpers.GetJsonRepositories("tags.json", "notes.json");
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
            var repos = GetStandardRepositories();
            Console.WriteLine("[SEARCH TAGS BASED ON NAMES]");
            Console.Write("? ");
            var input = Console.ReadLine();
            var searchTerms = input.Split(',').ToList();
            var results = repos.tagRepository.GetTagIdsFromNames(searchTerms);
            results.ForEach(id => Console.WriteLine(id));
        }

        private static (TagRepository tagRepository, NoteRepository noteRepository) GetStandardRepositories()
        {
            var tagsRepoXml = new XmlRepository<Tag>("tags.xml");
            var tagRepository = new TagRepository(tagsRepoXml);
            var noteRepoXml = new XmlRepository<Note>("notes.xml");
            var noteRepository = new NoteRepository(noteRepoXml, tagRepository);

            return (tagRepository, noteRepository);
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
            var repos = JsonHelpers.GetJsonRepositories("tags.json", "notes.json");

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
