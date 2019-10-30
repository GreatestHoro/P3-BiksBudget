using MySql.Data.MySqlClient;
using System;

namespace BBCollection.DBConncetion
{
    class InitializeDB
    {
        public void start(DatabaseConnect dbConnect)
        {
            CheckDBExistence(dbConnect);
            GenerateWebcrawelerDatabaseTables(dbConnect);
            GenerateAPIDatabaseTables(dbConnect);
        }

        /*
         Check if database exist, if it don't it will create it
        */
        private void CheckDBExistence(DatabaseConnect dbConnect)
        {


            MySqlConnection connection = null;
            try
            {

                connection = new MySqlConnection(dbConnect.ConnectionString(false));
                connection.Open();

                string databaseExist = "CREATE DATABASE IF NOT EXISTS `" + dbConnect.DatabaseName + "`;";

                MySqlCommand msc = new MySqlCommand(databaseExist, connection);

                msc.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }


        }

        private void GenerateWebcrawelerDatabaseTables(DatabaseConnect dbConnect)
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

        private void GenerateAPIDatabaseTables(DatabaseConnect dbConnect)
        {
            string productTable =
                "CREATE TABLE IF NOT EXISTS `products` (" +
                "`id` INT AUTO_INCREMENT," +
                "`ean` VARCHAR(255) UNIQUE," +
                "`productName` VARCHAR(255)," +
                "`productName2` VARCHAR(255)," +
                "`price` DECIMAL(6,2)," +
                "`productHierarchyID` INT," +
                "PRIMARY KEY(id));";

            new SQLConnect().NonQueryString(productTable, dbConnect);
        }
    }
}