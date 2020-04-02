using System;
using System.Collections.Generic;
using System.Text;

namespace HA.Model
{
   public class UserProfile
    {
        public int FKRoleId { get; set; }
        public string usertype { get; set; }
        public string emailid { get; set; }
        public string password { get; set; }
        public string ValidationMessages { get; set; }
    }

    public class LoginModel
    {
       
        public string username { get; set; }
        public string password { get; set; }
        public string grant_type { get; set; }
       // public int fkroleid { get; set; }
    }

    public class LoginResponse
    {
        public string access_token { get; set; }
        public bool Isvalid => !string.IsNullOrEmpty(access_token);
        public string token_type { get; set; }
        public long expires_in { get; set; }
        public string error { get; set; }
        public string error_description { get; set; }
    }

    public class UserProfileAPIDTO
    {

        //public long PKUserId { get; set; }
        public long FKRoleId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PhoneNo { get; set; }
        public string EmailId { get; set; }
        //public string PhotoUrl { get; set; }
        //public string Address { get; set; }
        //public string City { get; set; }
        //public string State { get; set; }
        //public string Country { get; set; }
        //public string twisid { get; set; }
        //public string twikey { get; set; }
        //public string Zipcode { get; set; }
        //public bool IsActive { get; set; }
        //public string FacebookPageID { get; set; }
        //public Nullable<bool> IsPaid { get; set; }
        //public string DisplayPhoneNo { get; set; }
        //public Nullable<System.TimeSpan> StartTime { get; set; }
        //public Nullable<System.TimeSpan> EndTime { get; set; }
        //public Nullable<long> ParentId { get; set; }
        //public string twakid { get; set; }
        //public string usertype { get; set; }

    }
}
