using iProcessHelper.Models;
using iProcessHelper.MVVM;
using iProcessHelper.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace iProcessHelper.Helpers
{
    class CommonHelper
    {
        public void OpenLink(ProcessTreeViewElement obj)
        {
            if(string.IsNullOrEmpty(Constants.SiteUrl))
            {
                MessageBox.Show("Заполните системную настройку SiteUrl (пр. http://127.0.0.1:9009)");
                return;
            }

            var link = $"{Constants.SiteUrl}/0/Nui/ViewModule.aspx?vm=SchemaDesigner#process/{obj.SysSchema.UId}";
            System.Diagnostics.Process.Start(link);
        }

        public void Open(ProcessTreeViewElement obj, ObservableCollection<ProcessTreeViewElement> processes)
        {
            var hWindow = new HierarchicalProcessWindow(obj, processes);
            hWindow.ShowDialog();
        }
    }
}
