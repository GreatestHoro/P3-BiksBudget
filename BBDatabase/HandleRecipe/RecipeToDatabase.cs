using BBCollection;
using BBCollection.BBObjects;
using BBCollection.DBConncetion;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace BBCollection.HandleRecipe
{
    class RecipeToDatabase
    {
        public void CombineRecipe(Recipe recipe, DatabaseConnect dbConnect)
        {
            AddRecipeToDatabase(recipe, dbConnect);
            AddIngredientsToDatabase(recipe._ingredientList, dbConnect);
            CombineRecipeAndIngredient(recipe, dbConnect);
        }

        private void AddRecipeToDatabase(Recipe recipe, DatabaseConnect dbConnect)
        {
            string recipeQuery = "INSERT INTO `Recipes`(`id`,`recipeName`,`amountPerson`,`recipeDesc`) VALUES(@RecipeID,@RecipeName,@RecipePersons,@RecipeDescription);";
            MySqlCommand msc = new MySqlCommand(recipeQuery);

            msc.Parameters.AddWithValue("@RecipeID", recipe._recipeID);
            msc.Parameters.AddWithValue("@RecipeName", recipe._Name);
            msc.Parameters.AddWithValue("@RecipePersons", recipe._PerPerson);
            msc.Parameters.AddWithValue("@RecipeDescription", recipe._description);

            NonQueryMSC(msc, dbConnect);
        }

        private void AddIngredientsToDatabase(List<Ingredient> ingredients, DatabaseConnect dbConnect)
        {
            foreach (Ingredient ingredient in ingredients)
            {
                if (!IngredientExist(ingredient, dbConnect))
                {
                    AddIngredientToDatabase(ingredient, dbConnect);
                }
            }
        }

        private void AddIngredientToDatabase(Ingredient ingredient, DatabaseConnect dbInfo)
        {
            string IngredientToDatabase = "INSERT INTO `Ingredients` (`ingredientName`) VALUES (@Ingredient);";
            MySqlCommand msc = new MySqlCommand(IngredientToDatabase);

            msc.Parameters.AddWithValue("@Ingredient", ingredient._IngredientName);

            NonQueryMSC(msc, dbInfo);
        }

        private bool IngredientExist(Ingredient ingredient, DatabaseConnect dbConnect)
        {
            bool exist = false;
            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(dbConnect.ConnectionString(true));
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

        private void CombineRecipeAndIngredient(Recipe recipe, DatabaseConnect dbConnect)
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

                NonQueryMSC(msc, dbConnect);
            }
        }

        public void NonQueryMSC(MySqlCommand msc, DatabaseConnect dbConnect)
        {
            MySqlConnection connection = null;

            try
            {
                connection = new MySqlConnection(dbConnect.ConnectionString(true));
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
