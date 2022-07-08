using Microsoft.Win32;
using NoteBunny.Frontend.Wpf.DotNetSix.ExtensionMethods;
using NoteBunny.Frontend.Wpf.DotNetSix.Viewmodels;
using NoteBunny.FrontEnd.Wpf.DotNetSix.Helpers;
using System;
using System.Linq;
using System.Windows;

namespace NoteBunny.Frontend.Wpf.DotNetSix.Windows
{
    /// <summary>
    /// Interaction logic for ExportNotes.xaml
    /// </summary>
    public partial class ExportNotes : Window
    {
        private ExportNotesViewModel Viewmodel => (DataContext as ExportNotesViewModel)!;

        public ExportNotes()
        {
            InitializeComponent();
            DataContext = new ExportNotesViewModel(RepositoryFactory.GetJsonRepositories().noteRepository);

            NoteSortOptions.OnSortPropertyChanged += (property) => Viewmodel.OnSetSortProperty?.Execute(property);
            NoteSortOptions.OnSortDirectionChanged += (direction) => Viewmodel.OnSetSortDirection?.Execute(direction);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Viewmodel.ExportCandidates.Any() && MessageBox.Show("Bla", "Bla", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!Viewmodel.ExportCandidates.Any()) {
                return;
            }

            try
            {
                var date = $"{DateTime.Now.ToShortDateString()}_{DateTime.Now.ToShortTimeString()}".ReplaceAlphanumeric('_');

                SaveFileDialog saveFileDialog = new()
                {
                    Filter = "JSON file (*.json)|*.json",
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    FileName = $"export_{date}.json"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    Viewmodel.OnConfirm?.Execute(saveFileDialog.FileName);
                    MessageBox.Show("Notes exported.");
                    Close();
                }
            }
            catch
            {
                MessageBox.Show("Something went wrong.");
            }
        }
    }
}
