using System;
using System.Collections.Generic;
using System.Text;

namespace HA.Model
{
	public class EndUserAppointment
	{
		public string VendorName { get; set; }
		public string VendorEmail { get; set; }
		public string VendorPhone { get; set; }
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
