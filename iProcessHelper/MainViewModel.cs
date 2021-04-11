using iProcessHelper.DBContexts;
using iProcessHelper.DBContexts.DBModels;
using iProcessHelper.Helpers;
using iProcessHelper.JsonProcessModels.Short;
using iProcessHelper.Models;
using iProcessHelper.MVVM;
using iProcessHelper.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace iProcessHelper
{
    class MainViewModel : NotifyPropertyChanged
    {
        private string searchedProcessName;
        private int blurRadius;
        private bool isTreeLoading;
        private double currentProgress;

        public string SearchedProcessName { get => searchedProcessName; set { searchedProcessName = value; OnPropertyChanged(); } }

        public int BlurRadius { get => blurRadius; set { blurRadius = value; OnPropertyChanged(); } }
        public bool IsTreeLoading { get => isTreeLoading; set { isTreeLoading = value; CommandManager.InvalidateRequerySuggested(); } }

        public Command Load { get; set; }
        public Command AddFilter { get; set; }
        public Command Apply { get; set; }
        public Command DeleteFilter { get; set; }
        public ObservableCollection<ProcessTreeViewElement> Processes { get; set; } = new ObservableCollection<ProcessTreeViewElement>();
        public ObservableCollection<SysSchema> Local { get; set; }

        public ObservableCollection<FilterObject> FilterObjects { get; set; }

        private event Action<ProcessTreeViewElement> OnCollectionUpdate;
        private event Action<ProcessTreeViewElement, ProcessTreeViewElement> OnChildCollectionUpdate;
        private event Action OnProcessClear;

        private event Action<SysSchema> OnEntitiesCollectionUpdate;

        private readonly BackgroundWorker _worker;
        public double CurrentProgress { get => currentProgress; set { currentProgress = value; OnPropertyChanged(); } }

        public ObservableCollection<FilterType> FilterTypes { get; set; }

        public MainViewModel()
        {
            this.InitializeCommand();

            IsTreeLoading = false;

            OnCollectionUpdate += MainViewModel_OnCollectionUpdate;
            OnChildCollectionUpdate += MainViewModel_OnChildCollectionUpdate;
            OnProcessClear += MainViewModel_OnProcessClear;
            OnEntitiesCollectionUpdate += MainViewModel_OnEntitiesCollectionUpdate;

            _worker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };

            _worker.ProgressChanged += ProgressChanged;
            _worker.DoWork += DoWork;
            _worker.RunWorkerCompleted += RunWorkerCompleted;

            FilterObjects = new ObservableCollection<FilterObject>();
            FilterTypes = Constants.filterTypes;
        }

        private void MainViewModel_OnEntitiesCollectionUpdate(SysSchema obj)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Constants.entities.Add(obj);
            });
        }

        private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            IsTreeLoading = false;
        }
        private void DoWork(object sender, DoWorkEventArgs e)
        {
            this.OnProcessClear();
            using (var sysSchemaContext = new SysSchemaContext())
            {
                sysSchemaContext.SysSchemas.Load();

                Local = sysSchemaContext.SysSchemas.Local;
                var processes = Local.OrderBy(e1 => e1.CreatedOn).Where(e1 => e1.ManagerName == "ProcessSchemaManager");

                var count = processes.Count();
                double percent = 0;
                foreach (var process in processes)
                {
                    double step = 100.0 / count;
                    percent += step;
                    _worker.ReportProgress((int)percent);

                    if (process.ParentId != null && process.ParentId != Guid.Empty)
                    {
                        var parent = Processes.FirstOrDefault(e1 => e1.SysSchema.Id == process.ParentId);
                        if (parent != null)
                        {
                            var json = new ProcessMetadataParser().Deserialize<ProcessModelShort>(process.MetaData);
                            OnChildCollectionUpdate(parent, new ProcessTreeViewElement
                            {
                                SysSchema = process,
                                Json = json
                        });
                        }
                    }
                    else
                    {
                        var json = new ProcessMetadataParser().Deserialize<ProcessModelShort>(process.MetaData);
                        OnCollectionUpdate(new ProcessTreeViewElement
                        {
                            SysSchema = process,
                            Json = json
                        });
                    }
                }

                var entities = Local.Where(e1 => e1.ManagerName == "EntitySchemaManager" && e1.ParentId == Guid.Parse("af5f2299-b00d-4480-bd18-6e4b188ea0ab")).OrderBy(e1 => e1.Caption);
                foreach (var entity in entities)
                {
                    OnEntitiesCollectionUpdate(entity);
                }

                OnPropertyChanged(nameof(Processes));
            }

            using (var sysSettings = new SysSettingsContext())
            {
                var sysSetting = sysSettings.SysSettings.FirstOrDefault(x => x.Code == "SiteUrl");
                Constants.SiteUrl = sysSetting.SysSettingsValues.FirstOrDefault().TextValue;
            }
            CurrentProgress = 0;
        }
        private void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            CurrentProgress = e.ProgressPercentage;
        }

        private void MainViewModel_OnProcessClear()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Processes.Clear();
            });
        }
        private void MainViewModel_OnChildCollectionUpdate(ProcessTreeViewElement obj1, ProcessTreeViewElement obj2)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                obj1.Items.Add(obj2);
            });
        }
        private void MainViewModel_OnCollectionUpdate(ProcessTreeViewElement obj)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Processes.Add(obj);
            });
        }

        private void InitializeCommand()
        {
            Load = new Command(LoadMethod, LoadCanExecute);
            AddFilter = new Command(AddFilterMethod, IsHasProcesses);
            Apply = new Command(ApplyMethod, IsHasProcesses);
            DeleteFilter = new Command(DeleteFilterMethod);
        }

        private bool IsHasProcesses(object arg)
        {
            return Processes.Any();
        }

        private void DeleteFilterMethod(object obj)
        {
            if(obj is ProcessSchemaStartSignalEvent startSignal)
                FilterObjects.Remove(startSignal);
        }

        private void AddFilterMethod(object obj)
        {
            var addFilterWindow = new SelectWindow(Constants.filterTypes, App.Current.MainWindow);
            if (addFilterWindow.ShowDialog() == true)
            {
                if (addFilterWindow.SelectedItem is FilterType filterType)
                {
                    var filter = Activator.CreateInstance(Type.GetType($"iProcessHelper.Models.{filterType.Name}"));
                    if(filter is ProcessSchemaStartSignalEvent startSignal)
                        FilterObjects.Add(startSignal);
                }
            }
        }

        private bool LoadCanExecute(object arg)
        {
            return !this.IsTreeLoading;
        }

        private void LoadMethod(object obj)
        {
            IsTreeLoading = true;

            _worker.RunWorkerAsync();
        }

        private void Search(string value)
        {
            Task.Factory.StartNew(() =>
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
            });
        }

        private void ApplyMethod(object obj)
        {
            Task.Factory.StartNew(() =>
            {
                if (Processes.Count == 0)
                    return;

                /*if (string.IsNullOrEmpty(SearchedProcessName))
                {
                    foreach (var process in Processes)
                        process.IsVisible = true;
                    return;
                }*/

                foreach (var process in Processes)
                {
                    var result = true;

                    if (!string.IsNullOrEmpty(SearchedProcessName))
                        result = process.SysSchema.Caption.Contains(SearchedProcessName) || process.SysSchema.Name.Contains(SearchedProcessName);

                    if (FilterObjects.Any())
                        result = result && this.GetFilterResult(process);

                    process.IsVisible = result;
                }
            });
        }

        private bool GetFilterResult(ProcessTreeViewElement process)
        {
            foreach (var obj in FilterObjects)
            {
                if (!obj.Filter(process))
                    return false;
            }
            return true;
        }
    }
}
