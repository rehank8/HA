using HA.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace HA.Services
{
    public class RoleIdDbService
    {
        SQLiteConnection db;

        public RoleIdDbService(string dbPath)
        {
            db = new SQLiteConnection(dbPath);
            db.CreateTable<UserProfile>();
        }

        public UserProfile GetAuthUser()
        {
            var data = db.Table<UserProfile>().FirstOrDefault();
            if (data != null)
                Helper.UserProfile = data;
            return data;
        }

        public void Save(UserProfile model)
        {
            Delete();
            _ = db.InsertOrReplace(model);
            Helper.UserProfile = model;
        }

        public void Delete()
        {
            db.Execute("Delete from UserProfile");
        }
    }
}
