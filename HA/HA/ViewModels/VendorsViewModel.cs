using HA.Model;
using HA.Services;
using HA.Views;
using MvvmHelpers;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Plugin.Calendar.Models;

namespace HA.ViewModels
{
    public class VendorsViewModel : BaseViewModel
    {
        private AccountService accntService = new AccountService();
        UserProfile user = Helper.RoleIdDbService.GetAuthUser();
        public VendorsViewModel()
        {
            GetLocation();
        }
        public ICommand LogoutCommand => new Command(Logout_clicked);
        public ICommand DayCommand => new Command(Date_clicked);
        string _currentLocation;
        public string CurrentLocation
        {
            get { return _currentLocation; }
            set { _currentLocation = value; OnPropertyChanged(); }
        }
        public EventCollection Events
        {
            get; set;
        }
        private List<UserIndex> _vendors;

        public List<UserIndex> Vendors
        {
            get { return _vendors; }
            set { _vendors = value; OnPropertyChanged(); }
        }
        private string _selectedDate;

        public string SelectedDate
        {
            get { return _selectedDate; }
            set { _selectedDate = value; OnPropertyChanged(); }
        }


        private List<string> _vendorsDateTime;

        public List<string> VendorsDateTime
        {
            get { return _vendorsDateTime; }
            set { _vendorsDateTime = value; OnPropertyChanged(); }
        }
        private UserIndex _vendorModel;

        public UserIndex VendorModel
        {
            get { return _vendorModel; }
            set { _vendorModel = value; OnPropertyChanged(); }
        }
        private UserQueryDTO _uQuery;

        public UserQueryDTO UQuery
        {
            get { return _uQuery; }
            set { _uQuery = value; OnPropertyChanged(); }
        }
        async void GetVendors()
        {
            await Task.Run(() =>
            {
                Vendors = accntService.GetVendors(CurrentLocation);
            });
        }
        private async void GetLocation()
        {
            try
            {
                IsBusy = true;
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 100;
                var position = await locator.GetLastKnownLocationAsync();
                position = await locator.GetPositionAsync(TimeSpan.FromMilliseconds(100), null, true);
                var addresses = await locator.GetAddressesForPositionAsync(position, null);
                var address = addresses.FirstOrDefault();
                if (address != null)
                {
                    CurrentLocation = address.Locality;
                }
                GetVendors();
            }
            catch (FeatureNotSupportedException fnsEx)
            {
            }
            finally
            {
                IsBusy = false;
            }
        }
        async void Date_clicked()
        {
            DateTime SDate = (Convert.ToDateTime(SelectedDate));
            foreach (var item in Vendors)
            {
                if (user.emailid == item.EmailId)
                {
                    VendorModel = item;
                }
            }
            await Task.Run(() =>
            {
                VendorsDateTime = accntService.GetVendorAvailableTimeByDate(SDate, VendorModel.Teacherid, VendorModel.ListingId);
            });
            
        }
        private async void Logout_clicked(object obj)
        {
            bool isLogoutornot = await Application.Current.MainPage.DisplayAlert("Alert", "Are you sure you want to Logout?", "Yes", "No");
            if (isLogoutornot)
            {
                Helper.RoleIdDbService.Delete();
                Helper.LoginResponse = null;
                Application.Current.MainPage = new NavigationPage(new HomePage());
            }
           
        }
    }
}
