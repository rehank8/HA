using HA.Model;
using HA.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HA.ViewModels
{
    public class VendorlistViewModel : INotifyPropertyChanged
    {
        private AccountService accntService = new AccountService();
        public VendorlistViewModel()
        {
           
        }
        public VendorlistViewModel(string name)
        {
            CategoryName = name;
            GetVendorsList();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private string _categoryName, _currentLocation;
        string _vendorsCount;
        public string CurrentLocation
        {
            get { return _currentLocation; }
            set { _currentLocation = value; NotifyPropertyChanged(); }
        }
        public string CategoryName
        {
            get { return _categoryName; }
            set { _categoryName = value;NotifyPropertyChanged(); }
        }
        public string VendorsCount
        {
            get 
            {
                return _vendorsCount; 
            }
            set { _vendorsCount = value; NotifyPropertyChanged(); }
        }

        private List<UserIndex> _vendors;

        public List<UserIndex> Vendors
        {
            get { return _vendors; }
            set { _vendors = value; NotifyPropertyChanged(); }
        }

        async void GetVendorsList()
        {
            try
            {
                List<UserIndex> users = new List<UserIndex>();
                await Task.Run(() =>
                {
                    users = accntService.GetVendorslist();
                });

                Vendors = users.Where(x => x.CategoryName == CategoryName).ToList();
                var Count = Vendors.Count.ToString();
                VendorsCount = "(" + Count + ")";
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
