using iProcessHelper.DBContexts;
using iProcessHelper.DBContexts.DBModels;
using iProcessHelper.DBContexts.Repository;
using iProcessHelper.Helpers;
using iProcessHelper.JsonModels.JsonProcessModels;
using iProcessHelper.Models;
using iProcessHelper.MVVM;
using iProcessHelper.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
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
        private readonly BackgroundWorker _worker;


        private string searchedProcessName;
        public string SearchedProcessName { get => searchedProcessName; set { searchedProcessName = value; OnPropertyChanged(); this.ApplyMethod(null); } }

        private int blurRadius;
        public int BlurRadius { get => blurRadius; set { blurRadius = value; OnPropertyChanged(); } }

        private bool isTreeLoading;
        public bool IsTreeLoading { get => isTreeLoading; set { isTreeLoading = value; CommandManager.InvalidateRequerySuggested(); } }

        private double currentProgress;
        public double CurrentProgress { get => currentProgress; set { currentProgress = value; OnPropertyChanged(); } }

        #region Commands

        public Command Load { get; set; }
        public Command AddFilter { get; set; }
        public Command Apply { get; set; }
        public Command DeleteFilter { get; set; }

        #endregion

        public ObservableCollection<ProcessTreeViewElement> Processes { get; set; } = new ObservableCollection<ProcessTreeViewElement>();
        public ObservableCollection<SysSchema> Local { get; set; }
        public ObservableCollection<FilterObject> FilterObjects { get; set; }
        public ObservableCollection<FilterType> FilterTypes { get; set; }

        #region Events

        private event Action<ProcessTreeViewElement> OnCollectionUpdate;
        private event Action<ProcessTreeViewElement, ProcessTreeViewElement> OnChildCollectionUpdate;
        private event Action OnProcessClear;
        private event Action<SysSchema> OnEntitiesCollectionUpdate;

        #endregion


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
            FilterTypes = Constants.FilterTypes;
        }

        private void MainViewModel_OnEntitiesCollectionUpdate(SysSchema obj)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Constants.Entities.Add(obj);
            });
        }

        private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            IsTreeLoading = false;
        }

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            this.OnProcessClear();
            using (SqlConnection connection = new SqlConnection(DBConnection.ConnectionString))
            {
                connection.Open();

                BackgroundLoadFromDBService.LoadParentProcesses(connection, _worker, OnCollectionUpdate);
                BackgroundLoadFromDBService.LoadChildProcesses(connection, _worker, Processes, OnChildCollectionUpdate);
                BackgroundLoadFromDBService.LoadEntities(connection, _worker, OnEntitiesCollectionUpdate);

                var sqlExpression = "select * from SysSettingsValue ssv join SysSettings ss ON ss.Id=ssv.SysSettingsId where ss.Code='SiteUrl'";
                var command = new SqlCommand(sqlExpression, connection);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Constants.SiteUrl = reader["TextValue"].ToString();
                    }
                }

                connection.Close();
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
            if (obj is ProcessSchemaStartSignalEvent startSignal)
                FilterObjects.Remove(startSignal);
        }

        private void AddFilterMethod(object obj)
        {
            var addFilterWindow = new SelectWindow(Constants.FilterTypes, App.Current.MainWindow);
            if (addFilterWindow.ShowDialog() == true)
            {
                if (addFilterWindow.SelectedItem is FilterType filterType)
                {
                    var filter = Activator.CreateInstance(Type.GetType($"iProcessHelper.Models.{filterType.Name}"));

                    if (filter is ProcessSchemaStartSignalEvent startSignal)
                    {
                        FilterObjects.Add(startSignal);
                    }
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

        private void ApplyMethod(object obj)
        {
            Task.Factory.StartNew(() =>
            {
                if (Processes.Count == 0)
                    return;

                foreach (var process in Processes)
                {
                    var result = true;

                    if (!string.IsNullOrEmpty(SearchedProcessName))
                    {
                        result = process.SysSchema.Caption.Contains(SearchedProcessName) || process.SysSchema.Name.Contains(SearchedProcessName);
                    }

                    if (FilterObjects.Any())
                    {
                        result = result && this.GetFilterResult(process);
                    }

                    process.IsVisible = result;
                }
            });
        }

        private bool GetFilterResult(ProcessTreeViewElement process)
        {
            foreach (var obj in FilterObjects)
            {
                if (!obj.Filter(process))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
