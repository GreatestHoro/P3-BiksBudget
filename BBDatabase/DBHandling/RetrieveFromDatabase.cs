using BBCollection.BBObjects;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace BBCollection.HandleRecipe
{
    class RetrieveFromDatabase
    {
        public List<Recipe> RetrieveRecipeList(string recipeName, DatabaseConnect dbConnect)
        {
            MySqlConnection connection = null;
            List<Recipe> recipeList = new List<Recipe>();

            try
            {
                connection = new MySqlConnection(dbConnect.ConnectionString(true));
                connection.Open();

                string recipesQuery = "SELECT * FROM recipes WHERE recipeName LIKE @RecipeName";

                MySqlCommand msc = new MySqlCommand(recipesQuery, connection);

                msc.Parameters.AddWithValue("@RecipeName", "%" + recipeName + "%");

                MySqlDataReader msdr = msc.ExecuteReader();

                while (msdr.Read())
                {

                    Recipe recipe = new Recipe(msdr.GetInt32("id"), msdr.GetString("recipeName"), msdr.GetString("recipeDesc"),
                                               GetIngredientsFromRecipeID(msdr.GetInt32("id"), dbConnect), msdr.GetInt32("amountPerson"));

                    recipeList.Add(recipe);
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

            return recipeList;
        }

        public List<Ingredient> GetIngredientsFromRecipeID(int recipeID, DatabaseConnect dbConnect)
        {
            MySqlConnection connection = null;
            List<Ingredient> ingredients = new List<Ingredient>();

            try
            {
                connection = new MySqlConnection(dbConnect.ConnectionString(true));
                connection.Open();

                string ingredientsToRecipeQuery = "SELECT * FROM IngredientsInRecipe WHERE recipeID = @RecipeID";
                MySqlCommand msc = new MySqlCommand(ingredientsToRecipeQuery, connection);
                msc.Parameters.AddWithValue("@RecipeID", recipeID);
                MySqlDataReader msdr = msc.ExecuteReader();

                while (msdr.Read())
                {
                    Ingredient ingredient = new Ingredient(msdr.GetString("ingredientName"), msdr.GetString("unit"), msdr.GetInt32("amount"));
                    ingredients.Add(ingredient);
                }

            }
            catch (MySqlException)
            {

            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }

            return ingredients;
        }
    }
}
