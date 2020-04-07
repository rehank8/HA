﻿using Android.Webkit;
using HA.Model;
using HA.Services;
using HA.Views;
using MvvmHelpers;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HA.ViewModels
{
    public class RegisterViewModel : INotifyPropertyChanged
    {
        AccountService accntService = new AccountService();
        LoginResponse login = Helper.UserProfileDBService.GetAuthUser();
        public RegisterViewModel()
        {
            IsLoginornot();
            GetLocation();
            GetVendorsByLocation();
        }
        public ICommand RegistrationCommand => new Command(Registration_clicked);
        public ICommand GoToLoginCommand => new Command(GoToLogon_clicked);
        public ICommand RegisterCommand => new Command(Register_clicked);
        public ICommand LoginCommand => new Command(Login_clicked);
        public ICommand ListSelectedCommand => new Command(List_clicked);
        public ICommand SearchVendorCommand => new Command(Search_clicked);
        public ICommand EmptySearchCommand => new Command(Search1_clicked);
        public ICommand CloseRegCommand => new Command(CloseReg_clicked);
        public ICommand LoginCloseCommand => new Command(Loginclose_clicked);
        public ICommand LogoutCommand => new Command(Logout_clicked);
        private bool _IsRegisterVisible;
        private bool _IsLoginVisible, _isLogin, _isLogout;
        public bool result = false;
        private string _email, _password, _UserType, _currentLocation;


        public event PropertyChangedEventHandler PropertyChanged;
        private UserProfile _user;

        public UserProfile User
        {
            get { return _user; }
            set { _user = value; NotifyPropertyChanged(); }
        }
        public string UserType
        {
            get { return _UserType; }
            set { _UserType = value; NotifyPropertyChanged(); }
        }
        public string Email
        {
            get { return _email; }
            set { _email = value; NotifyPropertyChanged(); }
        }
        public string Password
        {
            get { return _password; }
            set { _password = value; NotifyPropertyChanged(); }
        }
        public string CurrentLocation
        {
            get { return _currentLocation; }
            set { _currentLocation = value; NotifyPropertyChanged(); }
        }
        bool _isUserType, _isEmail, _isPasword;
        public bool IsUserType
        {
            get { return _isUserType; }
            set { _isUserType = value; NotifyPropertyChanged(); }
        }
        public bool IsEmail
        {
            get { return _isEmail; }
            set { _isEmail = value; NotifyPropertyChanged(); }
        }
        public bool IsLogin
        {
            get { return _isLogin; }
            set { _isLogin = value; NotifyPropertyChanged(); }
        }
        public bool IsLogout
        {
            get { return _isLogout; }
            set { _isLogout = value; NotifyPropertyChanged(); }
        }
        public bool IsPassword
        {
            get { return _isPasword; }
            set { _isPasword = value; NotifyPropertyChanged(); }
        }
        public bool IsRegisterVisible
        {
            get { return _IsRegisterVisible; }
            set { _IsRegisterVisible = value; NotifyPropertyChanged(); }
        }

        public bool IsLoginVisible
        {
            get { return _IsLoginVisible; }
            set { _IsLoginVisible = value; NotifyPropertyChanged(); }
        }
        private List<UserIndex> _vendors;

        public List<UserIndex> Vendors
        {
            get { return _vendors; }
            set { _vendors = value; NotifyPropertyChanged(); }
        }

        async void Register_clicked()
        {
            if (!Registrationvalidation())
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please fill all fields", "Ok");
            }
            else
            {
                UserProfile user = new UserProfile()
                {
                    usertype = UserType,
                    emailid = Email,
                    password = Password
                };

                await Task.Run(() =>
                {
                    accntService.RegisterUser(user);
                    result = true;
                });
                await Application.Current.MainPage.DisplayAlert("Message", "Succesfully Registerd", "Ok");
                await Application.Current.MainPage.Navigation.PopAsync();
                IsRegisterVisible = false;
                IsLoginVisible = true;
            }

        }

        public bool Registrationvalidation()
        {
            bool isValid = true;
            if (string.IsNullOrEmpty(UserType))
            {
                IsUserType = true;
                isValid = false;
            }
            else
                IsUserType = false;
            if (string.IsNullOrEmpty(Email))
            {
                IsEmail = true;
                isValid = false;
            }
            else
                IsEmail = false;
            if (string.IsNullOrEmpty(Password))
            {
                IsPassword = true;
                isValid = false;
            }
            else
                IsPassword = false;
            return isValid;
        }
        public bool Loginvalidation()
        {
            bool isValid = true;

            if (string.IsNullOrEmpty(Email))
            {
                IsEmail = true;
                isValid = false;
            }
            else
                IsEmail = false;
            if (string.IsNullOrEmpty(Password))
            {
                IsPassword = true;
                isValid = false;
            }
            else
                IsPassword = false;
            return isValid;
        }

        private void Registration_clicked()
        {
            IsRegisterVisible = true;
            IsLoginVisible = false;
        }

        private void GoToLogon_clicked()
        {
            IsLoginVisible = true;
            IsRegisterVisible = false;
        }

        async void Login_clicked()
        {
            if (!Loginvalidation())
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please fill all fields", "Ok");
            }
            else
            {
                UserProfile user = new UserProfile()
                {
                    emailid = Email,
                    password = Password

                };
                LoginResponse loginResponse = null;
                await Task.Run(() =>
                {
                    loginResponse = accntService.Login(user);
                });
                if (loginResponse.Isvalid)
                {
                    Helper.UserProfileDBService.Save(loginResponse);
                    string token = loginResponse.access_token;
                    string tokenType = loginResponse.token_type;
                    string Authorization_Token = tokenType + " " + token;

                    UserProfile userProfile = accntService.GetUserDetail(Authorization_Token);

                    if (userProfile.FKRoleId == 2)
                        Application.Current.MainPage = new NavigationPage(new VendorsList());
                    else if (userProfile.FKRoleId == 3)
                        Application.Current.MainPage = new NavigationPage(new HomePage());
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", loginResponse.error_description, "OK");
                }

            }
        }
        private void Loginclose_clicked()
        {
            IsLoginVisible = IsRegisterVisible = false;
        }
        private void CloseReg_clicked()
        {
            IsLoginVisible = IsRegisterVisible = false;
        }

        private async void GetLocation()
        {
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 100;

                var position = await locator.GetLastKnownLocationAsync();
                position = await locator.GetPositionAsync(TimeSpan.FromSeconds(20), null, true);
                var addresses = await locator.GetAddressesForPositionAsync(position, null);
                var address = addresses.FirstOrDefault();
               
                if (address != null )
                {
                    CurrentLocation = address.Locality;
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
            }
        }

        private async void getcurrentlocation()
        {
            //    //var geoInfo = await Plugin.Geolocator.CrossGeolocator.Current.GetPositionAsync(new TimeSpan(0, 0, 10), null, true);

            //    var request = new GeolocationRequest(GeolocationAccuracy.Best);
            //    var request = new GeolocationRequest(GeolocationAccuracy.Best);
            //    var locations = await Geolocation.GetLocationAsync(request);
            //    if (geoInfo != null)
            //    {
            //        // get the lat lng
            //        var Location = new Point
            //        {
            //          X=geoInfo.
            //        }
            //        // get the location accuracy
            //            var locationAccuracy = (int)geoInfo.Accuracy;
            //    }
        }
        async void List_clicked(object obj)
        {
            var e = obj as UserIndex;
            // UserIndex categoryName = null;
            // categoryName = e.CurrentSelection.FirstOrDefault() as UserIndex;
            await Application.Current.MainPage.Navigation.PushAsync(new VendorListCount(e.CategoryName, CurrentLocation));
        }

        private void Search_clicked()
        {
            GetVendorsByLocation();
        }
        private void Search1_clicked()
        {
            if (string.IsNullOrEmpty(CurrentLocation))
                GetVendorsByLocation();
        }


        async void GetVendorsByLocation()
        {
            try
            {
                await Task.Run(() =>
                {
                    Vendors = accntService.GetVendors(CurrentLocation);
                });
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void IsLoginornot()
        {
            if (login != null)
            {
                IsLogout = true; IsLogin = false;
            }
            else
            {
                IsLogin = true; IsLogout = false;
            }
        }
        async void Logout_clicked()
        {
            bool isLogoutornot = await Application.Current.MainPage.DisplayAlert("Alert", "Are you sure you want to Logout?", "Yes", "No");
            if (isLogoutornot)
            {
                Helper.UserProfileDBService.Delete();
                Helper.LoginResponse = null;
                Application.Current.MainPage = new NavigationPage(new HomePage());
            }
        }
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
