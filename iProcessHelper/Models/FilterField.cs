using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iProcessHelper.Models
{
    class FilterField
    {
        public Guid UId { get; set; }

        public string Name { get; set; }

        public int IntValue { get; set; }
        public bool BoolValue { get; set; }
        public decimal DecimalValue { get; set; }
        public string TextValue { get; set; }
        public OperationType OperationType { get; set; }
    }
}
