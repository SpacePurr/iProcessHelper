using iProcessHelper.DBContexts.DBModels;
using iProcessHelper.Helpers;
using iProcessHelper.Models;
using iProcessHelper.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace iProcessHelper.Views
{
    /// <summary>
    /// Логика взаимодействия для HierarchicalProcessWindow.xaml
    /// </summary>
    public partial class HierarchicalProcessWindow : Window
    {
        public HierarchicalProcessWindow()
        {
            InitializeComponent();
        }

        public HierarchicalProcessWindow(ProcessTreeViewElement element, ObservableCollection<ProcessTreeViewElement> processes)
        {
            InitializeComponent();
            this.DataContext = new HierarchicalProcessViewModel(element, processes);
        }

        private void OpenLink_Click(object sender, RoutedEventArgs e)
        {
            new CommonHelper().OpenLink(ProcessesTreeView.SelectedItem as ProcessTreeViewElement);
        }
    }
}
