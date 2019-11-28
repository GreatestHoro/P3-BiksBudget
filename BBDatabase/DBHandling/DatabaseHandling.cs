using BBCollection.DBConncetion;
using MySql.Data.MySqlClient;
using System;

namespace BBCollection.DBHandling
{
    public class DatabaseHandling
    {
        ConnectionSettings connectionSettings = new ConnectionSettings();
        public void Start()
        {
            CreateDB();
            GenerateWebcrawelerDatabaseTables();
            GenerateAPIDatabaseTables();
            UpdateProductTable();
            GenerateUserDatabaseTables();
            GenerateSallingProductDB();
            GenerateStorageTables();
            GenerateShoppingListTables();
        }

        /*
         Check if database exist, if it don't it will create it
        */
        private void CreateDB()
        {
            MySqlConnection connection = null;
            try
            {
                DatabaseInformation databaseInformation = new DatabaseInformation();
                connection = new MySqlConnection(databaseInformation.ConnectionString(false));
                connection.Open();

                string databaseExist = "CREATE DATABASE IF NOT EXISTS `"+ connectionSettings._DatabaseName +"`;";

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

        private void GenerateWebcrawelerDatabaseTables()
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
                "`id` int auto_increment unique," +
                "`ingredientName` VARCHAR(255) UNIQUE," +
                "PRIMARY KEY(id));";

            string ingredientInRecipeTable =
                "CREATE TABLE IF NOT EXISTS `IngredientsInRecipe` (`id` INT AUTO_INCREMENT," +
                "`recipeID` INT," +
                "`ingredientID` int," +
                "`amount` INT," +
                "`unit` varchar(255)," +
                "PRIMARY KEY(ID)," +
                "FOREIGN KEY(recipeID) REFERENCES RECIPES(id)," +
                "FOREIGN KEY(ingredientID) REFERENCES INGREDIENTS(id)); ";

            new SQLConnect().NonQueryString(recipeTable);
            new SQLConnect().NonQueryString(ingredientTable);
            new SQLConnect().NonQueryString(ingredientInRecipeTable);
        }

        private void GenerateAPIDatabaseTables()
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

            string productInIngredientQuery =
                "CREATE TABLE IF NOT EXISTS `products_matching_ingredients`(" +
                "`ingredient_id` int, " +
                "`product_id` varchar(255), " +
                "foreign key(ingredient_id) references ingredients(id)," +
                "foreign key(product_id) references products(id))";


            new SQLConnect().NonQueryString(productTable);
            new SQLConnect().NonQueryString(productInIngredientQuery);
        }

        private void GenerateUserDatabaseTables()
        {
            string userTable =
                "CREATE TABLE IF NOT EXISTS `users` (" +
                "`username` VARCHAR(255) UNIQUE, " +
                "`password` VARCHAR(255), " +
                "PRIMARY KEY(username));";

            new SQLConnect().NonQueryString(userTable);
        }

        private void GenerateSallingProductDB()
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

            new SQLConnect().NonQueryString(sallingTable);
        }

        private void GenerateStorageTables()
        {
            string storageTable =
                "CREATE TABLE IF NOT EXISTS `userstorage` (" +
                "`username` VARCHAR(255), " +
                "`prodid` VARCHAR(255), " +
                "`custom_name` VARCHAR(255), " +
                "`amountStored` int, " +
                "`timeadded` DATETIME DEFAULT CURRENT_TIMESTAMP, " +
                "`state` varchar(255), " +
                "foreign key(username) REFERENCES users(username), " +
                "foreign key(prodid) REFERENCES products(id));";

            new SQLConnect().NonQueryString(storageTable);
        }

        private void GenerateShoppingListTables()
        {
            string shoppingListTables =
                "CREATE TABLE IF NOT EXISTS `shoppinglists`( " +
                "`id` int auto_increment unique, " +
                "`username` varchar(255), " +
                "`shoppinglist_name` varchar(255), " +
                "`product_id` varchar(255), " +
                "`amount` int, " +
                "primary key(id), " +
                "foreign key(username) references users(username)," +
                "foreign key(product_id) references products(id));";

            new SQLConnect().NonQueryString(shoppingListTables);
        }

        private void UpdateProductTable()
        {
            string newCollumnQuery =
                "ALTER TABLE `products` ADD `ingredient_reference` varchar(255)";

            new SQLConnect().NonQueryString(newCollumnQuery);
        }
    }
}
