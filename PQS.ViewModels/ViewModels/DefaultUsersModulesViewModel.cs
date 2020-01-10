using Helpers.Interfaces;
using PQS.Models.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace PQS.ViewModels.ViewModels
{
    public class DefaultUsersModulesViewModel : UsersModulesViewModel
    {
        public DefaultUsersModulesViewModel(IAdvancedRetrievor<User, UsersModules> userModules, Categories category, User currentUser, IUpdator<User> updateNav)
            : base(userModules, category, currentUser, updateNav)
        {
        }

        protected override void DisplayModulesCollection()
        {
            Modules = new ObservableCollection<UsersModules>(userModules.GetFilteredList(currentUser)
                .Where(x => x.Module.Category.PK_Category_id == category.PK_Category_id && x.Module.IsActive).ToList());
        }
    }
}
