using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace HA.Model
{
	public class EndUserAppointment:BaseViewModel
	{
		public string VendorName { get; set; }
		public string VendorEmail { get; set; }
		string _vendorPhone;
		public string VendorPhone
		{
			get
			{
				var e=Regex.Replace(_vendorPhone, ".{3}", "$0-");
				return e;
			}
			set 
			{
				_vendorPhone = value;
			}
		}
		public string BusinessName { get; set; }
		public string AreaName { get; set; }
		public string CategoryName { get; set; }
		public long UserQueryID { get; set; }
		public DateTime selecteddate { get; set; }
		public DateTime selectedtime { get; set; }
		public string Query { get; set; }
		public DateTime BookingDateTime { get; set; }
	}
}
