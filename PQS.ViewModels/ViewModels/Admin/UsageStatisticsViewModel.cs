using Helpers.Interfaces;
using PQS.Models.Models;
using PQS.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using PQS.Models;
using System.Diagnostics;
using System.Windows.Documents;

namespace PQS.ViewModels.ViewModels.Admin
{
    public class UsageStatisticsViewModel : ViewModelBase
    {
        #region Vars
        private readonly IRetrievor<ModuleExecutionRecords> moduleExecutionRecordsRetrievor;
        private readonly IRetrievor<Module> moduleRetrievor;
        #endregion

        #region Properties
        private ObservableCollection<ModuleExecutionRecords> moduleExecutions;
        /// <summary>
        /// A collection of all the module execution records 
        /// </summary>
        public ObservableCollection<ModuleExecutionRecords> ModuleExecutionRecords
        {
            get { return moduleExecutions; }
            set
            {
                moduleExecutions = value;
                OnPropertyChanged("ModuleExecutionRecords");
            }
        }

        private ModuleExecutionRecords selectedModule;
        public ModuleExecutionRecords SelectedModule
        {
            get { return selectedModule; }
            set
            {
                selectedModule = value;
                OnPropertyChanged("SelectedModule");
            }
        }

        private ObservableCollection<UsersStats> usersStats;
        public ObservableCollection<UsersStats> UsersStats
        {
            get { return usersStats; }
            set
            {
                usersStats = value;
                OnPropertyChanged("UsersStats");
            }
        }

        private DateTime startDate = DateTime.Now.AddDays(-7);
        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                startDate = value;
                DisplayCorrectList();
                OnPropertyChanged("StartDate");
            }
        }

        private DateTime endDate = DateTime.Now.AddDays(1).Date;
        public DateTime EndDate
        {
            get { return endDate; }
            set
            {
                endDate = value;
                DisplayCorrectList();
                OnPropertyChanged("EndDate");
            }
        }

        private bool allModuleRecords = true;
        public bool AllModuleRecords
        {
            get { return allModuleRecords; }
            set
            {
                allModuleRecords = value;
                OnPropertyChanged("AllModuleRecords");
            }
        }

        private bool filteredModuleRecords = false;
        public bool FilteredModuleRecords
        {
            get { return filteredModuleRecords; }
            set
            {
                filteredModuleRecords = value;
                OnPropertyChanged("FilteredModuleRecords");
            }
        }

        private string changeListButtonContent;
        /// <summary>
        /// The content of the button at the bottom of the page
        /// </summary>
        public string ChangeListButtonContent
        {
            get { return changeListButtonContent; }
            set
            {
                changeListButtonContent = value;
                OnPropertyChanged("ChangeListButtonContent");
            }
        }
        #endregion

        #region Constructors
        public UsageStatisticsViewModel(IRetrievor<ModuleExecutionRecords> moduleExecutionRecordsRetrievor, IRetrievor<Module> iModuleRetrievor)
        {
            this.moduleExecutionRecordsRetrievor = moduleExecutionRecordsRetrievor;
            this.moduleRetrievor = iModuleRetrievor;

            ChangeListButtonContent = "User Records";
            UpdateExecutionRecords();
        }
        #endregion

        #region
        public RelayCommand ChangeViewCommand
        {
            get
            {
                return new RelayCommand(param =>
                {
                    FilteredModuleRecords = FilteredModuleRecords ? false : true;
                    AllModuleRecords = AllModuleRecords ? false : true;

                    DisplayCorrectList();
                });

            }
        }

        #endregion

        #region Methods
        private void UpdateExecutionRecords()
        {
            ModuleExecutionRecords = new ObservableCollection<ModuleExecutionRecords>(
                moduleExecutionRecordsRetrievor.GetAll()
                .Where(x => x.ExecutionDate >= StartDate && x.ExecutionDate <= EndDate)
                .OrderByDescending(x => x.ExecutionDate));
        }

        private void DistinctModuleRecords()
        {
            UpdateExecutionRecords();

            var uniqueRecords = new ObservableCollection<ModuleExecutionRecords>(
                                moduleExecutionRecordsRetrievor.GetAll()
                .Where(x => x.ExecutionDate >= StartDate && x.ExecutionDate <= EndDate)
                .GroupBy(x => x.Module.AdminIdentifyingName).Select(x => x.First()).Distinct()
                .OrderByDescending(x => x.ExecutionDate));

            UsersStats = new ObservableCollection<UsersStats>(uniqueRecords.Select(x => new UsersStats() { Title = x.Module.AdminIdentifyingName }).ToList());


            foreach(var UserStat in UsersStats)
            {
                UserStat.ModuleExecutionRecords = new ObservableCollection<Models.Models.ModuleExecutionRecords>();

                List<ModuleExecutionRecords> tempRecords = new List<ModuleExecutionRecords>(ModuleExecutionRecords.Where(x => x.Module.AdminIdentifyingName.Equals(UserStat.Title))
                    .OrderByDescending(x => x.ExecutionDate));

                foreach (var user in tempRecords)
                {
                    Debug.WriteLine(user.Module.PK_Module_Id + " " + user.Module.AdminIdentifyingName);
                    Debug.WriteLine(user.User.PK_User_Id + " " + user.User.Username);

                    var exists = UserStat.ModuleExecutionRecords.Where(x => x.User.PK_User_Id == user.User.PK_User_Id 
                        && x.Module.PK_Module_Id == user.Module.PK_Module_Id).FirstOrDefault();

                    if (exists == null) // found that hasnt been found before
                    {
                        user.ExecutionCounter++;
                        UserStat.ModuleExecutionRecords.Add(user);
                    }
                    else
                    {
                        exists.ExecutionCounter++;
                    }

                    UserStat.NumOfExecutions++;
                }

                
            }
            
        }

        private void DisplayCorrectList()
        {
            if (FilteredModuleRecords)
            {
                ChangeListButtonContent = "All Records";
                DistinctModuleRecords();
            }
            else
            {
                ChangeListButtonContent = "User Records";
                UpdateExecutionRecords();
            }
        }
        #endregion
    }
}
