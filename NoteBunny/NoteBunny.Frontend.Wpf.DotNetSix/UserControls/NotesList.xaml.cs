using NoteBunny.FrontEnd.Wpf.DotNetSix.Viewmodels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace NoteBunny.Frontend.Wpf.DotNetSix.UserControls
{
    /// <summary>
    /// Interaction logic for NotesList.xaml
    /// </summary>
    public partial class NotesList : UserControl
    {
        public ObservableCollection<NoteViewModel> Notes
        {
            get { return (ObservableCollection<NoteViewModel>)GetValue(NoteViewModelsProperty); }
            set { SetValue(NoteViewModelsProperty, value); }
        }


        public event Action<List<string>>? OnSelectedNoteChanged = null;

        // Using a DependencyProperty as the backing store for NoteViewModels.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NoteViewModelsProperty =
            DependencyProperty.Register(nameof(Notes), typeof(ObservableCollection<NoteViewModel>), typeof(NotesList), new PropertyMetadata(new ObservableCollection<NoteViewModel>()));

        public IList<NoteViewModel> SelectedNotes => lstNotes.SelectedItems.OfType<NoteViewModel>().ToList();

        public SelectionMode SelectionMode
        {
            get { return (SelectionMode)GetValue(SelectionModeProperty); }
            set { SetValue(SelectionModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectionModeProperty =
            DependencyProperty.Register(nameof(SelectionMode), typeof(SelectionMode), typeof(NotesList), new PropertyMetadata(SelectionMode.Single));

        public NotesList()
        {
            InitializeComponent();
        }

        public void ClearSelection()
        {
            lstNotes.SelectedIndex = -1;
        }

        private void LstNotes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItems = (sender as ListBox)!.SelectedItems.OfType<NoteViewModel>();
            OnSelectedNoteChanged?.Invoke(selectedItems.Select(x => x.Id).ToList());
        }
    }
}
