using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using HA.DependencyInjection;
using HA.Droid.DependencyServices;
using Xamarin.Forms;

[assembly: Dependency(typeof(CheckForPermission))]
namespace HA.Droid.DependencyServices
{
	public class CheckForPermission : ICheckPermission
	{
		public bool IsgpsEnabled()
		{
			var context = MainActivity.AndroidContext;
			LocationManager lm = (LocationManager)(LocationManager)context.GetSystemService(Context.LocationService);
			bool gps_enabled = false;

			try
			{
				gps_enabled = lm.IsProviderEnabled(LocationManager.GpsProvider);
			}
			catch (Exception) { }

			return gps_enabled;
		}

		public bool IsLocationGrantedForApplication()
		{
			var context = MainActivity.AndroidContext;
			return ContextCompat.CheckSelfPermission(context, Manifest.Permission.AccessFineLocation) == Permission.Granted;
		}
	}
}