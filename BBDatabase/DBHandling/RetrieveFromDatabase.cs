using BBCollection.BBObjects;
using BBCollection.DBConncetion;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace BBCollection.HandleRecipe
{
    class RetrieveFromDatabase
    {
        public List<Recipe> RetrieveRecipeList(string recipeName, DatabaseInformation dbInformation)
        {
            Stopwatch sw = new Stopwatch();
            List<Recipe> recipeList = new List<Recipe>();
            string table = "recipes";
            string collumn = "recipename";
            
            MySqlCommand msc = new SqlQuerySort().SortMSC(recipeName, table,collumn);
            sw.Start();
            try{
                foreach (DataRow r in new SQLConnect().DynamicSimpleListSQL(msc, dbInformation).Tables[0].Rows)
                {

                    Recipe recipe = new Recipe((int)r[0], (string)r[1], (string)r[3], GetIngredientsFromRecipeID((int)r[0], dbInformation), Convert.ToSingle(r[2]));

                    recipeList.Add(recipe);
                }
            }
            catch(NullReferenceException e)
            {
                Console.WriteLine(e);
            }
            
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
            return recipeList;
        }

        public List<Ingredient> GetIngredientsFromRecipeID(int recipeID, DatabaseInformation dbInformation)
        {
            List<Ingredient> ingredients = new List<Ingredient>();
            string ingredientsToRecipeQuery = "SELECT ingredients.ingredientName, ingredientsinrecipe.unit, ingredientsinrecipe.amount " +
                                              "FROM ingredientsinrecipe INNER JOIN ingredients ON ingredientsinrecipe.ingredientID = ingredients.id " +
                                              "WHERE ingredientsinrecipe.recipeID = @RecipeID;";

            MySqlCommand msc = new MySqlCommand(ingredientsToRecipeQuery);
            msc.Parameters.AddWithValue("@RecipeID", recipeID);

            foreach (DataRow r in new SQLConnect().DynamicSimpleListSQL(msc, dbInformation).Tables[0].Rows)
            {
                Ingredient ingredient = new Ingredient((string) r[0], (string) r[1], (int) r[2]);
                ingredients.Add(ingredient);
            }

            return ingredients;
        }

        public List<Recipe> RetrieveRecipeListInterval(string recipeName, int limit, int offset, DatabaseInformation dbInformation)
        {
            Stopwatch sw = new Stopwatch();
            List<Recipe> recipeList = new List<Recipe>();
            string table = "recipes";
            string collumn = "recipename";

            MySqlCommand msc = new SqlQuerySort().SortMSCInterval(recipeName, table, collumn, limit, offset);
            sw.Start();
            foreach (DataRow r in new SQLConnect().DynamicSimpleListSQL(msc, dbInformation).Tables[0].Rows)
            {

                Recipe recipe = new Recipe((int)r[0], (string)r[1], (string)r[3], GetIngredientsFromRecipeID((int)r[0], dbInformation), Convert.ToSingle(r[2]));

                recipeList.Add(recipe);
            }
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
            return recipeList;
        }
    }
}
