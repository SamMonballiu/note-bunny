using NoteBunny.FrontEnd.Wpf.DotNetSix.Viewmodels;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace NoteBunny.Frontend.Wpf.DotNetSix.UserControls
{
    /// <summary>
    /// Interaction logic for NotesList.xaml
    /// </summary>
    public partial class NotesList : UserControl
    {
        public IList<NoteViewModel> Notes
        {
            get { return (IList<NoteViewModel>)GetValue(NoteViewModelsProperty); }
            set { SetValue(NoteViewModelsProperty, value); }
        }

        public event Action<string>? OnSelectedNoteChanged = null;

        // Using a DependencyProperty as the backing store for NoteViewModels.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NoteViewModelsProperty =
            DependencyProperty.Register(nameof(Notes), typeof(IList<NoteViewModel>), typeof(NotesList), new PropertyMetadata(new List<NoteViewModel>()));

        public bool HasNotes => Notes.Count > 0;
        public int NoteCount => Notes.Count;

        public NotesList()
        {
            InitializeComponent();
        }

        private void LstNotes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = (sender as ListBox)!.SelectedItem as NoteViewModel;
            OnSelectedNoteChanged?.Invoke(selectedItem?.Id);
        }
    }
}
