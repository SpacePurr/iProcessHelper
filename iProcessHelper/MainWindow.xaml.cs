using iProcessHelper.DBContexts;
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
using System.Data.Entity;
using iProcessHelper.Models;
using iProcessHelper.Helpers;

namespace iProcessHelper
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenLink_Click(object sender, RoutedEventArgs e)
        {
            new CommonHelper().OpenLink(ProcessesTreeView.SelectedItem as ProcessTreeViewElement);
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as MainViewModel;
            new CommonHelper().Open(ProcessesTreeView.SelectedItem as ProcessTreeViewElement, vm.Processes);
        }
    }
}
