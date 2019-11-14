using BBCollection.DBConncetion;
using MySql.Data.MySqlClient;
using System;

namespace BBCollection.DBHandling
{
    class InitializeDB
    {
        public void start(DatabaseInformation dbInformation)
        {
            CreateDB(dbInformation);
            GenerateWebcrawelerDatabaseTables(dbInformation);
            GenerateAPIDatabaseTables(dbInformation);
        }

        public void CreateUserDB(DatabaseInformation dbInfo)
        {
            GenerateUserDatabaseTables(dbInfo);
        }

        public void CreateSallingProduct(DatabaseInformation dbInfo)
        {
            GenerateSallingProductDB(dbInfo);
        }

        public void CreateStorageDB(DatabaseInformation dbInformation)
        {
            GenerateStorageTables(dbInformation);
        }

        /*
         Check if database exist, if it don't it will create it
        */
        private void CreateDB(DatabaseInformation dbInformation)
        {
            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(dbInformation.ConnectionString(false));
                connection.Open();

                string databaseExist = "CREATE DATABASE IF NOT EXISTS `" + dbInformation.DatabaseName + "`;";

                MySqlCommand msc = new MySqlCommand(databaseExist, connection);

                msc.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                //Console.WriteLine(e);
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }

        private void GenerateWebcrawelerDatabaseTables(DatabaseInformation dbInformation)
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

            new SQLConnect().NonQueryString(recipeTable, dbInformation);
            new SQLConnect().NonQueryString(ingredientTable, dbInformation);
            new SQLConnect().NonQueryString(ingredientInRecipeTable, dbInformation);
        }

        private void GenerateAPIDatabaseTables(DatabaseInformation dbInformation)
        {
            string productTable =
                "CREATE TABLE IF NOT EXISTS `products` (" +
                "`id` VARCHAR(255) UNIQUE, " +
                "`productname` VARCHAR(255), " +
                "`amount` VARCHAR(255), " +
                "`price` DECIMAL(6,2), " +
                "`image` varchar(255), " +
                "`store` varchar(255), " +
                "PRIMARY KEY(id)); ";


            /*msc.Parameters.AddWithValue("@Id", product._id);
            msc.Parameters.AddWithValue("@ProductName", product._productName);
            msc.Parameters.AddWithValue("@Description", product._description);
            msc.Parameters.AddWithValue("@Amount", product._amount);
            msc.Parameters.AddWithValue("@AmountLeft", product._amountleft);
            msc.Parameters.AddWithValue("@TimeAdded", product._timeAdded);
            msc.Parameters.AddWithValue("@Price", product._price);
            msc.Parameters.AddWithValue("@Image", product._image);
            msc.Parameters.AddWithValue("@Store", product._storeName);*/

            new SQLConnect().NonQueryString(productTable, dbInformation);
        }

        private void GenerateUserDatabaseTables(DatabaseInformation databaseInformation)
        {
            string userTable =
                "CREATE TABLE IF NOT EXISTS `users` (" +
                "`username` VARCHAR(255) UNIQUE, " +
                "`password` VARCHAR(255), " +
                "PRIMARY KEY(username));";

            new SQLConnect().NonQueryString(userTable, databaseInformation);
        }

        private void GenerateSallingProductDB(DatabaseInformation databaseInformation)
        {
            string sallingTable =
                "CREATE TABLE IF NOT EXISTS `sallingproducts` (" +
                "`title` VARCHAR(255), " + // productname
                "`id` VARCHAR(255), " + // id
                "`prodid` VARCHAR(255) UNIQUE, " + //
                "`price` DECIMAL(6,2), " +
                "`description` VARCHAR(255), " +
                "`link` VARCHAR(255), " +
                "`img` VARCHAR(255), " +
                "PRIMARY KEY(id));";

            new SQLConnect().NonQueryString(sallingTable, databaseInformation);
        }

        private void GenerateStorageTables(DatabaseInformation databaseInformation)
        {
            string storageTable =
                "CREATE TABLE IF NOT EXISTS `userstorage` (" +
                "`id` int AUTO_INCREMENT UNIQUE, " +
                "`username` VARCHAR(255), " +
                "`prodid` VARCHAR(255), " +
                "`amountStored` int, " +
                "`timeadded` DATETIME DEFAULT CURRENT_TIMESTAMP, " +
                "primary key(id), " +
                "foreign key(username) REFERENCES users(username), " +
                "foreign key(prodid) REFERENCES products(id));";

            new SQLConnect().NonQueryString(storageTable, databaseInformation);
        }
    }
}

/*use biksbudgetdb;



SELECT*
FROM userstorage
INNER JOIN userstorage ON userstorage.prodid = products.id WHERE username = ;*/