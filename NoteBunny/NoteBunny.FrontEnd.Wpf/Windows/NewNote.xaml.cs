using NoteBunny.BLL.Interfaces;
using NoteBunny.BLL.Models;
using NoteBunny.BLL.Repositories;
using NoteBunny.DAL.Json.Models;
using NoteBunny.DAL.Xml.Helpers;
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

namespace NoteBunny.FrontEnd.Wpf.Windows
{
    /// <summary>
    /// Interaction logic for NewNote.xaml
    /// </summary>
    public partial class NewNote : Window
    {
        private INoteRepository noteRepository;
        private ITagRepository tagRepository;

        public NewNote()
        {
            InitializeComponent();
            var repos = JsonHelpers.GetJsonRepositories("tags.json", "notes.json");
            noteRepository = repos.noteRepository;
            tagRepository = repos.tagRepository;
        }

        private void BtnSaveNewNote_Click(object sender, RoutedEventArgs e)
        {
            var subject = txtSubject.Text;
            var content = txtContent.Text;
            tagRepository.AddTagsFromString(txtTags.Text);
            var tags = tagRepository.GetTagIdsFromNames(txtTags.Text);

            var note = new Note()
            {
                Subject = subject,
                Content = content,
                TagIds = tags
            };

            noteRepository.Add(note);
            noteRepository.Save();
            MessageBox.Show("OK!");
            this.Close();
        }
    }
}
