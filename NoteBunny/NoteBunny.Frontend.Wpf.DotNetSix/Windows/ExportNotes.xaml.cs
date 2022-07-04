using NoteBunny.Frontend.Wpf.DotNetSix.Viewmodels;
using NoteBunny.FrontEnd.Wpf.DotNetSix.Helpers;
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
    }
}
