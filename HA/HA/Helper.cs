using HA.Model;
using HA.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HA
{
   public class Helper
    {
        static UserProfileDBService _db1;
        public static string databasepath1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "HA.db3");

        public static UserProfileDBService UserProfileDBService
        {
            get
            {
                if (_db1 == null)
                {
                    _db1 = new UserProfileDBService(databasepath1);

                }
                return _db1;
            }
        }
        public static LoginResponse LoginResponse
        {
            get;
            set;
        }

    }
}
