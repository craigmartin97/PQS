using System.Windows;
using CustomDialog.Service;
using Helpers.Login;
using PQS.ViewModels.ViewModels;
using Helpers.CategoriesHelpers;
using System.Windows.Forms;
using Helpers.UserHelpers;
using Helpers.ModulesHelpers;
using System.Reflection;
using System.Drawing;
using Helpers.ObserverFavourites;
using PQS.Models.Models;

namespace PQS.Views
{
    /// <summary>
    /// Interaction logic for PQSWindow.xaml
    /// </summary>
    public partial class PQSWindow : Window
    {
        public PQSWindow(User currentUser = null)
        {
            InitializeComponent();
            DataContext = new PQSWindowViewModel(new UsersModulesProcessor(), new UsersCategoriesRetrievor(),
                new UserProcessor(), currentUser ?? null);

            ChangeBorderThickness();
        }

        private void PQS_StateChanged(object sender, System.EventArgs e)
        {
            ChangeBorderThickness();
        }

        private void ChangeBorderThickness()
        {
            outerBorder.BorderThickness = this.WindowState == WindowState.Maximized ?
                new Thickness(6) : new Thickness(0);
        }
    }
}
