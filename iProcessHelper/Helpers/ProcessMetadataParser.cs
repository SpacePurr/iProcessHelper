using iProcessHelper.DBContexts.DBModels;
using iProcessHelper.JsonProcessModels.Short;
using iProcessHelper.Models;
using iProcessHelper.ProcessJsonModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iProcessHelper.Helpers
{
    class ProcessMetadataParser
    {
        public T Deserialize<T>(byte[] data) where T : class
        {
            using (var stream = new MemoryStream(data))
            using (var reader = new StreamReader(stream, Encoding.UTF8))
                return JsonSerializer.Create().Deserialize(reader, typeof(T)) as T;
        }

        public ObservableCollection<ProcessTreeViewElement> GetMainParents(ObservableCollection<ProcessTreeViewElement> processes, ProcessTreeViewElement process)
        {
            return this.GetNextParent(processes, process);
        }

        public ObservableCollection<ProcessTreeViewElement> GetNextParent(ObservableCollection<ProcessTreeViewElement> processes, ProcessTreeViewElement process)
        {
            var list = new ObservableCollection<ProcessTreeViewElement>();

            var flowElements = processes.Where(p => p.Json.Metadata.Schema.FlowElements.Select(fe => fe.SchemaUId).Contains(process.SysSchema.UId));

            if (flowElements.Any())
                foreach (var fe in flowElements)
                    foreach (var item in this.GetNextParent(processes, fe))
                        list.Add(item);
            else
                list.Add(process);

            return list;
        }

        public ProcessTreeViewElement GetChildrenTree(ObservableCollection<ProcessTreeViewElement> processes, ProcessTreeViewElement process)
        {
            return new ProcessTreeViewElement
            {
                SysSchema = process.SysSchema,
                Items = this.GetChildren(processes, process)
            };
        }

        public ObservableCollection<ProcessTreeViewElement> GetChildren(ObservableCollection<ProcessTreeViewElement> processes, ProcessTreeViewElement process)
        {
            var list = new ObservableCollection<ProcessTreeViewElement>();

            var flowElements = process.Json.Metadata.Schema.FlowElements.Where(fe => fe.TypeName.Contains("ProcessSchemaSubProcess"));
            foreach (var fe in flowElements)
            {
                var findedProc = processes.FirstOrDefault(p => p.SysSchema.UId == fe.SchemaUId);
                if (findedProc != null)
                    list.Add(new ProcessTreeViewElement
                    {
                        SysSchema = findedProc.SysSchema,
                        Items = this.GetChildren(processes, findedProc)
                    });
            }

            return list;
        }
    }
}
