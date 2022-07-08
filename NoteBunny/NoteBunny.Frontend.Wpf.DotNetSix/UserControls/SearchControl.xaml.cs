using Microsoft.Toolkit.Mvvm.Input;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NoteBunny.Frontend.Wpf.DotNetSix.UserControls
{
    /// <summary>
    /// Interaction logic for SearchControl.xaml
    /// </summary>
    public partial class SearchControl : UserControl
    {
        public string Term
        {
            get { return (string)GetValue(TermProperty); }
            set { 
                SetValue(TermProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for SearchTerm.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TermProperty =
            DependencyProperty.Register(nameof(Term), typeof(string), typeof(SearchControl), new PropertyMetadata(string.Empty));

        public RelayCommand OnSearch
        {
            get { return (RelayCommand)GetValue(OnSearchProperty); }
            set { SetValue(OnSearchProperty, value); }
        }

        public static readonly DependencyProperty OnSearchProperty =
            DependencyProperty.Register(nameof(OnSearch), typeof(RelayCommand), typeof(SearchControl), new PropertyMetadata(null));

        public SearchControl()
        {
            InitializeComponent();
            txtSearchAlt.KeyDown += TxtSearchAlt_KeyDown;
        }

        private void TxtSearchAlt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Enter)
            {
                try
                {
                    OnSearch?.Execute(null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Something went wrong.");
                }
            }
        }
    }
}
