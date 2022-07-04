using NoteBunny.BLL.Enums;
using NoteBunny.BLL.Models;
using NoteBunny.Frontend.Wpf.DotNetSix;
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

namespace NoteBunny.FrontEnd.Wpf.DotNetSix.Windows
{
    /// <summary>
    /// Interaction logic for TagWindow.xaml
    /// </summary>
    public partial class TagWindow : Window
    {
        private Sorter<TagListEntry, object> tagSorter = new Sorter<TagListEntry, object>(x => x.Name);
        private TagSortOptions selectedSortOption = TagSortOptions.Name;
        private IEnumerable<TagListEntry> tags;
        private MainWindow _parent;

        private enum TagSortOptions
        {
            Name,
            Uses,
        } 

        public TagWindow(MainWindow parent)
        {
            InitializeComponent();
            _parent = parent;
            var repos = RepositoryFactory.GetJsonRepositories();
            var mapper = new TagMapper();
            tags = mapper.Map(repos.tagRepository.GetAll().AsEnumerable(), repos.noteRepository.GetAll().AsEnumerable());
            UpdateTagsList();

            tagSorter.SetSortFunc(x => x.Name);
            tagSorter.SortDirection = SortDirection.Ascending;

            cbxSortOptions.ItemsSource = Enum.GetNames(typeof(TagSortOptions));
            cbxSortDirection.ItemsSource = Enum.GetNames(typeof(SortDirection));

            cbxSortDirection.SelectionChanged += SortingOptions_SelectionChanged;
            cbxSortOptions.SelectionChanged += SortingOptions_SelectionChanged;
        }

        private void SortingOptions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetSortingOptions();
            UpdateTagsList();
        }

        private void SetSortingOptions()
        {
            selectedSortOption = (TagSortOptions)cbxSortOptions.SelectedIndex;
            Func<TagListEntry, object> tagSortFunc = null;
            switch (selectedSortOption)
            {
                case TagSortOptions.Name:
                    tagSortFunc = x => x.Name;
                    break;
                case TagSortOptions.Uses:
                    tagSortFunc = x => x.Uses;
                    break;
                default:
                    tagSortFunc = x => x.Name;
                    break;
            }
            tagSorter.SetSortFunc(tagSortFunc);

            tagSorter.SortDirection = (SortDirection)cbxSortDirection.SelectedIndex;

        }

        private void UpdateTagsList()
        {
            tags = tagSorter.Sort(tags);
            lstTags.ItemsSource = tags;
        }

        private void LstTags_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedTag = (TagListEntry)lstTags.SelectedItem;
            //_parent.txtSearchAlt.Text = selectedTag.Name;
            //_parent.DoSearch();
            _parent.Focus();
        }
    }

    internal class TagListEntry
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Uses { get; set; }
        public override string ToString() => $"{Name} ({Uses})";

    }

    internal class TagMapper
    {
        internal IEnumerable<TagListEntry> Map(IEnumerable<Tag> tags, IEnumerable<Note> notes)
        {
            foreach (var tag in tags)
            {
                var relevantNotesCount = notes.Count(x => x.TagIds.Contains(tag.Id));
                yield return new TagListEntry
                {
                    Name = tag.Name,
                    Uses = relevantNotesCount,
                    Id = tag.Id
                };
            }
        }
    }
}
