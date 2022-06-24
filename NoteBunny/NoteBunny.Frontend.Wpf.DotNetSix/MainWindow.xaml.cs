using NoteBunny.FrontEnd.Wpf.DotNetSix.Context;
using NoteBunny.FrontEnd.Wpf.DotNetSix.Helpers;
using NoteBunny.FrontEnd.Wpf.DotNetSix.Viewmodels;
using NoteBunny.FrontEnd.Wpf.DotNetSix.Windows;
using NoteBunny.FrontEnd.Wpf.DotNetSix.Enumerations;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NoteBunny.Frontend.Wpf.DotNetSix
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel Viewmodel => (DataContext as MainViewModel)!;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel(RepositoryFactory.GetJsonRepositories().noteRepository);

            txtSearchAlt.Focus();

            cbxSortOptions.ItemsSource = Enum.GetNames(typeof(NoteSortOptions));
            cbxSortDirection.ItemsSource = Enum.GetNames(typeof(SortDirection));
            cbxSortDirection.SelectedIndex = (int)SortDirection.Descending;
            cbxSortOptions.SelectionChanged += CbxSortOptions_SelectionChanged;
            cbxSortDirection.SelectionChanged += CbxSortDirection_SelectionChanged; ;

            OpenTagsWindow();

            SelectedNoteContext.OnSelectedNoteChanged += (note) =>
            {
                spSelected.DataContext = note;
                spSelected.Visibility = note is null
                    ? Visibility.Hidden
                    : Visibility.Visible;
            };
        }

        private void CbxSortDirection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedSortDirection = (SortDirection)(sender as ComboBox)!.SelectedIndex;
            Viewmodel.OnSetSortDirection?.Execute(selectedSortDirection);
        }

        #region EVENT HANDLERS 

        private void CbxSortOptions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedSortingOption = (NoteSortOptions)(sender as ComboBox)!.SelectedIndex;
            Viewmodel.OnSetSortProperty?.Execute(selectedSortingOption);
        }

        private void Btn_NewNote_Click(object sender, RoutedEventArgs e)
        {
            var result = new NewNote().ShowDialog();
            if (result.HasValue && result is true) {
                Viewmodel.OnGetData?.Execute(null);
            }
        }
 
        private void Btn_EditNote_Click(object sender, RoutedEventArgs e)
        {
            var note = SelectedNoteContext.SelectedNote;
            if (note is null) return;
            var result = new NewNote(note).ShowDialog();
            if (result.HasValue && result is true)
            {
                Viewmodel.OnGetData?.Execute(null);
            }
        }
        
        #endregion

        private void UpdateStatusBarText(string additionalText = null)
        {
            //statusTxt.Text = $"Total notes: {notesRepository.GetAll().Count()} | Search results: {lstNotes.Items.Count} | Tags: {tags.GetAll().Count()} | ";
            //if (additionalText != null)
            //{
            //    statusTxt.Text += additionalText;
            //}
        }

        private void LstNotes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = (sender as ListBox)!.SelectedItem as NoteViewModel;
            Viewmodel.OnSetSelectedNote.Execute(selectedItem?.Id);
        }

        private void TxtSearchAlt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Enter)
            {
                try
                {
                    Viewmodel.OnSearch?.Execute(null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Something went wrong.");
                }
            }
        }

        private void Menu_Tags_Click(object sender, RoutedEventArgs e)
        {
            OpenTagsWindow();
        }

        private void OpenTagsWindow()
        {
            return;
            var tags = new TagWindow(this);
            tags.Top = 0;
            tags.Left = 0;
            tags.Show();
        }
    }
}
