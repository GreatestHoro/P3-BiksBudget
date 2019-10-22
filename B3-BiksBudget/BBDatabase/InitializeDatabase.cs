using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net.Http;
using System.IO;
using System.Reflection;

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
                connection = new MySqlConnection(dbInfo.connectionString(true));
                connection.Open();
                string databaseExist = "CREATE DATABASE IF NOT EXISTS `"+dbInfo.databaseName+"`;";
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
                connection = new MySqlConnection(dbInfo.connectionString(false));
                connection.Open();

                String databaseSQL;
                
                Assembly assembly = Assembly.GetExecutingAssembly();

                Console.WriteLine("Test: = " + assembly.GetName().Name + ".Properties." + "CreateDatabase.sql");


                using (Stream dbRessourceStream = assembly.GetManifestResourceStream(assembly.GetName().Name + ".Properties.Resources." + "CreateDatabase"))
                {
                    using (StreamReader readRessource = new StreamReader(dbRessourceStream))
                    {
                        databaseSQL = readRessource.ReadToEnd();
                    }
                }

                new MySqlCommand(databaseSQL, connection).ExecuteNonQuery();
                
                //StreamReader sr = new StreamReader();



                //string recipeTable = "CREATE TABLE IF NOT EXISTS `Recipe` (" +
                //                     "`id` INT," +
                //                     "`name` VARCHAR(255)," +
                //                     "PRIMARY KEY(id));";

                //string ingredientTable = "CREATE TABLE IF NOT EXISTS `Ingredient` (" +
                //                     "`id` INT AUTO_INCREMENT," +
                //                     "`name` VARCHAR(255) UNIQUE," +
                //                     "PRIMARY KEY(id));";

                //string ingredientInRecipeTable = "CREATE TABLE IF NOT EXISTS `IngridientsInRecipes` (" +
                //                                 "`id` INT auto_increment," +
                //                                 "`recipe` varchar(255)," +
                //                                 "`ingridient` varchar(255)," +
                //                                 "primary key(id), " +
                //                                 "FOREIGN KEY (recipe) REFERENCES Recipes(recipe), " +
                //                                 "foreign key (ingridient) REFERENCES Ingredients(ingridient));";


                //new MySqlCommand(recipeTable, connection).ExecuteNonQuery();
                //new MySqlCommand(ingredientTable, connection).ExecuteNonQuery();
                //new MySqlCommand(ingredientInRecipeTable, connection).ExecuteNonQuery();
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