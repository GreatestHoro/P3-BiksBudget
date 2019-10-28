using B3_BiksBudget.BBObjects;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace B3_BiksBudget.BBDatabase
{
    class RecipeHandling
    {
        public void AddRecipe(Recipe recipe, DatabaseInformation dbInfo)
        {
            AddRecipeToDatabase(recipe, dbInfo);
            AddIngredientsToDatabase(recipe._ingredientList, dbInfo);
            CombineRecipeAndIngredient(recipe, dbInfo);
        }

        private void AddRecipeToDatabase(Recipe recipe, DatabaseInformation dbInfo)
        {
            string recipeQuery = "INSERT INTO `Recipes`(`id`,`recipeName`,`amountPerson`,`recipeDesc`) VALUES(@RecipeID,@RecipeName,@RecipePersons,@RecipeDescription);";
            MySqlCommand msc = new MySqlCommand(recipeQuery);

            msc.Parameters.AddWithValue("@RecipeID", recipe._recipeID);
            msc.Parameters.AddWithValue("@RecipeName", recipe._Name);
            msc.Parameters.AddWithValue("@RecipePersons", recipe._PerPerson);
            msc.Parameters.AddWithValue("@RecipeDescription", recipe._description);

            ConnectionHandlingNonQuery(msc, dbInfo);
        }

        private void AddIngredientsToDatabase(List<Ingredient> ingredients, DatabaseInformation dbInfo)
        {
            foreach (Ingredient ingredient in ingredients)
            {
                if (!IngredientExist(ingredient, dbInfo))
                {
                    AddIngredientToDatabase(ingredient, dbInfo);
                }
            }
        }

        private void AddIngredientToDatabase(Ingredient ingredient, DatabaseInformation dbInfo)
        {
            string IngredientToDatabase = "INSERT INTO `Ingredients` (`ingredientName`) VALUES (@Ingredient);";
            MySqlCommand msc = new MySqlCommand(IngredientToDatabase);

            msc.Parameters.AddWithValue("@Ingredient", ingredient._IngredientName);

            ConnectionHandlingNonQuery(msc, dbInfo);
        }

        private bool IngredientExist(Ingredient ingredient, DatabaseInformation dbInfo)
        {
            bool exist = false;
            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(dbInfo.connectionString(true));
                connection.Open();

                Console.WriteLine(ingredient._IngredientName);

                string ingredientExist = "SELECT * FROM `Ingredients` WHERE `ingredientName` = @Ingredient;";
                MySqlCommand msc = new MySqlCommand(ingredientExist, connection);

                msc.Parameters.AddWithValue("@Ingredient", ingredient._IngredientName);

                MySqlDataAdapter ingredientsFromQuery = new MySqlDataAdapter(msc);
                DataSet checkForIngredient = new DataSet();
                ingredientsFromQuery.Fill(checkForIngredient);

                if (checkForIngredient.Tables[0].Rows.Count == 1)
                {
                    exist = true;
                }
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
            return exist;
        }

        private void CombineRecipeAndIngredient(Recipe recipe, DatabaseInformation dbInfo)
        {
                foreach (Ingredient ingredient in recipe._ingredientList)
                {
                    string addIngredientReferance = "INSERT INTO `IngredientsInRecipe` (`recipeID`,`ingredientName`,`amount`,`unit`)" +
                                                    "VALUES(@RecipeID,@Ingredient,@Amount,@Unit);";

                    MySqlCommand msc = new MySqlCommand(addIngredientReferance);

                    msc.Parameters.AddWithValue("@RecipeID", recipe._recipeID);
                    msc.Parameters.AddWithValue("@Ingredient", ingredient._IngredientName);
                    msc.Parameters.AddWithValue("@Amount", ingredient._Amount);
                    msc.Parameters.AddWithValue("@Unit", ingredient._unit);

                    ConnectionHandlingNonQuery(msc, dbInfo);
                }
        }

        private void ConnectionHandlingNonQuery(MySqlCommand msc, DatabaseInformation dbInfo)
        {
            MySqlConnection connection = null;

            try
            {
                connection = new MySqlConnection(dbInfo.connectionString(true));
                connection.Open();

                msc.Connection = connection;
                msc.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                Console.Write(e);
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
