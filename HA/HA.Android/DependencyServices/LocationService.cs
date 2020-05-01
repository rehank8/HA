using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Location;
using Android.Gms.Common.Apis;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace HA.Droid.DependencyServices
{
	public class LocationService
	{
        public async static void AskForLocation()
        {
            if (!CheckIsGPSEnabled())
            {
                Int64
        interval = 1000 * 60 * 1,
        fastestInterval = 1000 * 50;

                try
                {
                    GoogleApiClient
                        googleApiClient = new GoogleApiClient.Builder(MainActivity.AndroidContext)
                            .AddApi(LocationServices.API)
                            .Build();

                    googleApiClient.Connect();

                    LocationRequest
                        locationRequest = LocationRequest.Create()
                            .SetPriority(LocationRequest.PriorityBalancedPowerAccuracy)
                            .SetInterval(interval)
                            .SetFastestInterval(fastestInterval);

                    LocationSettingsRequest.Builder
                        locationSettingsRequestBuilder = new LocationSettingsRequest.Builder()
                            .AddLocationRequest(locationRequest);

                    locationSettingsRequestBuilder.SetAlwaysShow(false);

                    LocationSettingsResult
                        locationSettingsResult = await LocationServices.SettingsApi.CheckLocationSettingsAsync(
                            googleApiClient, locationSettingsRequestBuilder.Build());

                    if (locationSettingsResult.Status.StatusCode == LocationSettingsStatusCodes.ResolutionRequired)
                    {
                        locationSettingsResult.Status.StartResolutionForResult(MainActivity.CurrentActivity, 0);
                    }
                }
                catch (Exception)
                {
                    // Log exception
                }
            }
        }


        public static bool CheckIsGPSEnabled()
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
    }
}