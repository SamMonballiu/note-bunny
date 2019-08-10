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
        private NoteRepository notes;
        private TagRepository tags;

        private ObservableCollection<Note> notesCollection = new ObservableCollection<Note>();

        public MainWindow()
        {
            InitializeComponent();
            var repos = XmlHelpers.GetXmlRepositories("tags.xml", "notes.xml");
            notes = repos.noteRepository;
            tags = repos.tagRepository;

            UpdateNotesList();
        }

        private void Btn_NewNote_Click(object sender, RoutedEventArgs e)
        {
            new NewNote().ShowDialog();
            UpdateNotesList();
        }

        private void LstNotes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var senderLb = sender as ListBox;
            Note selectedItem = (Note)senderLb.SelectedItem;
            new NoteDetails(selectedItem).ShowDialog();
            UpdateNotesList();
        }

        private void UpdateNotesList()
        {
            notesCollection.Clear();
            foreach (var item in notes.GetAll())
            {
                notesCollection.Add(item);
            }
            lstNotes.ItemsSource = null;
            lstNotes.ItemsSource = notesCollection;
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            UpdateNotesList();
        }
    }
}
