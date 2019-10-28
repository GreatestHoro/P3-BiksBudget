using BBDatabase.HandleRecipe;
using BBDatabase.DBObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace BBDatabase
{
    class Connect
    {
        private string ServerName { get; }
        private string DatabaseName { get; }
        private string DatabaseUser { get; }
        private string DatabasePassword { get; }

        public Connect(string sName, string dBName, string dBUser, string dBPw)
        {
            ServerName = sName;
            DatabaseName = dBName;
            DatabaseUser = dBUser;
            DatabasePassword = dBPw;
        }

        private Connect GetConnect()
        {
            return new Connect(ServerName, DatabaseName, DatabaseUser, DatabasePassword);
        }

        public string ConnectionString(bool withDB)
        {
            if (withDB)
            {
                return @"server=" + ServerName + ";database=" + DatabaseName + ";userid=" + DatabaseUser + ";password=" + DatabasePassword;
            }
            else
            {
                return @"server=" + ServerName + ";userid=" + DatabaseUser + ";password=" + DatabasePassword;
            }
        }

        public void AddRecipe(Recipe recipe)
        {
            new RecipeToDatabase().CombineRecipe(recipe, GetConnect());
        }

        public void GetRecipe()
        {

        }

        public void InitializeDatabase()
        {

        }
    }
}
