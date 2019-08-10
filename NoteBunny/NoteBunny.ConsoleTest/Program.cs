using NoteBunny.BLL.Models;
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
            var notesRepo = new XmlRepository<Note>("notes.xml");

            var note = new Note()
            {
                Content = "Hello",
                Tags = new List<Tag>()
                {
                    new Tag()
                    {
                        Name = "Unit Testing"
                    },

                    new Tag()
                    {
                        Name = "Doops"
                    }
                }
            };

            notesRepo.Add(note);
            notesRepo.Save();

            Console.ReadKey();
        }
    }
}
