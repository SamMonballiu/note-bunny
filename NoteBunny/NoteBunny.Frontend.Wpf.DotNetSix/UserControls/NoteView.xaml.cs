using NoteBunny.BLL.Models;
using System.Windows;
using System.Windows.Controls;

namespace NoteBunny.Frontend.Wpf.DotNetSix.UserControls
{
    /// <summary>
    /// Interaction logic for NoteView.xaml
    /// </summary>
    public partial class NoteView : UserControl
    {
        public Note? Note
        {
            get => (Note)GetValue(NoteProperty);
            set { SetValue(NoteProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Note.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NoteProperty =
            DependencyProperty.Register(nameof(Note), typeof(Note), typeof(NoteView), new PropertyMetadata(null));

        public bool ShowMarkdown
        {
            get { return (bool)GetValue(ShowMarkdownProperty); }
            set { SetValue(ShowMarkdownProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowMarkdown.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowMarkdownProperty =
            DependencyProperty.Register(nameof(ShowMarkdown), typeof(bool), typeof(NoteView), new PropertyMetadata(true));


        public NoteView()
        {
            InitializeComponent();
        }

        private void OpenHyperlink(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
            {
                UseShellExecute = true,
                FileName = e.Parameter.ToString()
            });
        }
    }
}
