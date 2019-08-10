using NoteBunny.BLL.Models;
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
    /// Interaction logic for NoteDetails.xaml
    /// </summary>
    public partial class NoteDetails : Window
    {
        public NoteDetails(Note note)
        {
            InitializeComponent();
            this.DataContext = note;
            string tags = String.Join(", ", note.Tags.Select(p => p.Name));
            lblTags.Text = tags;
        }
    }
}
