using System;
using System.Collections.Generic;
using System.Text;
using BBCollection.DBConncetion;
using Microsoft.AspNet.Identity;
using MySql.Data.MySqlClient;

namespace BBCollection.DBHandling
{
    class UserHandling
    {
        public void InsertUser(string userName, string password, DatabaseInformation dbInfo)
        {
            InsertUserToDB(userName, password, dbInfo);
        }

        public bool VerifyUser(string userName, string password)
        {
            return false;
        }

        private void InsertUserToDB(string userName, string password, DatabaseInformation dbInfo)
        {
            string insertUser =
                "INSERT INTO `users`(`username`,`password`) VALUES(@Username,@Password)";
            MySqlCommand msc = new MySqlCommand(insertUser);

            msc.Parameters.AddWithValue("@Username", userName);
            msc.Parameters.AddWithValue("@Password", ConvertPasswordToHash(password));

            new SQLConnect().NonQueryMSC(msc, dbInfo);
        }

        private string GetHashedPWFromUsername(string username)
        {
            return null;
        }

        private string ConvertPasswordToHash(string password)
        {
            string pwh = new PasswordHasher().HashPassword(password);
            Console.WriteLine(new PasswordHasher().VerifyHashedPassword(pwh, password));
            return pwh;
        }
    }
}
