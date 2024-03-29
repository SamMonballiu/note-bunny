﻿using Markdig;
using Markdig.Wpf;
using NoteBunny.BLL.Enums;
using NoteBunny.BLL.Models;
using NoteBunny.Frontend.Wpf.DotNetSix.Windows;
using NoteBunny.FrontEnd.Wpf.DotNetSix.Context;
using NoteBunny.FrontEnd.Wpf.DotNetSix.Helpers;
using NoteBunny.FrontEnd.Wpf.DotNetSix.Viewmodels;
using NoteBunny.FrontEnd.Wpf.DotNetSix.Windows;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;

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

            NotesList.OnSelectedNoteChanged += (selected) => Viewmodel.OnSetSelectedNote?.Execute(selected.SingleOrDefault());
            NoteSortOptions.OnSortPropertyChanged += (property) => Viewmodel.OnSetSortProperty?.Execute(property);
            NoteSortOptions.OnSortDirectionChanged += (direction) => Viewmodel.OnSetSortDirection?.Execute(direction);

            OpenTagsWindow();

            spSelected.Visibility = Visibility.Hidden;

            SelectedNoteContext.OnSelectedNoteChanged += (note) =>
            {
                spSelected.DataContext = note;
                spSelected.Visibility = note is null
                    ? Visibility.Hidden
                    : Visibility.Visible;
                NoteView.Note = note;
            };

            Search.FocusTextBox();
        }


        #region EVENT HANDLERS 

        private void Btn_NewNote_Click(object sender, RoutedEventArgs e)
        {
            ShowNoteWindow();
        }

        private void Btn_EditNote_Click(object sender, RoutedEventArgs e)
        {
            var note = SelectedNoteContext.SelectedNote;
            if (note is null) return;
            ShowNoteWindow(note);
        }
        
        private void ShowNoteWindow(Note? note = null)
        {
            var result = new NewNote(note).ShowDialog();
            if (result.HasValue && result is true)
            {
                Viewmodel.OnGetData?.Execute(null);
                NotesList.ClearSelection();
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

        private void menu_Export_Click(object sender, RoutedEventArgs e)
        {
            new ExportNotes().ShowDialog();
        }

        private void menu_Import_Click(object sender, RoutedEventArgs e)
        {
            new ImportNotes().ShowDialog();
        }

        private void OpenHyperlink(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            try
            {
                Process.Start(e.Parameter.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "That didn't quite work...");
            }
        }
    }
}
