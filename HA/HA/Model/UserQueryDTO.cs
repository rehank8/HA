using System;
using System.Collections.Generic;
using System.Text;

namespace HA.Model
{
   public class UserQueryDTO
    {
		public long PKId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string EMailID { get; set; }
		public string PhoneNo { get; set; }

		public string Query { get; set; }

		public Nullable<long> TeacherID { get; set; }

		public string ReferalUserId { get; set; }

		public DateTime selelecteddate { get; set; }

		public DateTime time { get; set; }

		public Nullable<long> EndUserID { get; set; }
		public string AssignedTo { get; set; }

		public string referalphonenumber { get; set; }

	}
}
