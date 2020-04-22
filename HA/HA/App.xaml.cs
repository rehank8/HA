using HA.Model;
using HA.Views;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Plugin.Toast;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HA
{
    public partial class App : Application
    {
        LoginResponse login = Helper.UserProfileDBService.GetAuthUser();
        UserProfile user = Helper.RoleIdDbService.GetAuthUser();
        public App()
        {
            InitializeComponent();
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            if (login != null)
            {
                if (user != null)
                {
                    if (user.FKRoleId == 2)
                        MainPage = new NavigationPage(new VendorsList());
                    else if (user.FKRoleId == 3)
                        MainPage = new NavigationPage(new HomePage());
                }
                else
                    MainPage = new NavigationPage(new HomePage());
            }
            else
                MainPage = new NavigationPage(new HomePage());
        }
        void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            CheckConnection(e.NetworkAccess);
        }
       
        async static void CheckConnection(NetworkAccess access)
        {
            try
            {
                if (access != NetworkAccess.Internet)
                {
                    await Task.Yield();
                    await Application.Current.MainPage.DisplayAlert("", "No Internet!", "Ok");
                    CrossToastPopUp.Current.ShowToastError("Offline");
                }
                else
                {
                    CrossToastPopUp.Current.ShowToastSuccess("Back online");
                }
            }
            catch (Exception ex)
            {
            }
        }
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
