using NoteBunny.BLL.Interfaces;
using NoteBunny.BLL.Models;
using NoteBunny.FrontEnd.Wpf.DotNetSix.Helpers;
using NoteBunny.FrontEnd.Wpf.DotNetSix.Viewmodels;
using System.Windows;

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
