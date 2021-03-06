using iProcessHelper.DBContexts;
using iProcessHelper.DBContexts.DBModels;
using iProcessHelper.Helpers;
using iProcessHelper.JsonModels.JsonProcessModel;
using iProcessHelper.JsonModels.JsonProcessModels;
using iProcessHelper.MVVM;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iProcessHelper.Models
{
    class ProcessSchemaStartSignalEvent : FilterObject
    {
        private SysSchema entity;
        public SysSchema Entity { 
            get => entity; 
            set 
            { 
                entity = value;

                var context = new InfromationSchemaColumnsContext();

                var columns = context.AllInformationSchemaColumns.Where(x => x.table_name == Entity.Name);

                foreach (var column in columns)
                {
                    var entityColumns = new EntityColumn
                    {
                        Name = column.column_name,
                        DataType = ColumnType.Parse(column.data_type)
                    };
                    Columns.Add(entityColumns);
                }
            } 
        }

        public ObservableCollection<SysSchema> Entities { get; set; } = Constants.Entities;

        public EntitySignal EntitySignal { get; set; }
        public List<EntitySignal> EntitySignals { get; set; } = Constants.EntitySignals;

        public ObservableCollection<FilterField> FilterFields { get; set; }
        public ObservableCollection<TriggerField> TriggerFields { get; set; }

        public ObservableCollection<EntityColumn> Columns { get; set; }

        public Command AddFilterField { get; set; }
        public Command AddTriggerField { get; set; }

        public ProcessSchemaStartSignalEvent() : base()
        {
            Name = "Сигнал";

            FilterFields = new ObservableCollection<FilterField>();
            TriggerFields = new ObservableCollection<TriggerField>();
            AddFilterField = new Command(AddFilterFieldMethod);
            AddTriggerField = new Command(AddTriggerFieldMethod);

            Columns = new ObservableCollection<EntityColumn>();
        }

        public override bool Filter(ProcessTreeViewElement element)
        {
            return this.IsHasSignalByObject(element) 
                && this.IsHasSignalByEntitySignal(element)
                && this.IsHasSignalByTriggerFields(element)
                && this.IsHasSignalByFilterFields(element);
        }

        private bool IsHasSignalByObject(ProcessTreeViewElement element)
        {
            if (Entity != null)
                return element.Json.Metadata.Schema.FlowElements.FirstOrDefault(e => e.EntitySignal == EntitySignal.Value && e.EntitySchemanUId == Entity.UId) != null;

            return true;
        }

        private bool IsHasSignalByEntitySignal(ProcessTreeViewElement element)
        {
            if (EntitySignal != null)
                return element.Json.Metadata.Schema.FlowElements.FirstOrDefault(e => e.EntitySignal == EntitySignal.Value && e.EntitySchemanUId == Entity.UId) != null;

            return true;
        }

        private bool IsHasSignalByTriggerFields(ProcessTreeViewElement element)
        {
            if (TriggerFields.Any())
            {
                var signals = element.Json.Metadata.Schema.FlowElements.Where(fe => fe.TypeName == "Terrasoft.Core.Process.ProcessSchemaStartSignalEvent");
                var columnCount = TriggerFields.Count;
                var searchCount = 0;
                foreach (var signal in signals)
                {
                    foreach (var triggerField in TriggerFields)
                        if (signal.NewEntityChangedColumns.Contains(triggerField.Column.UId.ToString()))
                            searchCount++;

                    if (searchCount >= columnCount)
                        return true;
                }

                return false;
            }

            return true;
        }

        private bool IsHasSignalByFilterFields(ProcessTreeViewElement element)
        {
            if (FilterFields.Any())
            {
                var signals = element.Json.Metadata.Schema.FlowElements.Where(fe => fe.TypeName == "Terrasoft.Core.Process.ProcessSchemaStartSignalEvent");
                var columnCount = FilterFields.Count;
                var searchCount = 0;

                foreach (var signal in signals)
                {
                    var entityFilters = signal.EntityFilters;
                    var json = MetadataParser.Deserialize<EntityFilter>(entityFilters);

                    var j1 = JObject.Parse(json.DataSourceFilters);

                    foreach (var item in j1["items"])
                    {
                        foreach (var el in item)
                        {
                            var comparisonType = el["comparisonType"];
                            var columnPath = el["leftExpression"]["columnPath"];

                            JToken value;

                            if(el["rightExpression"] != null)
                            {
                                var rightExpression = el["rightExpression"];

                                //var rightExpressionArray = JArray.Parse(el["rightExpression"].ToString());
                                var parameter = rightExpression["parameter"];

                                if (parameter["dataValueType"].ToString() == "10")
                                    columnPath += "Id";

                                var filterField = FilterFields.FirstOrDefault(ff => ff.Column.Name == columnPath.ToString());
                                if (filterField != null && filterField.OperationType.Value == int.Parse(comparisonType.ToString()) && filterField.IsValid(parameter["value"].ToString()))
                                    searchCount++;
                            }
                            /*else
                            {
                                value = el["rightExpression"]["parameter"]["value"];

                                var filterField = FilterFields.FirstOrDefault(ff => ff.Column.Name == columnPath.ToString());
                                if (filterField != null && filterField.OperationType.Value == int.Parse(comparisonType.ToString()) && filterField.IsValid(value.ToString()))
                                    searchCount++;
                            }*/
                        }
                    }

                    if (searchCount >= columnCount)
                        return true;
                }

                return false;
            }
            return true;
        }

        public void AddFilterFieldMethod(object obj)
        {
            var filterField = new FilterField
            {
               Columns = Columns
            };
            FilterFields.Add(filterField);
        }
        public void AddTriggerFieldMethod(object obj)
        {
            var triggerField = new TriggerField(Entity)
            {
                Columns = Columns
            };
            TriggerFields.Add(triggerField);
        }
    }
}
