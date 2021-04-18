using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iProcessHelper.Models
{
    public class EntityColumn
    {
        public Guid UId { get; set; }
        public string Name { get; set; }
        public string Caption { get; set; }
        public DataType DataType { get; set; }
    }
}
