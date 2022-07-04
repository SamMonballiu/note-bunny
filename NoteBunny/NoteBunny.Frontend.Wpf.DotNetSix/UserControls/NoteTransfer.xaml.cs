using NoteBunny.Frontend.Wpf.DotNetSix.ExtensionMethods;
using NoteBunny.FrontEnd.Wpf.DotNetSix.Viewmodels;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace NoteBunny.Frontend.Wpf.DotNetSix.UserControls
{
    /// <summary>
    /// Interaction logic for NoteTransfer.xaml
    /// </summary>
    public partial class NoteTransfer : UserControl
    {
        public ObservableCollection<NoteViewModel> Left
        {
            get => (ObservableCollection<NoteViewModel>)GetValue(LeftProperty);
            set
            {
                SetValue(LeftProperty, value);
            }
        }

        public static readonly DependencyProperty LeftProperty =
            DependencyProperty.Register(
                nameof(Left),
                typeof(ObservableCollection<NoteViewModel>),
                typeof(NoteTransfer),
                new PropertyMetadata(new ObservableCollection<NoteViewModel>()
                    )
                );

        public ObservableCollection<NoteViewModel> Right
        {
            get { return (ObservableCollection<NoteViewModel>)GetValue(RightProperty); }
            set { SetValue(RightProperty, value); }
        }

        public static readonly DependencyProperty RightProperty =
            DependencyProperty.Register(nameof(Right), typeof(ObservableCollection<NoteViewModel>), typeof(NoteTransfer), new PropertyMetadata(new ObservableCollection<NoteViewModel>()));

        public NoteTransfer()
        {
            InitializeComponent();
            ToRight.Click += (_, _) =>
            {
                var selected = LeftList.SelectedNotes;
                if (selected is null)
                {
                    return;
                }

                Right.AddRange(selected);
                Left.RemoveRange(selected);
            };

            ToLeft.Click += (_, _) =>
            {
                var selected = RightList.SelectedNotes;
                if (selected is null)
                {
                    return;
                }

                Left.AddRange(selected);
                Right.RemoveRange(selected);

            };
        }

        private void ToRight_Click(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}