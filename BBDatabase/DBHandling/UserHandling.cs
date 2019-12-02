using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using BBCollection.DBConncetion;
using Microsoft.AspNet.Identity;
using MySql.Data.MySqlClient;

namespace BBCollection.DBHandling
{
    public class UserHandling
    {
        /// <summary>
        /// The add method uses the InsertUserToDB method to add a user from the parameters
        /// username and password.
        /// </summary>
        /// <param name="username"></param> Username is a string containing the user's name.
        /// <param name="password"></param> The password gets parsed as a string as well, and contains
        /// the raw password that will be hashed and salted, later in the process.
        public async Task Add(string username, string password)
        {
            await InsertUserToDB(username, password);
        }

        /// <summary>
        /// The Verify method takes a username and password parameter, and first takes the username
        /// through the GetHashedPWFromUsername method, to get the correct password from the database,
        /// and then it checks if the password's match eachother by using the checkHashedMethod.
        /// </summary>
        /// <param name="username"></param> A string of the username, that the user inserts in the login.
        /// <param name="password"></param> A string of the password, that the user inserts in the login.
        /// <returns></returns>
        public async Task<bool> Verify(string username, string password)
        {
            string hashpw = await GetHashedPWFromUsername(username);
            
            return await CheckHashedPassword(password, hashpw);
        }

        /// <summary>
        /// In the InsertUserToDB method, we have a INSERT query, that specifies the two collumns `username`
        /// and `password` collumns, after that the values that have to be inserted into the collumns are specified,
        /// this is done by using @'s to pinpoint in what spots the parameters have to be placed.
        /// After that the string is saved in the MySqlCommand. This is followed by adding the parameters that 
        /// have to be added to the string.
        /// </summary>
        /// <param name="username"></param> The username parameter is the username that the user inserted in the register page.
        /// <param name="password"></param> The password parameter, is the hashed version of the password that the user inserted in the register page.
        private async Task InsertUserToDB(string username, string password)
        {
            string insertUserQuery =
                "INSERT INTO `users`(`username`,`password`) VALUES(@Username,@Password)";
            MySqlCommand msc = new MySqlCommand(insertUserQuery);

            msc.Parameters.AddWithValue("@Username", username);
            msc.Parameters.AddWithValue("@Password", await ConvertPasswordToHash(password));

            await new SQLConnect().NonQueryMSC(msc);
        }

        /// <summary>
        /// GetHashedPWFromUsername uses a SELECT query to get the password from the database.
        /// </summary>
        /// <param name="username"></param> The username parameter is the username string the user inserted in the login view.
        /// <returns></returns>
        private async Task<string> GetHashedPWFromUsername(string username)
        {
            // First the password gets initiated with null, to have something to store the password 
            // from the database in. Then the select query gets made, that will select the password
            // From the `users` collumn in the database, where the username is equal to the String username.
            string password = null; 
            string getPasswordQuery =
                "SELECT password FROM users WHERE username = @Username ;";

            // After the variables are made, we store the query in the MySqlCommand, and then specify the string
            // That have to by inserted in replacement to the palceholder of the parameter, this is specified 
            // as @Username
            MySqlCommand msc = new MySqlCommand(getPasswordQuery);
            msc.Parameters.AddWithValue("@Username", username);

            // We then use the DynamicSimpleListSQL method, inside the SQLConnect() Class, that uses the query to
            // create and return a Dataset. We store this dataset in DS and then we add the first element in the
            // dataset in our password string and return it. We can do this because the username is unique, so the amount
            // of rows that get returned, will always be 1 or 0.
            
                DataSet ds = await new SQLConnect().DynamicSimpleListSQL(msc);

                if (ds.Tables[0].Rows.Count != 0)
                {
                    password = (string)ds.Tables[0].Rows[0]["password"];
                }
            return password;

        }

        /// <summary>
        /// CheckHashedpasword uses the class PasswordHasher with the method VerifyHashedPassword, 
        /// included in the Identity class from Microsoft, to see if the password 
        /// from the database is equal to the password that have been inserted in the login.
        /// </summary>
        /// <param name="password"></param> the password parameter, is the string the the user inserted in the login page.
        /// <param name="hashedPassword"></param> the hashedpassword parameter, is the password that was retrieved from the database.
        /// <returns></returns>
        private async Task<bool> CheckHashedPassword(string password, string hashedPassword)
        {
            var result = await Task.Run(() =>
            {
                PasswordHasher pw = new PasswordHasher();
                return pw.VerifyHashedPassword(hashedPassword, password);
            });
                if (result == PasswordVerificationResult.Success)
                {
                    return true;
                }
                else
                {
                    return false;
                }
        }

        /// <summary>
        /// ConvertPasswordToHash, is a method that takes use of microsoft's PasswordHasher to Hash and Salt the password before
        /// It's stored in the database.
        /// </summary>
        /// <param name="password"></param> The password that the user inserted in the register page.
        /// <returns></returns>
        private async Task<string> ConvertPasswordToHash(string password)
        {
            return await Task.Run(() =>
            {
                return new PasswordHasher().HashPassword(password);
            });
        }
    }
}
