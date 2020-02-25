using NoteBunny.BLL.Interfaces;
using NoteBunny.BLL.Models;
using NoteBunny.DAL.Json.Repositories;
using NoteBunny.FrontEnd.Wpf.Helpers;
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

namespace NoteBunny.FrontEnd.Wpf.Windows
{
    /// <summary>
    /// Interaction logic for ToDoWindow.xaml
    /// </summary>
    public partial class ToDoWindow : Window
    {
        private IRepository<ToDoItem> _repository;
        private Sorter<ToDoItem, object> _itemSorter = new Sorter<ToDoItem, object>();

        private enum ItemSortOptions
        {
            Age,
            Alphabetically,
            Finished
        }

        public ToDoWindow()
        {
            _repository = new JsonRepository<ToDoItem>("todo.json");
            InitializeComponent();

            UpdateToDoList();

            cbxSortOptions.ItemsSource = Enum.GetNames(typeof(ItemSortOptions));
            cbxSortDirection.ItemsSource = Enum.GetNames(typeof(SortDirection));
        }

        private void UpdateToDoList()
        {
            lstTodo.ItemsSource = _repository.GetAll();
        }

        private void Btn_FinishItem_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var parent = btn.TemplatedParent;
            //MessageBox.Show(str);
        }

        private ToDoItem GetSelectedItem() => (ToDoItem)lstTodo.SelectedItem;
        
    }
}
