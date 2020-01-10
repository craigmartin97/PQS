using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using PQS.Models.Models;

namespace Helpers.ObserverFavourites
{
    public static class Subject
    {

        private static List<IObserver> Observers = new List<IObserver>();

        private static ObservableCollection<UsersModules> favouriteModules;
        public static ObservableCollection<UsersModules> FavouriteModules
        {
            get { return favouriteModules; }
            set
            {
                favouriteModules = value;
                NotifyObservers();
            }
        }

        public static User CurrentUser { get; set; }

        public static void RegisterObserver(IObserver observer)
        {
            Observers.Add(observer);
        }

        public static void NotifyObservers()
        {
            foreach (IObserver observer in Observers)
            {
                observer.Update();
            }
        }

    }
}
