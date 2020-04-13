using HA.Model;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Plugin.Calendar.Models;

namespace HA.ViewModels
{
    public class CalenderViewModel : BaseViewModel
    {
        public CalenderViewModel(List<string> Vendorsdatetime,UserIndex Vendor,UserQueryDTO userQuery)
        {
            UQuery = userQuery;
            VendorsDateTime = Vendorsdatetime;
            VendorModel = Vendor;
            GetData();
        }
        public EventCollection Events
        {
            get; set;
        }
        private List<string> _vendorsDateTime;

        public List<string> VendorsDateTime
        {
            get { return _vendorsDateTime; }
            set { _vendorsDateTime = value;OnPropertyChanged(); }
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
        void GetData()
        {
           

            foreach (var item in VendorsDateTime)
            {
                Events = new EventCollection
                {
                    [UQuery.selelecteddate] = new List<UserIndex> { new UserIndex { AvailableTime=VendorModel.AvailableTime} }
                };
            }
            //Events = new EventCollection
            //{

            //[DateTime.Now] =new List<EventModel> {new EventModel{ Name="N",Description="D"} }                
            //            [DateTime.Now] = new List<EventModel>
            //{
            //    new EventModel { Name = "Cool event1", Description = "This is Cool event1's description!" },
            //    new EventModel { Name = "Cool event2", Description = "This is Cool event2's description!" }
            //},
            //            // 5 days from today
            //            [DateTime.Now.AddDays(5)] = new List<EventModel>
            //{
            //    new EventModel { Name = "Cool event3", Description = "This is Cool event3's description!" },
            //    new EventModel { Name = "Cool event4", Description = "This is Cool event4's description!" }
            //},
            //            // 3 days ago
            //            [DateTime.Now.AddDays(-3)] = new List<EventModel>
            //{
            //    new EventModel { Name = "Cool event5", Description = "This is Cool event5's description!" }
            //},
            //            // custom date
            //            [new DateTime(2020, 3, 16)] = new List<EventModel>
            //{
            //    new EventModel { Name = "Cool event6", Description = "This is Cool event6's description!" }
            //}
            //};
        }

    }
}
