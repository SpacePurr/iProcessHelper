using iProcessHelper.JsonProcessModels.Short;
using iProcessHelper.ProcessJsonModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iProcessHelper.DBContexts.DBModels
{
    [Table("SysSchema")]
    public class SysSchema
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ManagerName { get; set; }
        public Guid UId { get; set; }
        public string Name { get; set; }
        public string Caption { get; set; }
        public byte[] MetaData { get; set; }
        public Guid? ParentId { get; set; }
    }
}
