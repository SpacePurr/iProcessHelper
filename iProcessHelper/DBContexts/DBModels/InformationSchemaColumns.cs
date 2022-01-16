using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace iProcessHelper.DBContexts.DBModels
{
    class InformationSchemaColumns
    {
        [Key]
        public string column_name { get; set; }
        public string data_type { get; set; }
        public string table_name { get; set; }
    }
}
