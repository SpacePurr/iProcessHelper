using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iProcessHelper.Models
{
    class FilterType
    {
        public string Name { get; set; }
        public string Caption { get; set; }

        public override string ToString()
        {
            return Caption;
        }
    }
}
