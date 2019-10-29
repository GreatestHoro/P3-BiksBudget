using BBCollection.HandleRecipe;
using BBCollection.BBObjects;
using BBCollection.DBConncetion;
using System;
using System.Collections.Generic;
using System.Text;

namespace BBCollection
{
    public class DatabaseConnect
    {
        public string ServerName { get; }
        public string DatabaseName { get; }
        public string DatabaseUser { get; }
        public string DatabasePassword { get; }

        public DatabaseConnect(string sName, string dBName, string dBUser, string dBPw)
        {
            ServerName = sName;
            DatabaseName = dBName;
            DatabaseUser = dBUser;
            DatabasePassword = dBPw;
        }

        private DatabaseConnect GetConnect()
        {
            return new DatabaseConnect(ServerName, DatabaseName, DatabaseUser, DatabasePassword);
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
            new InitializeDB().start(GetConnect());
        }
    }
}
