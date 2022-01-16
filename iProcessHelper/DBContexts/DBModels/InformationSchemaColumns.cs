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
        public string COLUMN_NAME { get; set; }
        public string DATA_TYPE { get; set; }
        public string TABLE_NAME { get; set; }
    }
}
