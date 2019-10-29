using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.Reflection;

namespace BBCollection.DBConncetion
{
    class InitializeDB
    {
        public void start(DatabaseConnect dbConnect)
        {
            checkDBExistence(dbConnect);
            generateWebcrawelerDatabaseTables(dbConnect);
        }

        /*
         Check if database exist, if it don't it will create it
        */
        private void checkDBExistence(DatabaseConnect dbConnect)
        {
            string databaseExist = "CREATE DATABASE IF NOT EXISTS `" + dbConnect.DatabaseName + "`;";

            new SQLConnect().NonQueryString(databaseExist, dbConnect);
        }

        private void generateWebcrawelerDatabaseTables(DatabaseConnect dbConnect)
        {
            string recipeTable =
                "CREATE TABLE IF NOT EXISTS `Recipes` (" +
                "`id` INT UNIQUE," +
                "`recipeName` VARCHAR(255)," +
                "`amountPerson` INT," +
                "`recipeDesc` VARCHAR(8000)," +
                "PRIMARY KEY(id));";

            string ingredientTable = 
                "CREATE TABLE IF NOT EXISTS `Ingredients` (" +
                "`ingredientName` VARCHAR(255) UNIQUE," +
                "PRIMARY KEY(ingredientName));";

            string ingredientInRecipeTable = 
                "CREATE TABLE IF NOT EXISTS `IngredientsInRecipe` (`id` INT AUTO_INCREMENT," +
                "`recipeID` INT," +
                "`ingredientName` varchar(255)," +
                "`amount` INT," +
                "`unit` varchar(255)," +
                "PRIMARY KEY(ID)," +
                "FOREIGN KEY(recipeID) REFERENCES RECIPES(id)," +
                "FOREIGN KEY(ingredientName) REFERENCES INGREDIENTS(ingredientName)); ";

            new SQLConnect().NonQueryString(recipeTable, dbConnect);
            new SQLConnect().NonQueryString(ingredientTable, dbConnect);
            new SQLConnect().NonQueryString(ingredientInRecipeTable, dbConnect);
        }
    }
}