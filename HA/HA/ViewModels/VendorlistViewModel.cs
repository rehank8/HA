using HA.Model;
using HA.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

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
        public ICommand BookNowCommand => new Command(BookNow_clicked);
        public ICommand ClosePopUpCommand => new Command(Close_clicked);
        public ICommand BookAppointmentCommand => new Command(BookAppointment_clicked);
        public event PropertyChangedEventHandler PropertyChanged;
        private string _categoryName, _currentLocation;
        string _vendorsCount,_firstname,_lastname,_email,_phone,_reason,_referralphone;
        bool _IsSubmitFormVisible;
        DateTime _selectedDate, _selectedTime,_minDate;
        int _teacherId;
        public int TeacherId
        {
            get { return _teacherId; }
            set { _teacherId = value; NotifyPropertyChanged(); }
        }
        public DateTime MinDate
        {
            get 
            {
                _minDate = DateTime.UtcNow;
                return _minDate;
            }
            set { _minDate = value; NotifyPropertyChanged(); }
        }
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set { _selectedDate = value; NotifyPropertyChanged(); }
        }
        public DateTime SelectedTime
        {
            get { return _selectedTime; }
            set { _selectedTime = value; NotifyPropertyChanged(); }
        }
        public string Firstname
        {
            get { return _firstname; }
            set { _firstname = value; NotifyPropertyChanged(); }
        }
        public string Lastname
        {
            get { return _lastname; }
            set { _lastname = value; NotifyPropertyChanged(); }
        }
        public string Email
        {
            get { return _email; }
            set { _email = value; NotifyPropertyChanged(); }
        }
        public string Phone
        {
            get { return _phone; }
            set { _phone = value; NotifyPropertyChanged(); }
        }
        public string Reason
        {
            get { return _reason; }
            set { _reason = value; NotifyPropertyChanged(); }
        }
        public string ReferralPhone
        {
            get { return _referralphone; }
            set { _referralphone = value; NotifyPropertyChanged(); }
        }
        public bool IsSubmitFormVisible
        {
            get { return _IsSubmitFormVisible ; }
            set { _IsSubmitFormVisible = value; NotifyPropertyChanged(); }
        }
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
        private UserIndex _vendor;

        public UserIndex Vendor
        {
            get { return _vendor; }
            set { _vendor = value; NotifyPropertyChanged(); }
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
        async void BookNow_clicked(object obj)
        {
            IsSubmitFormVisible = true;
            var item = obj as UserIndex;
            TeacherId = Convert.ToInt32(item.Teacherid);
        }
        private void Close_clicked()
        {
            IsSubmitFormVisible = false;
        }
        async void BookAppointment_clicked()
        {
            UserQueryDTO userQuery = new UserQueryDTO()
            {
                FirstName=Firstname,
                LastName=Lastname,
                EMailID=Email,
                PhoneNo=Phone,
                selelecteddate=SelectedDate,
                time=DateTime.UtcNow,
                referalphonenumber=ReferralPhone,
                Query=Reason,
                TeacherID=TeacherId
            };
            bool result=false;
            await Task.Run(() =>
            {
                result=accntService.BookAppointment(userQuery);
            });
            if (result)
                await Application.Current.MainPage.DisplayAlert("Message", "Your appointment is booked", "Ok");
            else
                await Application.Current.MainPage.DisplayAlert("Message", "Your appointment is not booked", "Ok");
        }
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
