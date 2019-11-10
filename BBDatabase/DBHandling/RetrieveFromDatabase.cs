using BBCollection.BBObjects;
using BBCollection.DBConncetion;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace BBCollection.HandleRecipe
{
    class RetrieveFromDatabase
    {
        public List<Recipe> RetrieveRecipeList(string recipeName, DatabaseInformation dbInformation)
        {
            List<Recipe> recipeList = new List<Recipe>();
            string recipesQuery = "SELECT * FROM recipes WHERE recipeName LIKE @RecipeName";

            MySqlCommand msc = new MySqlCommand(recipesQuery);
            msc.Parameters.AddWithValue("@RecipeName", "%" + recipeName + "%");

            foreach (DataRow r in new SQLConnect().DynamicSimpleListSQL(msc, dbInformation).Tables[0].Rows)
            {
                Recipe recipe = new Recipe((int)r[0], (string)r[1], (string)r[3], GetIngredientsFromRecipeID((int)r[0], dbInformation), Convert.ToSingle(r[2]));

                recipeList.Add(recipe);
            }

            return recipeList;
        }

        public List<Ingredient> GetIngredientsFromRecipeID(int recipeID, DatabaseInformation dbInformation)
        {
            List<Ingredient> ingredients = new List<Ingredient>();
            string ingredientsToRecipeQuery = "SELECT * FROM IngredientsInRecipe WHERE recipeID = @RecipeID";

            MySqlCommand msc = new MySqlCommand(ingredientsToRecipeQuery);
            msc.Parameters.AddWithValue("@RecipeID", recipeID);

            foreach (DataRow r in new SQLConnect().DynamicSimpleListSQL(msc, dbInformation).Tables[0].Rows)
            {
                Ingredient ingredient = new Ingredient((string) r[2], (string) r[4], (int) r[3]);
                ingredients.Add(ingredient);
            }

            return ingredients;
        }
    }
}
