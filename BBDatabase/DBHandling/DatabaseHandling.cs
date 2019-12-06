using BBCollection.DBConncetion;
using MySql.Data.MySqlClient;
using System;
using System.Threading.Tasks;

namespace BBCollection.DBHandling
{
    public class DatabaseHandling
    {
        ConnectionSettings connectionSettings = new ConnectionSettings();

        /// <summary>
        /// In the start() method, every method in the DatabaseHandler
        /// gets calls, this makes it easier to initialize the database,
        /// since all the tables have to exist for the program to work 
        /// perfectly.
        /// </summary>
        public async Task Start()
        {
            CreateCoreDatabase();
            await GenerateWebcrawelerDatabaseTables();
            await GenerateAPIDatabaseTables();
            await GenerateUserDatabaseTables();
            await GenerateSallingProductDB();
            await GenerateStorageTables();
            await GenerateShoppingListTables();
            await UpdateProductTable();
            await UpdateRecipesTable();
        }

        /// <summary>
        /// CreateCoreDatabase() creates the database if it doesn't exist.
        /// </summary>
        private void CreateCoreDatabase()
        {
            MySqlConnection connection = null;
            try
            {
                DatabaseInformation databaseInformation = new DatabaseInformation();
                // Because the parameter in ConnectionString is false, the connection string
                // doesn't contain a database parameter.
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

        // The following methods contains the sql queries for the creation of the tables in the database.
        #region Table queries
        private async Task GenerateWebcrawelerDatabaseTables()
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

            await new SQLConnect().NonQueryString(recipeTable);
            await new SQLConnect().NonQueryString(ingredientTable);
            await new SQLConnect().NonQueryString(ingredientInRecipeTable);
        }

        private async Task GenerateAPIDatabaseTables()
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


            await new SQLConnect().NonQueryString(productTable);
            await new SQLConnect().NonQueryString(productInIngredientQuery);
        }

        private async Task GenerateUserDatabaseTables()
        {
            string userTable =
                "CREATE TABLE IF NOT EXISTS `users` (" +
                "`username` VARCHAR(255) UNIQUE, " +
                "`password` VARCHAR(255), " +
                "PRIMARY KEY(username));";

            await new SQLConnect().NonQueryString(userTable);
        }

        private async Task GenerateSallingProductDB()
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

            await new SQLConnect().NonQueryString(sallingTable);
        }

        private async Task GenerateStorageTables()
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

            await new SQLConnect().NonQueryString(storageTable);
        }

        private async Task GenerateShoppingListTables()
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

            await new SQLConnect().NonQueryString(shoppingListTables);
        }

        private async Task UpdateProductTable()
        {
            string newCollumnQuery =
                "ALTER TABLE `products` ADD `ingredient_reference` varchar(255)";

            await new SQLConnect().NonQueryString(newCollumnQuery);
        }

        private async Task UpdateRecipesTable()
        {
            string newColumnQuery =
                "ALTER TABLE `recipes` ADD `recipe_totalprice` decimal(6,2)";

            await new SQLConnect().NonQueryString(newColumnQuery);
        }
        #endregion
    }
}
