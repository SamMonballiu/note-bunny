using NoteBunny.FrontEnd.Wpf.DotNetSix.Context;
using System.Linq;
using System.Windows.Controls;

namespace NoteBunny.Frontend.Wpf.DotNetSix.UserControls
{
    /// <summary>
    /// Interaction logic for Tags.xaml
    /// </summary>
    public partial class Tags : UserControl
    {
        public Tags()
        {
            InitializeComponent();
            SelectedNoteContext.OnSelectedNoteChanged += note =>
            {
                DataContext = note?.Tags.Select(x => x.Name) ?? Enumerable.Empty<string>();
            };
        }
    }
}
