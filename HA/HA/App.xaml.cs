using HA.Model;
using HA.Views;
using System;
using System.Collections.Generic;
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
            //MainPage = new NavigationPage(new List());

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
