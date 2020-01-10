using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;

namespace PQS.Models.Models
{
    public class UserModuleRequests : ModelBase
    {
        [Key]
        public int PK_UserModuleRequest_Id { get; set; }

        public virtual Module Module { get; set; }

        public virtual User User { get; set; }

        public PermissionLevels _permissionLevel;
        public virtual PermissionLevels PermissionLevel
        {
            get { return _permissionLevel; }
            set
            {
                _permissionLevel = value;
                OnPropertyChanged("PermissionLevel");
            }
        }

        public DateTime? RequestDate { get; set; }

        public DateTime? RequestCompletionDate { get; set; }

        public bool? IsComplete { get; set; }

        public bool? RequestAccepted { get; set; }


        private bool _requestAlreadyMade;

        [NotMapped]
        public bool RequestAlreadyMade
        {
            get { return _requestAlreadyMade; }
            set { _requestAlreadyMade = value; OnPropertyChanged("RequestAlreadyMade"); }
        }

        private ObservableCollection<PermissionLevels> _permissionLevels;

        [NotMapped]
        public ObservableCollection<PermissionLevels> PermissionLevels
        {
            get { return _permissionLevels; }
            set
            {
                _permissionLevels = value;
                OnPropertyChanged("PermissionLevels");
            }
        } 
    }
}
