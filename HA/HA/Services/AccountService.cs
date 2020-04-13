using HA.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace HA.Services
{
    public class AccountService
    {
        //http://localhost:5467/api/accountapi/registration/";
        private string regUrl = "https://dmnerdbooking.azurewebsites.net/api/accountapi/registration/";
        //private string loginUrl = "https://dmnerdbooking.azurewebsites.net/api/accountapi/Login/";
        private string loginUrl = "https://dmnerdbooking.azurewebsites.net/api/token";
        private string userDetailUrl = "https://dmnerdbooking.azurewebsites.net/api/accountapi/getuserdetail/";
        private string getvendorsurl = "https://dmnerdbooking.azurewebsites.net/api/homeapi/getvendors/";
        private string getvendorsdetail = "https://dmnerdbooking.azurewebsites.net/api/homeapi/venderdetail/{id}";

        private string booking = "https://dmnerdbooking.azurewebsites.net/api/homeapi/bookappoinment/";
        private string getDateTimeUrl = "https://dmnerdbooking.azurewebsites.net/api/homeapi/getvendoravailabletimebydate/";



        HttpClient client = new HttpClient();

        public UserProfile RegisterUser(UserProfile userProfile)
        {
            try
            {
                var json = JsonConvert.SerializeObject(userProfile);
                HttpContent httpcontent = new StringContent(json);
                httpcontent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync((regUrl), httpcontent).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var res = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<UserProfile>(res);

                }
                else
                {
                    throw new ApplicationException();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public LoginResponse Login(UserProfile model)
        {
            string errResponse = "";
            try
            {

                LoginModel loginModel = new LoginModel()
                {
                    username = model.emailid,
                    password = model.password,
                    grant_type = "password"
                };
                string content = "grant_type=password&username=" + model.emailid + "&password=" + model.password;
                //httpclient
                HttpClient httpClient = new HttpClient();
                //add username and password 
                HttpContent httpContent = new StringContent(content);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                //request type is POST
                var response = httpClient.PostAsync((loginUrl), httpContent).Result;
                var result = response.Content.ReadAsStringAsync().Result;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    LoginResponse model1 = JsonConvert.DeserializeObject<LoginResponse>(result);
                    return model1;
                }
                else
                {
                    var res = JsonConvert.DeserializeObject<LoginResponse>(result);
                    errResponse = res.error_description;

                }
            }
            catch (Exception)
            {
                throw;
            }
            return new LoginResponse() { error_description = errResponse };
        }

        public UserProfile GetUserDetail(string AuthorizationToken)
        {
            try
            {

                client.DefaultRequestHeaders.Add("Authorization", AuthorizationToken);

                var response = client.GetAsync(userDetailUrl).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var res = response.Content.ReadAsStringAsync().Result;
                    UserProfileAPIDTO userProfileAPIDTO = JsonConvert.DeserializeObject<UserProfileAPIDTO>(res);

                    return new UserProfile() { FKRoleId = int.Parse(userProfileAPIDTO.FKRoleId.ToString()), emailid = userProfileAPIDTO.EmailId };
                    ;

                }
                else
                {
                    throw new ApplicationException();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<UserIndex> GetVendors(string location)
        {
            string loc = "";
            if (!string.IsNullOrEmpty(location))
            {
                loc = "?location=" + location;
            }
            else
            {
                loc = string.Empty;
            }
            //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Setters.AuthenticationModel.Token);
            var response = client.GetAsync(getvendorsurl + loc).Result;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                List<UserIndex> vendors = JsonConvert.DeserializeObject<List<UserIndex>>(result);
                return vendors;

            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new ApplicationException("Could not get MoodHistory for the user");
            }
            return null;
        }
        public List<UserIndex> GetVendorslist()
        {
            //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Setters.AuthenticationModel.Token);
            var response = client.GetAsync(getvendorsurl).Result;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                List<UserIndex> vendors = JsonConvert.DeserializeObject<List<UserIndex>>(result);
                return vendors;

            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new ApplicationException("Could not get MoodHistory for the user");
            }
            return null;
        }

        public bool BookAppointment(UserQueryDTO model)
        {
            try
            {
                var json = JsonConvert.SerializeObject(model);
                HttpContent content = new StringContent(json);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpClient client = new HttpClient();
                //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Setters.AuthenticationModel.Token);
                var response = client.PostAsync((booking), content).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    bool user = JsonConvert.DeserializeObject<bool>(result);
                    return user;
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    throw new ApplicationException("Error..");
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<string> GetVendorAvailableTimeByDate(DateTime selectedDate, long teacherID, long classID)
        {
            var response = client.GetAsync(getDateTimeUrl + "?selectedDate=" + selectedDate + "&teacherID=" + teacherID + "&classID=" + classID).Result;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                List<string> vendorsdateTime = JsonConvert.DeserializeObject<List<string>>(result);
                return vendorsdateTime;

            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new ApplicationException("Could not get MoodHistory for the user");
            }
            return null;
        }
    }
}
