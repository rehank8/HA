using HA.DependencyInjection;
using HA.Model;
using HA.Services;
using HA.Views;
using MvvmHelpers;
using Plugin.Geolocator;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
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
			if (Helper.CheckNetworkAccess())
			{
				if (!string.IsNullOrEmpty(CurrentLocation))
				{
					await Task.Run(() =>
					{
						Vendors = accntService.GetVendors(CurrentLocation);
					});
				}

			}
			else
			{
				await Application.Current.MainPage.DisplayAlert("Alert", "No Internet!", "Ok");
			}
		}
		public bool IsgpsEnabled()
		{
			return DependencyService.Get<ICheckPermission>().IsgpsEnabled();
		}
		public bool CheckForPermission()
		{
			return DependencyService.Get<ICheckPermission>().IsLocationGrantedForApplication();
		}

		private async void GetLocation()
		{
			try
			{
				bool IsPermissionCheck = await RequestForPermission();
				if (IsPermissionCheck)
				{
					bool isLocEnabled = IsgpsEnabled();
					if (!isLocEnabled)
					{
						await Task.Run(() =>
						{
							Vendors = accntService.GetVendorslist();
						});
						await Application.Current.MainPage.DisplayAlert("Gps is not enabled", "Please turn on the Gps to accesss location", "Ok");
					}
					else
					{
						IsBusy = true;
						var locator = CrossGeolocator.Current;
						locator.DesiredAccuracy = 100;
						//position = await locator.GetLastKnownLocationAsync();
						var checkpermission = CheckForPermission();
						if (!checkpermission)
						{
							await Task.Run(() =>
							{
								Vendors = accntService.GetVendorslist();
							});
							await Application.Current.MainPage.DisplayAlert("Alert", "Permission is not granted please give permission for location", "Ok");
						}
						else
						{
							var request = new GeolocationRequest(GeolocationAccuracy.Best);
							var position = await Geolocation.GetLocationAsync(request);
							//await locator.GetLastKnownLocationAsync();//await locator.GetPositionAsync(TimeSpan.FromMilliseconds(100), null, true);
							if (position != null)
							{
								var addresses = await Geocoding.GetPlacemarksAsync(position.Latitude, position.Longitude);//await locator.GetAddressesForPositionAsync(position, null);
								var address = addresses?.FirstOrDefault();
								if (address != null)
								{
									CurrentLocation = address.Locality;

								}
								if (!string.IsNullOrEmpty(CurrentLocation))
								{
									GetVendors();
								}
							}
							else
							{
								await Task.Run(() =>
								{
									Vendors = accntService.GetVendorslist();
								});
								await Application.Current.MainPage.DisplayAlert("Alert", "Error in fetching location", "Ok");
							}
						}

					}
				}
				else
				{
					await Task.Run(() =>
					{
						Vendors = accntService.GetVendorslist();
					});
					await Application.Current.MainPage.DisplayAlert("Alert", "Permission is not granted", "Ok");
				}
				//	IsBusy = true;
				//	var locator = CrossGeolocator.Current;
				//	locator.DesiredAccuracy = 100;
				//	var position = await locator.GetLastKnownLocationAsync();
				//	position = await locator.GetPositionAsync(TimeSpan.FromMilliseconds(100), null, true);
				//	var addresses = await locator.GetAddressesForPositionAsync(position, null);
				//	var address = addresses.FirstOrDefault();
				//	if (address != null)
				//	{
				//		CurrentLocation = address.Locality;
				//	}
				//	GetVendors();
				//}
				//else
				//{
				//	await Application.Current.MainPage.DisplayAlert("", "Location permission is required", "Ok");
				//}
			}
			catch (FeatureNotSupportedException fnsEx)
			{
				// Handle not supported on device exception
			}
			catch (FeatureNotEnabledException fneEx)
			{
				await Application.Current.MainPage.DisplayAlert("Alert", "Location is not enabled", "Ok");
			}
			catch (PermissionException pEx)
			{
				// Handle permission exception
			}
			catch (Exception ex)
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
			if (VendorModel != null)
			{
				await Task.Run(() =>
				{
					VendorsDateTime = accntService.GetVendorAvailableTimeByDate(SDate, VendorModel.Teacherid, VendorModel.ListingId);
				});
			}
		}
		private async void Logout_clicked(object obj)
		{
			bool isLogoutornot = await Application.Current.MainPage.DisplayAlert("Alert", "Are you sure you want to Logout?", "Yes", "No");
			if (isLogoutornot)
			{
				Helper.RoleIdDbService.Delete();
				Helper.UserProfile = null;
				Helper.LoginResponse = null;
				Application.Current.MainPage = new NavigationPage(new HomePage());
			}

		}
		async Task<bool> RequestForPermission()
		{
			PermissionStatus permissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
			try
			{
				if (PermissionStatus.Granted != permissionStatus)
				{
					var response = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
					if (Device.RuntimePlatform == Device.iOS)
					{
						var calstatus = response[Permission.Location];
						if (PermissionStatus.Granted != calstatus)
							return false;
					}
					else
					{
						var status = response[Permission.Location];
						if (PermissionStatus.Granted != status)
							return false;
					}
				}
			}
			catch (Exception ex)
			{

			}
			return true;

		}
	}
}
