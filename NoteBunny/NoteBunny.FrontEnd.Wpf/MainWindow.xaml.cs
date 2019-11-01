using NoteBunny.BLL.Interfaces;
using NoteBunny.BLL.Models;
using NoteBunny.FrontEnd.Wpf.Helpers;
using NoteBunny.BLL.Repositories;
using NoteBunny.DAL.Json.Models;
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
        private INoteRepository notesRepository;
        private ITagRepository tags;
        private static Random rnd = new Random();
        private Sorter<Note, object> noteSorter = new NoteSorter<Note, object>();

        private ObservableCollection<Note> notesCollection = new ObservableCollection<Note>();

        public enum SortingOptions
        {
            CreatedOn,
            Subject,
            Id
        }

        private SortingOptions selectedSortingOption = SortingOptions.CreatedOn;

        public MainWindow()
        {
            InitializeComponent();
            var repos = RepositoryFactory.GetJsonRepositories();
            notesRepository = repos.noteRepository;
            tags = repos.tagRepository;

            try
            {
                noteSorter.SetSorter(x => x.CreatedOn);
                noteSorter.SortDirection = SortDirection.Descending;
                UpdateNotesList(noteSorter.SorterFunc);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            txtSearchAlt.Focus();

            cbxSortOptions.ItemsSource = Enum.GetNames(typeof(SortingOptions));
        }

        private void SetSortingOptions()
        {
            Func<Note, object> noteSortFunc = null;
            switch (selectedSortingOption)
            {
                case SortingOptions.CreatedOn:
                    noteSortFunc = x => x.CreatedOn;
                    break;
                case SortingOptions.Subject:
                    noteSortFunc = x => x.Subject;
                    break;
                case SortingOptions.Id:
                    noteSortFunc = x => x.Id;
                    break;
                default:
                    break;
            }
            noteSorter.SetSorter(noteSortFunc);

            switch (cbxSortAscending.IsChecked)
            {
                case true:
                    noteSorter.SortDirection = SortDirection.Ascending;
                    break;
                case false:
                    noteSorter.SortDirection = SortDirection.Descending;
                    break;
            }
        }

        #region EVENT HANDLERS 

        private void CbxSortOptions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedSortingOption = (SortingOptions)(sender as ComboBox).SelectedIndex;
            SetSortingOptions();
            UpdateNotesList(noteSorter.SorterFunc);
        }
        private void Btn_NewNote_Click(object sender, RoutedEventArgs e)
        {
            new NewNote().ShowDialog();
        }
        private void LstNotes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ShowSelectedNote();
        }
        //private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        //{
        //    UpdateNotesList();
        //}
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
            throw new NotImplementedException();
            //var note = GetSelectedNote();
            //if (note is null) return;
            //if (MessageBox.Show("Confirm", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
            //{
            //    notesRepository.Delete(note);
            //    notesRepository.Save();
            //    UpdateNotesList();
            //}
        }

        private void TxtSearchTagName_TextChanged(object sender, TextChangedEventArgs e)
        {
            var senderTxt = sender as TextBox;
            spTagSearchRequirements.IsEnabled = senderTxt.Text.Contains(',');
            if (senderTxt.Text == String.Empty && txtSearchNoteSubjectContent.Text == String.Empty) ClearSearchResults();
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
            throw new NotImplementedException();
            //new NoteDetails(GetSelectedNote()).ShowDialog();
            //UpdateNotesList();
        }
        private string GetSelectedSubject() => (string)lstNotes.SelectedItem;

        private void UpdateNotesList<TKey>(Func<Note, TKey> sorter, IEnumerable<Note> notes = null)
        {
            if (sorter is null)
            {
                //throw new ArgumentException("Must provide a sorter.");
                return;
            }

            notesCollection.Clear();
            notes = notes ?? notesRepository.GetAll();
            notes = noteSorter.Sort(notes);

            lstNotes.ItemsSource = notes.Select(p => p.Subject);

            statusTxt.Text = $"Total notes: {notesRepository.GetAll().Count()} | Search results: {notes.Count()} | Tags: {tags.GetAll().Count()}";

            //notes = notes ?? notesRepository.GetAll();

            //notesCollection.Clear();

            //Func<IEnumerable<Note>> sortByNameDescending = () => notes.OrderByDescending(x => x.Subject);
            //Func<IEnumerable<Note>> sortByDateDescending = () => notes.OrderByDescending(x => x.CreatedOn);
            //Func<IEnumerable<Note>> sortByIdDescending = () => notes.OrderByDescending(x => x.Id);

            //Func<IEnumerable<Note>> sortByNameAscending = () => notes.OrderBy(x => x.Subject);
            //Func<IEnumerable<Note>> sortByDateAscending = () => notes.OrderBy(x => x.CreatedOn);
            //Func<IEnumerable<Note>> sortByIdAscending = () => notes.OrderBy(x => x.Id);

            //Dictionary<(string, bool), Func<IEnumerable<Note>>> sortOptions = new Dictionary<(string, bool), Func<IEnumerable<Note>>>()
            //{
            //    { ("Name", true), sortByNameDescending },
            //    { ("Created On", true),  sortByDateDescending },
            //    { ("Id", true),  sortByIdDescending },
            //    { ("Name", false), sortByNameAscending },
            //    { ("Created On", false),  sortByDateAscending },
            //    { ("Id", false),  sortByIdAscending },
            //};

            //var sortedNotes = sortOptions[(sortBy, (bool)cbxSortAscending.IsChecked)].Invoke();

            //foreach (var item in sortedNotes)
            //{
            //    notesCollection.Add(item);
            //}
            //lstNotes.ItemsSource = null;
            //lstNotes.ItemsSource = notesCollection.Select(n => n.Subject);
            ////lblHeading.Content = $"All notes ({notesCollection.Count})";
            //statusTxt.Text = $"Total notes: {notesRepository.GetAll().Count()} | Search results: {notesCollection.Count} | Tags: {tags.GetAll().Count()}";
        }
        private void ClearSearchResults()
        {
            txtSearchNoteSubjectContent.Text = String.Empty;
            txtSearchTagName.Text = String.Empty;
            //UpdateNotesList();
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
            throw new NotImplementedException();
            //var note = GetSelectedNote();
            //ClearSearchResults();
            //new NoteDetails(note, Enumerations.NoteState.Edit).ShowDialog();
            //UpdateNotesList();
        }

        #endregion

        private void LstNotes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GetSelectedSubject() is string subject)
            {
                var note = notesRepository.First(x => x.Subject == subject);
                spSelected.DataContext = note;
                txtOnView.Visibility = String.IsNullOrWhiteSpace(note.Content) ? Visibility.Collapsed : Visibility.Visible;
            }
        }
        private void DoSearch()
        {
            if (txtSearchAlt.Text.Length < 3)
            {
                if (String.IsNullOrWhiteSpace(txtSearchAlt.Text))
                {
                    //TODO Update
                    //UpdateNotesList();
                }
                return;
            }

            var noteResults = new List<Note>();
            var tagSearchTerms = txtSearchAlt.Text.Split(',').Select(p => p.Trim());
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


            List<Note> contentSearchResults = new List<Note>();
            if (!txtSearchAlt.Text.Contains(','))
            {
                contentSearchResults = SearchNotes(notesRepository.GetAll().ToList(), txtSearchAlt.Text).ToList();
            }

            foreach (var item in contentSearchResults)
            {
                if (!noteResults.Any(x => x.Id == item.Id))
                {
                    noteResults.Add(item);
                }
            }

            //TODO Update
            //UpdateNotesList(noteResults, cbxSortOptions.Text);

            //lstNotes.ItemsSource = noteResults;
            //lblHeading.Content = $"All notes ({noteResults.Count})";
        }

        private void TxtSearchAlt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtSearchAlt.Text.Length == 0)
            {
                //TODO Update
                //UpdateNotesList();
            }
        }

        private void TxtSearchAlt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Enter)
            {
                DoSearch();
            }
        }

        private void BtnSort_Click(object sender, RoutedEventArgs e)
        {
            noteSorter.SortDirection = (bool)(sender as CheckBox).IsChecked
                ? SortDirection.Ascending
                : SortDirection.Descending;
            UpdateNotesList(noteSorter.SorterFunc);
        }

    }
}
