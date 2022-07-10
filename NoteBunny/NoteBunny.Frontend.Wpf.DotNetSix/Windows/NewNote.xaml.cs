using NoteBunny.BLL.Interfaces;
using NoteBunny.BLL.Models;
using NoteBunny.FrontEnd.Wpf.DotNetSix.Helpers;
using NoteBunny.FrontEnd.Wpf.DotNetSix.Viewmodels;
using static NoteBunny.Frontend.Wpf.DotNetSix.Helpers.Helpers;
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

        private NewNoteViewModel Viewmodel => (DataContext as NewNoteViewModel)!;

        public NewNote(Note? existingNote = null)
        {
            InitializeComponent();
            var repos = RepositoryFactory.GetJsonRepositories();
            noteRepository = repos.noteRepository;
            tagRepository = repos.tagRepository;
            DataContext = new NewNoteViewModel(noteRepository, tagRepository, existingNote);
            txtSubject.Focus();

            Viewmodel.OnSave += () => {
                DialogResult = true;
                Close();
            };

            Closing += (_, e) =>
            {
                if (!Viewmodel.IsDirty && Confirm("Are you sure? Changes will be lost.") == false)
                {
                    e.Cancel = true;
                    return;
                }
            };
        }
    }
}
