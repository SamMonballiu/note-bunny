using NoteBunny.BLL.Models;
using NoteBunny.BLL.Repositories;
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
        private NoteRepository noteRepository;
        private TagRepository tagRepository;

        public NewNote()
        {
            InitializeComponent();
            var repos = XmlHelpers.GetXmlRepositories("tags.xml", "notes.xml");
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
