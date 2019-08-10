using NoteBunny.BLL.Models;
using NoteBunny.BLL.Repositories;
using NoteBunny.DAL.Xml.Helpers;
using NoteBunny.FrontEnd.Wpf.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NoteBunny.FrontEnd.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NoteRepository notesRepository;
        private TagRepository tags;

        private ObservableCollection<Note> notesCollection = new ObservableCollection<Note>();

        public MainWindow()
        {
            InitializeComponent();
            var repos = XmlHelpers.GetXmlRepositories("tags.xml", "notes.xml");
            notesRepository = repos.noteRepository;
            tags = repos.tagRepository;

            UpdateNotesList();
        }

        #region EVENT HANDLERS 
        private void Btn_NewNote_Click(object sender, RoutedEventArgs e)
        {
            new NewNote().ShowDialog();
            UpdateNotesList();
        }
        private void LstNotes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ShowSelectedNote();
        }
        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            UpdateNotesList();
        }
        private void Btn_SearchTags_Click(object sender, RoutedEventArgs e)
        {
            DoSearch();
        }

        private void Btn_ClearSearchResults_Click(object sender, RoutedEventArgs e)
        {
            ClearSearchResults();
        }

        private void Btn_DeleteNote_Click(object sender, RoutedEventArgs e)
        {
            var note = GetSelectedNote();
            if (note is null) return;
            if (MessageBox.Show("Confirm", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                notesRepository.Delete(note);
                notesRepository.Save();
                UpdateNotesList();
            }
        }

        private void TxtSearchTagName_TextChanged(object sender, TextChangedEventArgs e)
        {
            var senderTxt = sender as TextBox;
            spTagSearchRequirements.IsEnabled = senderTxt.Text.Contains(',');
            if (senderTxt.Text == String.Empty && txtSearchNoteSubjectContent.Text == String.Empty) ClearSearchResults();
        }

        private void Btn_SearchNotes_Click(object sender, RoutedEventArgs e)
        {
            var searchText = txtSearchNoteSubjectContent.Text;
            if (String.IsNullOrEmpty(searchText))
            {
                return;
            }

            lstNotes.ItemsSource = SearchNotes(notesRepository.GetAll(), searchText);
        }

        private void TxtSearchNoteSubjectContent_TextChanged(object sender, TextChangedEventArgs e)
        {
            var senderTxt = sender as TextBox;
            if (senderTxt.Text == String.Empty && txtSearchTagName.Text == String.Empty) ClearSearchResults();
        }

        private void Btn_EditNote_Click(object sender, RoutedEventArgs e)
        {
            EditNote();
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            Key[] keys = { Key.Enter, Key.Home };
            if (!keys.Contains(e.Key)) { return; }

            IInputElement focusedControl = Keyboard.FocusedElement;
            var focusedType = focusedControl.GetType();

            if (e.Key == Key.Home)
            {
                ClearSearchResults();
            }

            else if (focusedType == typeof(TextBox) && e.Key == Key.Enter)
            {
                DoSearch();
            }
        }
        #endregion

        #region Methods
        private void ShowSelectedNote()
        {
            new NoteDetails(GetSelectedNote()).ShowDialog();
            UpdateNotesList();
        }
        private Note GetSelectedNote() => (Note)lstNotes.SelectedItem;
        private void UpdateNotesList()
        {
            notesCollection.Clear();
            foreach (var item in notesRepository.GetAll())
            {
                notesCollection.Add(item);
            }
            lstNotes.ItemsSource = null;
            lstNotes.ItemsSource = notesCollection;
        }
        private void DoSearch()
        {
            if (String.IsNullOrWhiteSpace(txtSearchTagName.Text) && String.IsNullOrWhiteSpace(txtSearchNoteSubjectContent.Text))
            {
                return;
            }

            var noteResults = new List<Note>();
            // search by tags if required
            if (!String.IsNullOrWhiteSpace(txtSearchTagName.Text))
            {
                var tagSearchTerms = txtSearchTagName.Text.Split(',').Select(p => p.Trim());
                var tagResults = new List<Tag>();

                foreach (var tag in tags.GetAll())
                {
                    foreach (var term in tagSearchTerms)
                    {
                        if (tag.Name.ToLower().Contains(term.ToLower()))
                        {
                            if (!tagResults.Contains(tag)) tagResults.Add(tag);
                        }
                    }
                }
                var ids = tagResults.Select(p => p.Id);
                if (ids.Count() > 0)
                {
                    var notesMustHaveAllTags = (bool)radTagSearchAll.IsChecked;

                    if (notesMustHaveAllTags)
                    {
                        foreach (var note in notesRepository.GetAll())
                        {
                            if (ids.All(id => note.TagIds.Contains(id)))
                            {
                                noteResults.Add(note);
                            }
                        }
                    }

                    else
                    {
                        foreach (var note in notesRepository.GetAll())
                        {
                            if (ids.Any(id => note.TagIds.Contains(id)))
                            {
                                noteResults.Add(note);
                            }
                        }
                    }
                }
            }


            if (!String.IsNullOrEmpty(txtSearchNoteSubjectContent.Text))
            {
                noteResults = SearchNotes(noteResults.Count == 0 ? notesRepository.GetAll().ToList() : noteResults, txtSearchNoteSubjectContent.Text).ToList();
            }
            lstNotes.ItemsSource = noteResults;
            lblHeading.Content = $"All notes ({noteResults.Count})";
        }
        private void ClearSearchResults()
        {
            txtSearchNoteSubjectContent.Text = String.Empty;
            txtSearchTagName.Text = String.Empty;
            UpdateNotesList();
            lblHeading.Content = "All notes";
        }
        private IEnumerable<Note> SearchNotes(IEnumerable<Note> notes, string searchTerm)
        {
            Func<Note, string, bool> noteContentContainsString = (Note note, string term) => note.Content.ToLower().Contains(term.ToLower());
            Func<Note, string, bool> noteSubjectContainsString = (Note note, string term) => note.Subject.ToLower().Contains(term.ToLower());

            var results = notes.Where(x => noteContentContainsString(x, searchTerm) || noteSubjectContainsString(x, searchTerm));
            return results;
        }
        private void EditNote()
        {
            var note = GetSelectedNote();
            ClearSearchResults();
            new NoteDetails(note, Enumerations.NoteState.Edit).ShowDialog();
            UpdateNotesList();
        }
        #endregion

        
    }
}
