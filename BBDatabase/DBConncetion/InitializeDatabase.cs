using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.Reflection;
using BBDatabase.DBConnection;

namespace B3_BiksBudget.BBDatabase
{
    class InitializeDatabase
    {
        public void start(DatabaseInformation dbInfo)
        {
            checkDBExistence(dbInfo);
            generateDatabaseTables(dbInfo);
        }

        /*
         Check if database exist, if it don't it will create it
        */
        private void checkDBExistence(DatabaseInformation dbInfo)
        {
            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(dbInfo.connectionString(false));
                connection.Open();
                string databaseExist = "CREATE DATABASE IF NOT EXISTS `"+dbInfo.DatabaseName+"`;";
                new MySqlCommand(databaseExist, connection).ExecuteNonQuery();
            }
            catch (MySqlException)
            {
                // Make exception
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }

        private void generateDatabaseTables(DatabaseInformation dbInfo)
        {
            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(dbInfo.connectionString(true));
                connection.Open();

                string recipeTable = "CREATE TABLE IF NOT EXISTS `Recipes` (" +
                                     "`id` INT UNIQUE," +
                                     "`recipeName` VARCHAR(255)," +
                                     "`amountPerson` INT," +
                                     "`recipeDesc` VARCHAR(8000)," +
                                     "PRIMARY KEY(id));";

                string ingredientTable = "CREATE TABLE IF NOT EXISTS `Ingredients` (" +
                                     "`ingredientName` VARCHAR(255) UNIQUE," +
                                     "PRIMARY KEY(ingredientName));";

                string ingredientInRecipeTable = "CREATE TABLE IF NOT EXISTS `IngredientsInRecipe` (`id` INT AUTO_INCREMENT," +
                                                 "`recipeID` INT," +
                                                 "`ingredientName` varchar(255)," +
                                                 "`amount` INT," +
                                                 "`unit` varchar(255)," +
                                                 "PRIMARY KEY(ID)," +
                                                 "FOREIGN KEY(recipeID) REFERENCES RECIPES(id)," +
                                                 "FOREIGN KEY(ingredientName) REFERENCES INGREDIENTS(ingredientName)); ";


                new MySqlCommand(recipeTable, connection).ExecuteNonQuery();
                new MySqlCommand(ingredientTable, connection).ExecuteNonQuery();
                new MySqlCommand(ingredientInRecipeTable, connection).ExecuteNonQuery();
            }
            catch (MySqlException)
            {
                // Make exception
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
        } 
    }
}