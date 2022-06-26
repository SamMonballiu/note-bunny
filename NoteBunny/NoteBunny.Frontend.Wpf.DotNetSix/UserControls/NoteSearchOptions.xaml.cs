using Microsoft.Toolkit.Mvvm.ComponentModel;
using NoteBunny.BLL.Enums;
using System.Windows;
using System.Windows.Controls;

namespace NoteBunny.Frontend.Wpf.DotNetSix.UserControls
{
    /// <summary>
    /// Interaction logic for NoteSearchOptions.xaml
    /// </summary>
    public partial class NoteSearchOptions : UserControl
    {
        public NoteFilterType FilterOn
        {
            get
            {
                return (NoteFilterType)GetValue(FilterOnProperty);
            }
            set { 
                SetValue(FilterOnProperty, value); 
            }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilterOnProperty =
            DependencyProperty.Register(nameof(FilterOn), typeof(NoteFilterType), typeof(NoteSearchOptions), new PropertyMetadata(NoteFilterType.All));


        public MatchType Match
        {
            get { return (MatchType)GetValue(MatchProperty); }
            set { SetValue(MatchProperty, value); }
        }

        public bool IsMatchEnabled
        {
            get { return (bool)GetValue(IsMatchEnabledProperty); }
            set { SetValue(IsMatchEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Match.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MatchProperty =
            DependencyProperty.Register(nameof(Match), typeof(MatchType), typeof(NoteSearchOptions), new PropertyMetadata(MatchType.Any));

        // Using a DependencyProperty as the backing store for IsMatchEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsMatchEnabledProperty =
            DependencyProperty.Register(nameof(IsMatchEnabled), typeof(bool), typeof(NoteSearchOptions), new PropertyMetadata(false));

        public NoteSearchOptions()
        {
            InitializeComponent();
        }
    }
}
