using SortProperty = NoteBunny.BLL.Enums.NoteSortOptions;
using NoteBunny.BLL.Enums;
using System;
using System.Windows.Controls;

namespace NoteBunny.Frontend.Wpf.DotNetSix.UserControls
{
    /// <summary>
    /// Interaction logic for NoteSortOptions.xaml
    /// </summary>
    public partial class NoteSortOptions : UserControl
    {
        public event Action<SortProperty>? OnSortPropertyChanged = null;
        public event Action<SortDirection>? OnSortDirectionChanged = null;

        public NoteSortOptions()
        {
            InitializeComponent();
            cbxSortOptions.SelectionChanged += (_, _) => OnSortPropertyChanged?.Invoke((SortProperty)cbxSortOptions.SelectedItem);
            cbxSortDirection.SelectionChanged += (_, _) => OnSortDirectionChanged?.Invoke((SortDirection)cbxSortDirection.SelectedItem);
        }
    }
}
