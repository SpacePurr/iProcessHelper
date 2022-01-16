using iProcessHelper.DBContexts;
using iProcessHelper.DBContexts.DBModels;
using iProcessHelper.DBContexts.Repository;
using iProcessHelper.JsonModels.JsonProcessModels;
using iProcessHelper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iProcessHelper.Helpers
{
    class BackgroundLoadFromDBService
    {
        public static void Load<T>(BackgroundWorker worker, IEnumerable<T> entities, Action<T> backgroundAction)
        {
            var count = entities.Count();

            double percent = 0;

            foreach (var entity in entities)
            {
                double step = 100.0 / count;
                percent += step;
                worker.ReportProgress((int)percent);

                backgroundAction(entity);
            }
        }

        public static void LoadParentProcesses(BackgroundWorker worker, Action<ProcessTreeViewElement> updateAction)
        {
            var context = new SysSchemaContext();
            var sysSchemas = context.SysSchemas
                .Where(x => x.ManagerName == "ProcessSchemaManager" && x.ParentId == null)
                .OrderBy(x => x.Caption)
                .ToList();

            Load(worker, sysSchemas, (sysSchema) =>
            {
                var json = MetadataParser.Deserialize<ProcessModel>(sysSchema.MetaData);

                updateAction(new ProcessTreeViewElement
                {
                    SysSchema = sysSchema,
                    Json = json
                });
            });
        }

        public static void LoadChildProcesses(BackgroundWorker worker, IEnumerable<ProcessTreeViewElement> processes, Action<ProcessTreeViewElement, ProcessTreeViewElement> updateAction)
        {
            var context = new SysSchemaContext();
            var schemas = context.SysSchemas
                .Where(x => x.ManagerName == "ProcessSchemaManager" && x.ParentId != null)
                .OrderBy(x => x.Caption)
                .ToList();

            Load(worker, schemas, (x) =>
            {
                var parent = processes.FirstOrDefault(e1 => e1.SysSchema.Id == x.ParentId);

                if (parent != null)
                {
                    var json = MetadataParser.Deserialize<ProcessModel>(x.MetaData);

                    updateAction(parent, new ProcessTreeViewElement
                    {
                        SysSchema = x,
                        Json = json
                    });
                }
            });
        }

        public static void LoadEntities(BackgroundWorker worker, Action<VwSysSchemaInfo> updateEvent)
        {
            var context = new VwSysSchemaInfoContext();
            var schemas = context.VwSysSchemaInfos
                .Where(x => x.ManagerName == "EntitySchemaManager")
                .OrderBy(x => x.Caption)
                .ToList();

            Load(worker, schemas, (schema) => updateEvent(schema));
        }
    }
}
