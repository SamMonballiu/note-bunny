using NoteBunny.BLL.Interfaces;
using NoteBunny.BLL.Models;
using NoteBunny.FrontEnd.Wpf.Helpers;
using NoteBunny.FrontEnd.Wpf.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
        private TagWindow tagWindow;

        public enum NoteSortingOptions
        {
            CreatedOn,
            Subject,
            Id,
            NumberOfTags
        }

        private NoteSortingOptions selectedSortingOption = NoteSortingOptions.CreatedOn;

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
                UpdateNotesList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            txtSearchAlt.Focus();

            cbxSortOptions.ItemsSource = Enum.GetNames(typeof(NoteSortingOptions));
            cbxSortDirection.ItemsSource = Enum.GetNames(typeof(SortDirection));
            cbxSortOptions.SelectionChanged += CbxSortOptions_SelectionChanged;
            cbxSortDirection.SelectionChanged += CbxSortOptions_SelectionChanged;

            OpenTagsWindow();

            this.Closing += CloseChildWindowsOnClosing;
        }

        private void CloseChildWindowsOnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            tagWindow?.Close();
        }

        private void GenerateRandomNotes(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                string letters = "abcdefghijklmnopqrstuvwxyz";
                StringBuilder sb = new StringBuilder();
                for (int j = 0; j < 12; j++)
                {
                    sb.Append(letters[rnd.Next(letters.Length)]);
                }

                var str = sb.ToString();
                var note = new Note
                {
                    Content = str,
                    Subject = str,
                    TagIds = new List<string>() { "b98fc232-043e-469e-aa27-0e9f69e2cd94" }
                };

                notesRepository.Add(note);
            }
            notesRepository.Save();
        }

        private void SetSortingOptions()
        {
            Func<Note, object> noteSortFunc = null;
            switch (selectedSortingOption)
            {
                case NoteSortingOptions.CreatedOn:
                    noteSortFunc = x => x.CreatedOn;
                    break;
                case NoteSortingOptions.Subject:
                    noteSortFunc = x => x.Subject;
                    break;
                case NoteSortingOptions.Id:
                    noteSortFunc = x => x.Id;
                    break;
                default:
                    noteSortFunc = x => x.TagIds.Count;
                    break;
            }
            noteSorter.SetSorter(noteSortFunc);

            noteSorter.SortDirection = (SortDirection)cbxSortDirection.SelectedIndex;
        }

        #region EVENT HANDLERS 

        private void CbxSortOptions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedSortingOption = (NoteSortingOptions)(sender as ComboBox).SelectedIndex;
            SetSortingOptions();
            UpdateNotesList();
        }
        private void Btn_NewNote_Click(object sender, RoutedEventArgs e)
        {
            new NewNote().ShowDialog();
        }

        private void LstNotes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ShowSelectedNote();
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
        private Note GetSelectedNote() => lstNotes.SelectedItem is null ? null : notesRepository.FindById((string)lstNotes.SelectedValue);

        private void UpdateNotesList(IEnumerable<Note> notes = null)
        {
            if (noteSorter.SorterFunc is null)
            {
                //throw new ArgumentException("Must provide a sorter.");
                return;
            }

            notes = notes ?? notesRepository.GetAll();
            notes = noteSorter.Sort(notes);

            lstNotes.ItemsSource = notes.Select(p => new { p.Subject, p.Id });
            lstNotes.DisplayMemberPath = "Subject";
            lstNotes.SelectedValuePath = "Id";

            if (lstNotes.Items.Count == 1)
            {
                lstNotes.SelectedIndex = 0; 
            }

            UpdateStatusBarText();
        }

        private void UpdateStatusBarText(string additionalText = null)
        {
            statusTxt.Text = $"Total notes: {notesRepository.GetAll().Count()} | Search results: {lstNotes.Items.Count} | Tags: {tags.GetAll().Count()} | ";
            if (additionalText != null)
            {
                statusTxt.Text += additionalText;
            }
        }
        private void ClearSearchResults()
        {
            txtSearchNoteSubjectContent.Text = String.Empty;
            txtSearchTagName.Text = String.Empty;
            UpdateNotesList();
            lblHeading.Content = "All notes";
        }
        private void EditNote()
        {
            var note = GetSelectedNote();
            if (note is null) return;
            ClearSearchResults();
            new NoteDetails(note, Enumerations.NoteState.Edit).ShowDialog();
            UpdateNotesList();
        }

        #endregion

        private void LstNotes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var note = GetSelectedNote();
            if (note is null) return;
            spSelected.DataContext = note;
            txtOnView.Visibility = String.IsNullOrWhiteSpace(note.Content)
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        public void DoSearch()
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

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var noteResults = new List<Note>();
            var searchTerm = txtSearchAlt.Text.ToLower();

            var tagSearchTerms = searchTerm.Split(',').Select(p => p.Trim());
            var allNotes = notesRepository.GetAll();

            var noteSubjects = allNotes.ToDictionary(n => n.Subject.ToLower(), n => n.Id);
            var noteContents = allNotes.Select(n => new { n.Content, n.Id });
            var notesTagIds = allNotes.ToDictionary(n => n.TagIds, n => n.Id);
            var notesById = allNotes.ToDictionary(n => n.Id, n => n);

            var allTags = tags.GetAll();
            var tagNames = allTags.ToDictionary(t => t.Name.ToLower(), p => p.Id);
            var tagsById = allTags.ToDictionary(t => t.Id, t => t);

            var noteSubjectMatchIds = noteSubjects.Where(x => x.Key.Contains(searchTerm)).Select(x => x.Value);
            var noteContentMatchIds = noteContents.Where(x => x.Content.ToLower().Contains(searchTerm)).Select(x => x.Id);

            IEnumerable<string> tagNameContentMatchIds;
            tagNameContentMatchIds = tagNames
                .Where(x => tagSearchTerms.All(term => x.Key.Contains(term)))
                .Select(x => x.Value);

            var tagMatchIds = notesTagIds
                .Where(x => tagNameContentMatchIds.Intersect(x.Key).Count() > 0)
                .Select(x => x.Value);

            var resultIds = noteSubjectMatchIds
                .Union(noteContentMatchIds)
                .Union(tagMatchIds);

            foreach (var id in resultIds)
            {
                noteResults.Add(notesById[id]);
            }

            UpdateNotesList(noteResults);
            UpdateStatusBarText("Search took " + stopwatch.ElapsedMilliseconds.ToString() + "ms");
            stopwatch.Stop();
        }

        private void TxtSearchAlt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtSearchAlt.Text.Length == 0)
            {
                UpdateNotesList();
            }
        }

        private void TxtSearchAlt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Enter)
            {
                try
                {
                    DoSearch();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Something went wrong.");
                }
            }
        }

        private void BtnSort_Click(object sender, RoutedEventArgs e)
        {
            noteSorter.SortDirection = (sender as CheckBox).IsChecked.Value
                ? SortDirection.Ascending
                : SortDirection.Descending;
            UpdateNotesList();
        }

        private void Menu_Tags_Click(object sender, RoutedEventArgs e)
        {
            OpenTagsWindow();
        }

        private void OpenTagsWindow()
        {
            tagWindow = new TagWindow(this);
            tagWindow.Top = 0;
            tagWindow.Left = 0;
            tagWindow.Show();
        }
    }
}
