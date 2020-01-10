using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.ComponentModel;
using PQS.Views;
using System.Diagnostics;
using PQS.Models.Models;
using Helpers.Executor;
using Helpers.ExecutionRecords;
using Helpers.Interfaces;
using Helpers.ObserverFavourites;
using Helpers.ModulesHelpers;
using Helpers.RegEdit;
using Helpers.Login;
using CustomDialog.Service;
using Helpers.UserHelpers;
using CustomDialog.Views;
using CustomDialog.Dialogs;
using PQS.ViewModels.ViewModels.CustomControls;
using PQS.Models;

namespace PQS
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, IDisposable, IObserver, IUsername
    {
        private System.Windows.Forms.NotifyIcon notifyIcon = new System.Windows.Forms.NotifyIcon();
        private System.Windows.Forms.ContextMenu contextMenu = new System.Windows.Forms.ContextMenu();
        private static List<UsersModules> modules = new List<UsersModules>();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            this.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            Subject.RegisterObserver(this);

            User currentUser = new UserProcessor().GetUser();
            CheckIfUserHasUsername(currentUser); //this assumes they are new user, if username is null or empty

            MainWindow = new PQSWindow(currentUser);
            this.ShutdownMode = ShutdownMode.OnMainWindowClose;
            MainWindow.Closing += new CancelEventHandler(MainWindow_Closing);

            notifyIcon.DoubleClick += (s, args) => ShowMainWindow();

            notifyIcon.Icon = PQS.Properties.Resources.playPQSLogo;
            notifyIcon.Visible = true;

            try
            {
                MainWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            
        }

        private void CreateContextMenu()
        {

            if (Subject.CurrentUser != null)
            {
                contextMenu = new System.Windows.Forms.ContextMenu(); // create new one, reset contextmenu and redo it
                IAdvancedRetrievor<User, UsersModules> userModules = new UsersModulesProcessor();

                modules = new List<UsersModules>(userModules.GetFilteredList(Subject.CurrentUser)
                    .Where(x => x.IsFavourite && x.Module.IsActive).Distinct().ToList());


                int counter = 0;
                System.Windows.Forms.MenuItem[] menuItems = new System.Windows.Forms.MenuItem[modules.Count];

                foreach (UsersModules module in modules.ToList())
                {
                    UsersModules mod = (UsersModules)module.Clone(); // stupid, must be visual studio 2010 issue as worked without clone on visual studio 2019
                    menuItems[counter] = new System.Windows.Forms.MenuItem();
                    menuItems[counter].Text = module.Module.AdminIdentifyingName;

                    menuItems[counter].Click += (sender, EventArgs) =>
                    {
                        menuItem_Click(sender, EventArgs, mod);
                    };

                    counter++;
                }

                menuItems = menuItems.Where(x => x != null).ToArray(); // clean up blank spaces in array
                contextMenu.MenuItems.AddRange(menuItems);
            }


            this.notifyIcon.ContextMenu = this.contextMenu;
            this.notifyIcon.Text = "Process Quality System";
        }

        void menuItem_Click(object sender, EventArgs e, UsersModules module)
        {

            MainWindow.WindowState = WindowState.Minimized;
            if (module != null)
            {
                ModuleExecutor executor = new ModuleExecutor(new AddModuleExecutionRecords());
                executor.Execute(module.Module, module.User);
                MainWindow.Topmost = false;
            }
        }

        private void ShowMainWindow()
        {
            if (MainWindow.IsVisible)
            {
                if (MainWindow.WindowState == WindowState.Minimized)
                {
                    MainWindow.WindowState = WindowState.Normal;
                }
                MainWindow.Activate();
            }
            else
            {
                MainWindow.WindowState = WindowState.Maximized;
                MainWindow.Topmost = true; // come to foreground of screen and display
                MainWindow.Show();
                MainWindow.Topmost = false; // then allow to go into the background as when apps are being executed they need to be in the foreground
            }
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            RegistryRemover remover = new RegistryRemover();
            remover.RemoveAllKeys();
            Dispose();
        }

        public void Dispose()
        {
            notifyIcon.Dispose();
        }

        public void Update()
        {
            Debug.WriteLine("Update favs in App.xaml.cs");
            CreateContextMenu();
        }

        /// <summary>
        /// Check if the username of the current logged in user is null, empty or whitespace
        /// </summary>
        /// <param name="currentUser"></param>
        private void CheckIfUserHasUsername(User currentUser)
        {
            IDialogService dialogService = new DialogService();

            string userName = PromptUserToEnterUsername(currentUser, dialogService);

            if (!string.IsNullOrWhiteSpace(userName))
                UpdateUsername(currentUser, userName);
        }

        /// <summary>
        /// If the users username is null or empty
        /// then request the user to add a username to there account.
        /// This method assumes they are also a new user and will open the 
        /// welcome message to the user
        /// </summary>
        public string PromptUserToEnterUsername(User currentUser, IDialogService dialogService)
        {
            if (string.IsNullOrWhiteSpace(currentUser.Username))
            {
                Debug.WriteLine("Alert Command Activated");
                var dialog = new WelcomeMessageViewModel("Welcome to the PQS");
                var result = dialogService.OpenDialogService(dialog);
                Debug.WriteLine(result);
                return result; // username
            }

            return null;
        }


        public void UpdateUsername(User currentUser, string userName)
        {
            try
            {
                using (PQSContext db = new PQSContext())
                {
                    User databaseUserRef = db.Users.FirstOrDefault(x => x.PK_User_Id == currentUser.PK_User_Id);

                    if (databaseUserRef != null)
                    {
                        currentUser.Username = userName;
                        databaseUserRef.Username = userName;
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error - " + ex.Message + " " + ex.InnerException);
            }

        }
    }
}
