using iProcessHelper.DBContexts.DBModels;
using iProcessHelper.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iProcessHelper.Models
{
    class ProcessSchemaStartSignalEvent : FilterObject
    {
        public SysSchema Entity { get; set; }
        public EntitySignal EntitySignal { get; set; }


        public List<EntitySignal> EntitySignals { get; set; } = Constants.entitySignals;
        public ObservableCollection<SysSchema> Entities { get; set; } = Constants.entities;

        public ProcessSchemaStartSignalEvent() : base()
        {
            Name = "Сигнал";
        }

        public override bool Filter(ProcessTreeViewElement element)
        {
            if(EntitySignal != null && Entity != null)
                return element.Json.Metadata.Schema.FlowElements.FirstOrDefault(e => e.EntitySignal == EntitySignal.Value && e.EntitySchemanUId == Entity.UId) != null;

            return false;
        }
    }
}
