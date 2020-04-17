using System;
using System.Collections.Generic;
using System.Text;

namespace HA.Model
{
	public class UserIndex
	{
		public long PKUserId { get; set; }
		public string VendorName { get; set; }

		public string PhotoUrl { get; set; }

		public string ListingName { get; set; }

		public string CategoryName { get; set; }

		public string Insurance { get; set; }

		public string Description { get; set; }
		public string VendorsCount { get; set; }

		public string AvailableDate { get; set; }

		public string AvailableTime { get; set; }

		public string ToDate { get; set; }

		public string ToTime { get; set; }

		public string Location { get; set; }
		public decimal Rating { get; set; }
		public long Teacherid { get; set; }
		public long PKAvailableTeacherId { get; set; }

		public string EmailId { get; set; }
		public long FKRoleId { get; set; }
		public long CityId { get; set; }
		public long StateId { get; set; }
		public long ListingId { get; set; }
		public string CityName { get; set; }

		public double? Price { get; set; }
		public string Languages { get; set; }

		public int NoOfComments { get; set; }
	}
}
