using BBCollection.BBObjects;
using BBCollection.DBConncetion;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;

namespace BBCollection.HandleRecipe
{
    class RecipeToDatabase
    {
        public void CombineRecipe(Recipe recipe, DatabaseInformation dbInformation)
        {
            AddRecipeToDatabase(recipe, dbInformation);
            AddIngredientsToDatabase(recipe._ingredientList, dbInformation);
            CombineRecipeAndIngredient(recipe, dbInformation);
        }

        private void AddRecipeToDatabase(Recipe recipe, DatabaseInformation dbInformation)
        {
            string recipeQuery = "INSERT INTO `Recipes`(`id`,`recipeName`,`amountPerson`,`recipeDesc`) VALUES(@RecipeID,@RecipeName,@RecipePersons,@RecipeDescription);";
            MySqlCommand msc = new MySqlCommand(recipeQuery);

            msc.Parameters.AddWithValue("@RecipeID", recipe._recipeID);
            msc.Parameters.AddWithValue("@RecipeName", recipe._Name);
            msc.Parameters.AddWithValue("@RecipePersons", recipe._PerPerson);
            msc.Parameters.AddWithValue("@RecipeDescription", recipe._description);

            new SQLConnect().NonQueryMSC(msc, dbInformation);
        }

        private void AddIngredientsToDatabase(List<Ingredient> ingredients, DatabaseInformation dbInformation)
        {
            foreach (Ingredient ingredient in ingredients)
            {
                if (!IngredientExist(ingredient, dbInformation))
                {
                    AddIngredientToDatabase(ingredient, dbInformation);
                }
            }
        }

        private void AddIngredientToDatabase(Ingredient ingredient, DatabaseInformation dbInformation)
        {
            string IngredientToDatabase = "INSERT INTO `Ingredients` (`ingredientName`) VALUES (@Ingredient);";
            MySqlCommand msc = new MySqlCommand(IngredientToDatabase);

            msc.Parameters.AddWithValue("@Ingredient", ingredient._IngredientName);

            new SQLConnect().NonQueryMSC(msc, dbInformation);
        }

        private bool IngredientExist(Ingredient ingredient, DatabaseInformation dbInformation)
        {
            string ingredientExist = "SELECT * FROM `Ingredients` WHERE `ingredientName` = @Ingredient;";
            MySqlCommand msc = new MySqlCommand(ingredientExist);

            msc.Parameters.AddWithValue("@Ingredient", ingredient._IngredientName);

            if (new SQLConnect().DynamicSimpleListSQL(msc, dbInformation).Tables[0].Rows.Count == 1)
            {
                return true;
            } else
            {
                return false;
            }
        }

        private void CombineRecipeAndIngredient(Recipe recipe, DatabaseInformation dbInformation)
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

                new SQLConnect().NonQueryMSC(msc, dbInformation);
            }
        }
    }
}
