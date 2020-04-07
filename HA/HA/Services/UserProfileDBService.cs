using HA.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace HA.Services
{
    public class UserProfileDBService
    {
        SQLiteConnection db;

        public UserProfileDBService(string dbPath)
        {
            db = new SQLiteConnection(dbPath);
            db.CreateTable<LoginResponse>();
        }

        public LoginResponse GetAuthUser()
        {
            var data = db.Table<LoginResponse>().FirstOrDefault();
            if (data != null)
                Helper.LoginResponse = data;
            return data;
        }

        public void Save(LoginResponse model)
        {
            Delete();
            _ = db.InsertOrReplace(model);
            Helper.LoginResponse = model;
        }

        public void Delete()
        {
            db.Execute("Delete from LoginResponse");
        }
    }
}
