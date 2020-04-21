using HA.Model;
using HA.Services;
using HA.Views;
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
        public VendorlistViewModel(string name, string location)
        {
            IsCalender = false;
            CurrentLocation = location;
            CategoryName = name;
            GetVendorsList();
        }
        public ICommand BookNowCommand => new Command(BookNow_clicked);
        public ICommand ClosePopUpCommand => new Command(Close_clicked);
        public ICommand BookAppointmentCommand => new Command(BookAppointment_clicked);
        public ICommand ImageCommand => new Command(Image_clicked);
        public ICommand DayCommand => new Command(Day_clicked);
        public ICommand ClearCommand => new Command(Clear_clicked);
        //public ICommand SelectedTimeCommand => new Command(SelectedTime_clicked);
        public event PropertyChangedEventHandler PropertyChanged;
        private string _categoryName, _currentLocation;
        string _vendorsCount, _firstname, _lastname, _email, _phone, _reason, _referralphone;
        bool _IsSubmitFormVisible, _isCalender, _isTime, _isBusy;
        DateTime _minDate,_minimumDate;
        string _selectedDate;
        string _selectedTime;
        int _teacherId;
        List<string> _VendorsdateTime;
        public bool IsCalender
        {
            get { return _isCalender; }
            set { _isCalender = value; NotifyPropertyChanged(); }
        }
        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; NotifyPropertyChanged(); }
        }
        public bool IsTime
        {
            get { return _isTime; }
            set { _isTime = value; NotifyPropertyChanged(); }
        }
        public List<string> VendorsdateTime
        {
            get { return _VendorsdateTime; }
            set { _VendorsdateTime = value; NotifyPropertyChanged(); }
        }

        public int TeacherId
        {
            get { return _teacherId; }
            set { _teacherId = value; NotifyPropertyChanged(); }
        }
        public DateTime SDate
        {
            get
            {

                return _minDate;
            }
            set { _minDate = value; NotifyPropertyChanged(); }
        }
        public string SelectedDate
        {
            get { return _selectedDate; }
            set { _selectedDate = value.Replace(" 00:00:00",""); NotifyPropertyChanged(); }
        }
        public string SelectedTime
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
            get { return _IsSubmitFormVisible; }
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
            set { _categoryName = value; NotifyPropertyChanged(); }
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
                IsBusy = true;
                List<UserIndex> users = new List<UserIndex>();
                if (string.IsNullOrEmpty(CurrentLocation))
                {
                    await Task.Run(() =>
                    {
                        users = accntService.GetVendorslist();
                    });

                    Vendors = users.Where(x => x.CategoryName == CategoryName).ToList();
                }
                else
                {
                    await Task.Run(() =>
                    {
                        users = accntService.GetVendors(CurrentLocation);
                    });
                    Vendors = users.Where(x => x.CategoryName == CategoryName).ToList();
                }
                var Count = Vendors.Count.ToString();
                VendorsCount = "(" + Count + ")";
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                IsBusy = false;
            }
        }
        async void BookNow_clicked(object obj)
        {
            IsSubmitFormVisible = true;
            IsCalender = false;
            var item = obj as UserIndex;
            Vendor = item;
            TeacherId = Convert.ToInt32(item.Teacherid);
        }
        private void Close_clicked()
        {
            IsSubmitFormVisible = false;
        }
        void Image_clicked()
        {
            IsCalender = true;
        }
        async void Day_clicked()
        {
            try
            {
                IsCalender = false; IsTime = true;
                SDate = (Convert.ToDateTime(SelectedDate));
                IsBusy = true;
                await Task.Run(() =>
                {
                    VendorsdateTime = accntService.GetVendorAvailableTimeByDate(SDate, Vendor.Teacherid, Vendor.ListingId);
                });
                VendorsdateTime = VendorsdateTime.Select(t => t.Replace("737527.", "")).ToList();
                if (VendorsdateTime.Count == 0)
                {
                    VendorsdateTime = new List<string>()
                {
                    "No Available Time"
                };
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                IsBusy = false;
            }
        }
        async void BookAppointment_clicked()
        {
            try
            {
                if (Helper.CheckNetworkAccess())
                {
                    if (string.IsNullOrEmpty(Firstname) || string.IsNullOrEmpty(Lastname) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Phone))
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Fields can't be empty", "Ok");
                    }
                    else
                    {
                        UserQueryDTO userQuery = new UserQueryDTO()
                        {
                            FirstName = Firstname,
                            LastName = Lastname,
                            EMailID = Email.Trim(),
                            PhoneNo = Phone,
                            selelecteddate = SDate,
                            time = Convert.ToDateTime(SelectedTime),
                            referalphonenumber = ReferralPhone,
                            Query = Reason,
                            TeacherID = TeacherId
                        };

                        bool result = false;
                        IsBusy = true;
                        await Task.Run(() =>
                        {
                            result = accntService.BookAppointment(userQuery);
                        });
                        if (result)
                        {
                            await Application.Current.MainPage.DisplayAlert("Message", "Your Appointment is booked", "Ok");
                            Clear_clicked();
                            SelectedDate = "";
                            IsSubmitFormVisible = false;
                        }


                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Message", "Error in booking Appointment", "Ok");
                            Clear_clicked();
                            SelectedDate = "";
                            IsSubmitFormVisible = false;
                        }
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Alert", "No Internet!", "Ok");
                }
               
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                IsBusy = false;
            }
        }
        private void Clear_clicked()
        {
            Firstname = string.Empty;
            Lastname = string.Empty;
            Email = string.Empty;
            Phone = string.Empty;
            SelectedDate = string.Empty;
            SelectedTime = string.Empty;
            ReferralPhone = string.Empty;
            Reason = string.Empty;
        }
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
