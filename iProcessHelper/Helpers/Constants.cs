using iProcessHelper.DBContexts.DBModels;
using iProcessHelper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iProcessHelper.Helpers
{
    class Constants
    {
        public static string SiteUrl;

        public static ObservableCollection<SysSchema> entities = new ObservableCollection<SysSchema>();

        public static List<EntitySignal> entitySignals = new List<EntitySignal>
        {
            new EntitySignal("Добавление записи", 1),
            new EntitySignal("Изменение записи", 2),
            new EntitySignal("Удаление записи", 4)
        };

        public static ObservableCollection<FilterType> filterTypes = new ObservableCollection<FilterType>
        {
            new FilterType()
            {
                Caption = "Сигнал",
                Name = "ProcessSchemaStartSignalEvent"
            }
        };
    }
}
