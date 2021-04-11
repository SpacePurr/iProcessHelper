using iProcessHelper.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iProcessHelper.Models
{
    abstract class FilterObject : NotifyPropertyChanged
    {
        public string Name { get; protected set; }

        public abstract bool Filter(ProcessTreeViewElement element);
    }
}
