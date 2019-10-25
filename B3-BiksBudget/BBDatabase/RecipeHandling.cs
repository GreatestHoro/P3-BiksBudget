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


        public void addRecipe(Recipe recipe, DatabaseInformation dbInfo)
        {
            MySqlConnection connection = null;

            


            try
            {
                connection = new MySqlConnection(dbInfo.connectionString(true));
                connection.Open();

                string recipeQuery = "INSERT INTO `Recipes`(`id`,`recipeName`,`amountPerson`,`recipeDesc`) VALUES(@RecipeID,@RecipeName,@RecipePersons,@RecipeDescription);";
                MySqlCommand msc = new MySqlCommand(recipeQuery, connection);

                msc.Parameters.AddWithValue("@RecipeID", recipe._recipeID);
                msc.Parameters.AddWithValue("@RecipeName", recipe._Name);
                msc.Parameters.AddWithValue("@RecipePersons", recipe._PerPerson);
                msc.Parameters.AddWithValue("@RecipeDescription", recipe._description);

                msc.ExecuteNonQuery();

                addMultipleIngredients(recipe._ingredientList, dbInfo);
                combineRecipeAndIngredient(recipe, dbInfo);
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

        public void addMultipleIngredients(List<Ingredient> ingredients, DatabaseInformation dbInfo)
        {
            foreach (Ingredient ingredient in ingredients)
            {
                if (!ingredientExist(ingredient, dbInfo))
                {
                    addIngredient(ingredient,dbInfo);
                }
            }
        }

        private bool ingredientExist(Ingredient ingredient, DatabaseInformation dbInfo)
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

        private void addIngredient(Ingredient ingredient, DatabaseInformation dbInfo)
        {
            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(dbInfo.connectionString(true));
                connection.Open();

                string addIngredient = "INSERT INTO `Ingredients` (`ingredientName`) VALUES (@Ingredient);";
                MySqlCommand msc = new MySqlCommand(addIngredient, connection);

                msc.Parameters.AddWithValue("@Ingredient", ingredient._IngredientName);

                msc.ExecuteNonQuery();

            }
            catch (MySqlException)
            {
                // Make exception
            }
            finally
            {
                if(connection != null)
                {
                    connection.Close();
                }
            }
        }



        private void combineRecipeAndIngredient(Recipe recipe, DatabaseInformation dbInfo)
        {
            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(dbInfo.connectionString(true));
                connection.Open();
                Console.WriteLine("QQ");
                foreach (Ingredient ingredient in recipe._ingredientList)
                {
                    string addIngredientReferance = "INSERT INTO `IngredientsInRecipe` (`recipeID`,`ingredientName`,`amount`,`unit`)" +
                                                    "VALUES(@RecipeID,@Ingredient,@Amount,@Unit);";


                    MySqlCommand msc = new MySqlCommand(addIngredientReferance, connection);
                    msc.Parameters.AddWithValue("@RecipeID", recipe._recipeID);
                    msc.Parameters.AddWithValue("@Ingredient", ingredient._IngredientName);
                    msc.Parameters.AddWithValue("@Amount", ingredient._Amount);
                    msc.Parameters.AddWithValue("@Unit", ingredient._unit);

                    msc.ExecuteNonQuery();
                }
            }
            catch (MySqlException)
            {
                // Make exception
            }
            finally
            {
                if(connection != null)
                {
                    connection.Close();
                }
            }

        }



    }
}
