using iProcessHelper.DBContexts.DBModels;
using iProcessHelper.Helpers;
using iProcessHelper.JsonProcessModels.Short;
using iProcessHelper.Models;
using iProcessHelper.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iProcessHelper.ViewModels
{
    class HierarchicalProcessViewModel : NotifyPropertyChanged
    {
        public ObservableCollection<ProcessTreeViewElement> Processes { get; set; }
        public HierarchicalProcessViewModel(ProcessTreeViewElement element, ObservableCollection<ProcessTreeViewElement> processes)
        {
            Processes = new ObservableCollection<ProcessTreeViewElement>();
            var helper = new ProcessMetadataParser();

            foreach (var item in processes)
            {
                item.Json = helper.Deserialize<ProcessModelShort>(item.SysSchema.MetaData);
            }

            this.SetTree(element, processes);
        }

        private void SetTree(ProcessTreeViewElement element, ObservableCollection<ProcessTreeViewElement> processes)
        {
            var helper = new ProcessMetadataParser();
            if (element.SysSchema != null)
            {
                var parents = helper.GetMainParents(processes, element);

                foreach (var parent in parents)
                {
                    var tree = helper.GetChildrenTree(processes, parent);

                    var ct = new ProcessTreeViewElement();
                    ct.Items.Add(tree);

                    Processes.Add(tree);
                }
            }
        }

    }
}
