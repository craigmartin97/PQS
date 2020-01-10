using Helpers.Interfaces;
using Helpers.ObserverFavourites;
using PQS.Models.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace PQS.ViewModels.ViewModels
{
    public class FavouritesViewModel : UsersModulesViewModel
    {
        private bool isFavouriteViewModel = true;
        public bool IsFavouriteViewModel
        {
            get { return isFavouriteViewModel; }
            set
            {
                isFavouriteViewModel = value;
                OnPropertyChanged("IsFavouriteViewModel");
            }
        }

        public FavouritesViewModel(IAdvancedRetrievor<User, UsersModules> userModules, User currentUser, IUpdator<User> updateNav)
            : base(userModules, currentUser, updateNav)
        {
        }

        /// <summary>
        /// Retrieve all the modules for the user that are marked as favourites
        /// </summary>
        protected override void DisplayModulesCollection()
        {
            Modules = new ObservableCollection<UsersModules>(userModules.GetFilteredList(currentUser)
                          .Where(x => x.IsFavourite && x.Module.IsActive).ToList());

        }
    }
}
