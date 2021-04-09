using iProcessHelper.DBContexts;
using iProcessHelper.DBContexts.DBModels;
using iProcessHelper.Helpers;
using iProcessHelper.Models;
using iProcessHelper.MVVM;
using iProcessHelper.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace iProcessHelper
{
    class MainViewModel : NotifyPropertyChanged
    {
        private string searchedProcessName;
        private int blurRadius;
        private bool spinnerVisible;

        public string SearchedProcessName { get => searchedProcessName; set { searchedProcessName = value; OnPropertyChanged(); this.Search(value); } }

        public int BlurRadius { get => blurRadius; set { blurRadius = value; OnPropertyChanged(); } }
        public bool SpinnerVisible { get => spinnerVisible; set { spinnerVisible = value; OnPropertyChanged(); } }

        public string SiteUrl { get; set; }

        public Command Load { get; set; }
        public ObservableCollection<ProcessTreeViewElement> Processes { get; set; } = new ObservableCollection<ProcessTreeViewElement>();
        public ObservableCollection<SysSchema> Local { get; set; }
        public MainViewModel()
        {
            this.InitializeCommand();
        }

        private void InitializeCommand()
        {
            Load = new Command(LoadMethod);
        }

        private void LoadMethod(object obj)
        {
            using (var sysSchemaContext = new SysSchemaContext())
            {
                sysSchemaContext.SysSchemas.Load();

                Local = sysSchemaContext.SysSchemas.Local;
                var processes = Local.OrderBy(e => e.CreatedOn).Where(e => e.ManagerName == "ProcessSchemaManager");

                foreach (var process in processes)
                {
                    if (process.ParentId != null && process.ParentId != Guid.Empty)
                    {
                        var parent = Processes.FirstOrDefault(e => e.SysSchema.Id == process.ParentId);
                        if (parent != null)
                        {
                            parent.Items.Add(new ProcessTreeViewElement
                            {
                                SysSchema = process
                            });
                        }
                    }
                    else
                    {
                        Processes.Add(new ProcessTreeViewElement
                        {
                            SysSchema = process
                        });
                    }
                }

                OnPropertyChanged(nameof(Processes));
            }

            using (var sysSettings = new SysSettingsContext())
            {
                var sysSetting = sysSettings.SysSettings.FirstOrDefault(x => x.Code == "SiteUrl");
                Constants.SiteUrl = sysSetting.SysSettingsValues.FirstOrDefault().TextValue;
            }
        }

        private async void LoadTree()
        {

        }

        private void Search(string value)
        {
            if (Processes.Count == 0)
                return;

            if (string.IsNullOrEmpty(value))
            {
                foreach (var process in Processes)
                    process.IsVisible = true;

                return;
            }

            foreach (var process in Processes)
            {
                if (process.SysSchema.Caption.Contains(value) || process.SysSchema.Name.Contains(value))
                    process.IsVisible = true;
                else
                    process.IsVisible = false;
            }
        }
    }
}
