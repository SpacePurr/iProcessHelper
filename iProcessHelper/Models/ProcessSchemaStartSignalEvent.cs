using iProcessHelper.DBContexts.DBModels;
using iProcessHelper.Helpers;
using iProcessHelper.MVVM;
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
        public ObservableCollection<FilterField> FilterFields { get; set; }

        public Command AddFilterField { get; set; }

        public ProcessSchemaStartSignalEvent() : base()
        {
            FilterFields = new ObservableCollection<FilterField>();
            Name = "Сигнал";
            AddFilterField = new Command(AddFilterFieldMethod);
        }

        public override bool Filter(ProcessTreeViewElement element)
        {
            if(EntitySignal != null && Entity != null)
                return element.Json.Metadata.Schema.FlowElements.FirstOrDefault(e => e.EntitySignal == EntitySignal.Value && e.EntitySchemanUId == Entity.UId) != null;

            return false;
        }

        public void AddFilterFieldMethod(object obj)
        {
            FilterFields.Add(new FilterField());
        }
    }
}
