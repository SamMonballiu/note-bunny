using NoteBunny.BLL.Interfaces;
using NoteBunny.BLL.Models;
using NoteBunny.BLL.Repositories;
using NoteBunny.DAL.Json.Models;
using NoteBunny.DAL.Xml.Helpers;
using NoteBunny.FrontEnd.Wpf.DotNetSix.Helpers;
using NoteBunny.FrontEnd.Wpf.DotNetSix.Viewmodels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NoteBunny.FrontEnd.Wpf.DotNetSix.Windows
{
    /// <summary>
    /// Interaction logic for NewNote.xaml
    /// </summary>
    public partial class NewNote : Window
    {
        private readonly INoteRepository noteRepository;
        private readonly ITagRepository tagRepository;

        public NewNote(Note? existingNote = null)
        {
            InitializeComponent();
            var repos = RepositoryFactory.GetJsonRepositories();
            noteRepository = repos.noteRepository;
            tagRepository = repos.tagRepository;
            DataContext = new NewNoteViewModel(noteRepository, tagRepository, existingNote);
            txtSubject.Focus();

            (DataContext as NewNoteViewModel)!.OnSave += () => {
                DialogResult = true;
                Close();
            };
        }
    }
}
