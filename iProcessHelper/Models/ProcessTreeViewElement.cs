using iProcessHelper.DBContexts.DBModels;
using iProcessHelper.JsonProcessModels.Short;
using iProcessHelper.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iProcessHelper.Models
{
    public class ProcessTreeViewElement : NotifyPropertyChanged
    {
        private bool isVisible;

        public SysSchema SysSchema { get; set; }

        public bool IsVisible { get => isVisible; set { isVisible = value; OnPropertyChanged(); } }

        public ObservableCollection<ProcessTreeViewElement> Items { get; set; }
        public ProcessModelShort Json { get; set; }


        public ProcessTreeViewElement()
        {
            IsVisible = true;
            Items = new ObservableCollection<ProcessTreeViewElement>();
        }
    }
}
