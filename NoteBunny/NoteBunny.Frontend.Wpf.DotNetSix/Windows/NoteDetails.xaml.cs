using NoteBunny.BLL.Interfaces;
using NoteBunny.BLL.Models;
using NoteBunny.FrontEnd.Wpf.DotNetSix.Enumerations;
using NoteBunny.FrontEnd.Wpf.DotNetSix.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace NoteBunny.FrontEnd.Wpf.DotNetSix.Windows
{
    /// <summary>
    /// Interaction logic for NoteDetails.xaml
    /// </summary>
    public partial class NoteDetails : Window
    {
        private NoteState _state;
        private Note _note;
        private INoteRepository noteRepository;
        private ITagRepository tagRepository;
        public NoteDetails(Note note, NoteState state = NoteState.New)
        {
            InitializeComponent();
            _note = note;
            this.DataContext = _note;
            string tags = String.Join(", ", note.Tags.Select(p => p.Name));
            lblTags.Text = tags;
            _state = state;
            btnDismiss.Content = _state == NoteState.Edit ? "Save" : "OK";
            txtContent.IsEnabled = _state == NoteState.Edit;
            var repos = RepositoryFactory.GetJsonRepositories();
            noteRepository = repos.noteRepository;
            tagRepository = repos.tagRepository;

            txtOnEdit.Visibility = _state == NoteState.Edit ? Visibility.Visible : Visibility.Collapsed;
            txtOnEdit.Focusable = _state == NoteState.Edit ? true : false;
            txtOnView.Visibility = _state == NoteState.New ? Visibility.Visible : Visibility.Collapsed;
            txtOnView.Focusable = _state == NoteState.New ? true : false;
            txtTagsEdit.Text = String.Join(", ", tagRepository.GetTagsFromIds(note.TagIds.ToList()).Select(p => p.Name));

            this.Title = $"{_state}: {note.Subject} ({note.CreatedOn.ToShortDateString()})";
        }

        private void BtnDismiss_Click(object sender, RoutedEventArgs e)
        {
            switch (this._state)
            {
                case NoteState.Edit:
                    tagRepository.AddTagsFromString(txtTagsEdit.Text);
                    _note.TagIds = new List<string>();
                    _note.Tags = null;
                    _note.TagIds = tagRepository.GetTagsFromString(txtTagsEdit.Text).Select(p => p.Id).ToList();

                    noteRepository.Update(_note);
                    noteRepository.Save();
                    MessageBox.Show("OK!");
                    break;
                default:
                    break;
            }
            this.Close();
        }
    }
}
