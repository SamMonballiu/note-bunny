using Microsoft.Win32;
using NoteBunny.Frontend.Wpf.DotNetSix.Viewmodels;
using NoteBunny.FrontEnd.Wpf.DotNetSix.Helpers;
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

namespace NoteBunny.Frontend.Wpf.DotNetSix.Windows
{
    /// <summary>
    /// Interaction logic for ImportNotes.xaml
    /// </summary>
    public partial class ImportNotes : Window
    {
        private ImportNotesViewModel Viewmodel => (DataContext as ImportNotesViewModel)!;

        public ImportNotes()
        {
            InitializeComponent();
            var (tagRepository, noteRepository) = RepositoryFactory.GetJsonRepositories();
            DataContext = new ImportNotesViewModel(noteRepository, tagRepository);

            Viewmodel.OnNotesImported += Viewmodel_OnNotesImported;
        }

        private void Viewmodel_OnNotesImported(NoteImportResult result)
        {
            var message = result.Success switch
            {
                true => result.NotImported.Any()
                    ? $"{result.NotImported.Count()} notes were not imported because they already exist:\n\n {string.Join("\n", result.NotImported.Select(x => $"- {x.Subject}"))}"
                    : $"{result.Imported.Count()} notes successfully imported.",
                false => "Something went wrong."
            };

            MessageBox.Show(message, string.Empty, MessageBoxButton.OK, result.Success ? MessageBoxImage.Information : MessageBoxImage.Error);
            if (result.Success)
            {
                Close();
                // TODO Provoke reload of list on Main
            }
        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new()
            {
                Multiselect = false,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    Viewmodel.LoadNotesCommand?.Execute(openFileDialog.FileName);
                }
                catch
                {
                    MessageBox.Show("Something went wrong.");
                }
            }
        }
    }
}
