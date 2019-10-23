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

                string recipeQuery = "INSERT INTO `Recipes`(`id`,`recipeName`,`amountPerson`,`recipeDesc`) VALUES(1,\"" + recipe._Name + "\", " +
                                                                                                                     "" + recipe._PerPerson + "," +
                                                                                                                   "\"" + recipe._description + "\");";

                new MySqlCommand(recipeQuery, connection).ExecuteNonQuery();
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

                string ingredientExist = "SELECT * FROM `Ingredients` WHERE `ingredientName` = '" + ingredient._IngredientName + "';";

                MySqlDataAdapter ingredientsFromQuery = new MySqlDataAdapter(new MySqlCommand(ingredientExist, connection));
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

                string addIngredient = "INSERT INTO `Ingredients` (`ingredientName`) VALUES ('"+ ingredient._IngredientName +"');";

                new MySqlCommand(addIngredient, connection).ExecuteNonQuery();

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
                foreach (Ingredient ingredient in recipe._ingredientList)
                {
                    string addIngredientReferance = "INSERT INTO `IngredientsInRecipe` (`recipeID`,`ingredientName`,`amount`,`unit`)" +
                                                    "VALUES("+ recipe._recipeID + ", '" + ingredient._IngredientName + "', " + ingredient._Amount + ", '" + ingredient._unit + "');";

                    new MySqlCommand(addIngredientReferance, connection).ExecuteNonQuery();
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
