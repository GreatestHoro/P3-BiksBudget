using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using BBCollection.DBConncetion;
using Microsoft.AspNet.Identity;
using MySql.Data.MySqlClient;

namespace BBCollection.DBHandling
{
    class UserHandling
    {
        public void InsertUser(string username, string password, DatabaseInformation dbInformation)
        {
            InsertUserToDB(username, password, dbInformation);
        }

        public bool VerifyUser(string username, string password, DatabaseInformation dbInformation)
        {

            return CheckHashedPassword(password, GetHashedPWFromUsername(username, dbInformation));
        }

        private void InsertUserToDB(string username, string password, DatabaseInformation dbInformation)
        {
            string insertUserQuery =
                "INSERT INTO `users`(`username`,`password`) VALUES(@Username,@Password)";
            MySqlCommand msc = new MySqlCommand(insertUserQuery);

            msc.Parameters.AddWithValue("@Username", username);
            msc.Parameters.AddWithValue("@Password", ConvertPasswordToHash(password));

            new SQLConnect().NonQueryMSC(msc, dbInformation);
        }

        private string GetHashedPWFromUsername(string username, DatabaseInformation dbInformation)
        {
            string password = null;
            string getPasswordQuery =
                "SELECT password FROM users WHERE username = @Username ;";

            MySqlCommand msc = new MySqlCommand(getPasswordQuery);
            msc.Parameters.AddWithValue("@Username", username);

            DataSet ds = new SQLConnect().DynamicSimpleListSQL(msc, dbInformation);

            if(ds.Tables[0].Rows.Count != 0) { 
               password = (string) ds.Tables[0].Rows[0]["password"];
            }
            
            return password;
        }

        private bool CheckHashedPassword(string password, string hashedPassword)
        {
            if(new PasswordHasher().VerifyHashedPassword(hashedPassword, password) == PasswordVerificationResult.Success)
            {
                return true;
            } else
            {
                return false;
            }
        }

        private string ConvertPasswordToHash(string password)
        {
            return new PasswordHasher().HashPassword(password);
        }
    }
}
