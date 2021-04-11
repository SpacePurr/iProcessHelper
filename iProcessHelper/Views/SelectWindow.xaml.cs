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

namespace iProcessHelper.Views
{
    /// <summary>
    /// Логика взаимодействия для SelectWindow.xaml
    /// </summary>
    public partial class SelectWindow : Window
    {
        public object SelectedItem { get; set; }
        public SelectWindow()
        {
            InitializeComponent();
        }

        public SelectWindow(IEnumerable<object> collection, Window owner)
        {
            this.Owner = owner;
            InitializeComponent();
            CollectionList.ItemsSource = collection;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedItem = CollectionList.SelectedItem;
            DialogResult = true;
        }
    }
}
