using HA.Model;
using HA.Services;
using MvvmHelpers;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HA.ViewModels
{
	public class MyRecentAppViewModel : BaseViewModel
	{
		AccountService accountService = new AccountService();
		public MyRecentAppViewModel(string location, List<UserIndex> vendors)
		{
			CurrentLocation = location;
			Vendors = vendors;
			GetUserAppointments();
		}
		public ICommand BookNowCommand => new Command(BookNow_clicked);
		public ICommand ClosePopUpCommand => new Command(Close_clicked);
		public ICommand BookAppointmentCommand => new Command(BookAppointment_clicked);
		public ICommand ImageCommand => new Command(Image_clicked);
		public ICommand DayCommand => new Command(Day_clicked);
		public ICommand ClearCommand => new Command(Clear_clicked);
		bool _IsSubmitFormVisible;
		string _vendorsCount, _currentLocation, _firstname, _lastname, _email, _phone, _reason, _referralphone, _categoryName;
		bool _isCalender, _isTime, _isBusy;
		DateTime _minDate, _minimumDate;
		string _selectedDate;
		string _selectedTime;
		int _teacherId;
		List<string> _VendorsdateTime;
		public bool IsCalender
		{
			get { return _isCalender; }
			set { _isCalender = value; OnPropertyChanged(); }
		}
		public bool IsBusy
		{
			get { return _isBusy; }
			set { _isBusy = value; OnPropertyChanged(); }
		}
		public bool IsTime
		{
			get { return _isTime; }
			set { _isTime = value; OnPropertyChanged(); }
		}
		public List<string> VendorsdateTime
		{
			get { return _VendorsdateTime; }
			set { _VendorsdateTime = value; OnPropertyChanged(); }
		}

		public int TeacherId
		{
			get { return _teacherId; }
			set { _teacherId = value; OnPropertyChanged(); }
		}
		public DateTime SDate
		{
			get
			{

				return _minDate;
			}
			set { _minDate = value; OnPropertyChanged(); }
		}
		public string SelectedDate
		{
			get { return _selectedDate; }
			set { _selectedDate = value.Replace(" 00:00:00", ""); OnPropertyChanged(); }
		}
		public string SelectedTime
		{
			get { return _selectedTime; }
			set { _selectedTime = value; OnPropertyChanged(); }
		}
		public string Firstname
		{
			get { return _firstname; }
			set { _firstname = value; OnPropertyChanged(); }
		}
		public string Lastname
		{
			get { return _lastname; }
			set { _lastname = value; OnPropertyChanged(); }
		}
		public string Email
		{
			get { return _email; }
			set { _email = value; OnPropertyChanged(); }
		}
		public string Phone
		{
			get { return _phone; }
			set { _phone = value; OnPropertyChanged(); }
		}
		public string Reason
		{
			get { return _reason; }
			set { _reason = value; OnPropertyChanged(); }
		}
		public string ReferralPhone
		{
			get { return _referralphone; }
			set { _referralphone = value; OnPropertyChanged(); }
		}
		public bool IsSubmitFormVisible
		{
			get
			{
				return _IsSubmitFormVisible;
			}
			set
			{
				_IsSubmitFormVisible = value; OnPropertyChanged();
			}
		}

		List<EndUserAppointment> _userAppointments;
		public List<EndUserAppointment> UserAppointments
		{
			get
			{
				return _userAppointments;
			}
			set
			{
				_userAppointments = value; OnPropertyChanged();
			}
		}
		public string CurrentLocation
		{
			get { return _currentLocation; }
			set { _currentLocation = value; OnPropertyChanged(); }
		}
		public string VendorsCount
		{
			get
			{
				return _vendorsCount;
			}
			set { _vendorsCount = value; OnPropertyChanged(); }
		}

		private List<UserIndex> _vendors;

		public List<UserIndex> Vendors
		{
			get { return _vendors; }
			set { _vendors = value; OnPropertyChanged(); }
		}
		private UserIndex _vendor;

		public UserIndex Vendor
		{
			get { return _vendor; }
			set { _vendor = value; OnPropertyChanged(); }
		}
		private EndUserAppointment _userAppointment;

		public EndUserAppointment UserAppointment
		{
			get { return _userAppointment; }
			set { _userAppointment = value; OnPropertyChanged(); }
		}
		async void GetUserAppointments()
		{
			if (Helper.CheckNetworkAccess())
			{
				await Task.Run(() =>
				{
					UserAppointments = accountService.GetUserAppointments();
				});
			}
			else
			{
				await Application.Current.MainPage.DisplayAlert("Alert","No Internet!","Ok");
			}
		}
		async void BookNow_clicked(object obj)
		{
			IsSubmitFormVisible = true;
			IsCalender = false;
			var item = obj as EndUserAppointment;
			UserAppointment = item;
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
				foreach (var item in Vendors)
				{
					if (UserAppointment.VendorEmail == item.EmailId)
					{
						Vendor = item;
					}
				}
				IsCalender = false; IsTime = true;
				SDate = (Convert.ToDateTime(SelectedDate));
				IsBusy = true;
				await Task.Run(() =>
				{
					VendorsdateTime = accountService.GetVendorAvailableTimeByDate(SDate, Vendor.Teacherid, Vendor.ListingId);
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
						result = accountService.BookAppointment(userQuery);
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
	
	}
}
