using BBCollection.BBObjects;
using BBCollection.DBConncetion;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace BBCollection.DBHandling
{
    public class RecipeHandling
    {
        #region Functions handling GetRecipe and GetIngredient.
        public List<Recipe> GetList(string recipeName)
        {
            List<Recipe> recipeList = new List<Recipe>();
            string table = "recipes";
            string collumn = "recipename";

            MySqlCommand msc = new SqlQuerySort().SortMSC(recipeName, table, collumn);
            try
            {
                foreach (DataRow r in new SQLConnect().DynamicSimpleListSQL(msc).Tables[0].Rows)
                {

                    Recipe recipe = new Recipe((int)r[0], (string)r[1], (string)r[3], GetIngredients((int)r[0]), Convert.ToSingle(r[2]));

                    recipeList.Add(recipe);
                }
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e);
            }
            return recipeList;
        }

        public async Task<List<Recipe>> GetRange(string recipeName, int limit, int offset)
        {
            List<Recipe> recipeList = new List<Recipe>();
            string table = "recipes";
            string collumn = "recipename";

            return await Task.Run(() =>
            {
                MySqlCommand msc = new SqlQuerySort().SortMSCInterval(recipeName, table, collumn, limit, offset);
                foreach (DataRow r in new SQLConnect().DynamicSimpleListSQL(msc).Tables[0].Rows)
                {

                    Recipe recipe = new Recipe((int)r[0], (string)r[1], (string)r[3], await GetIngredients((int)r[0]), Convert.ToSingle(r[2])); ;

                    recipeList.Add(recipe);
                }
                return recipeList;
            });

        }

        public async Task<List<Ingredient>> GetIngredients(int recipeID)
        {
            List<Ingredient> ingredients = new List<Ingredient>();
            string ingredientsToRecipeQuery = "SELECT ingredients.ingredientName, ingredientsinrecipe.unit, ingredientsinrecipe.amount " +
                                              "FROM ingredientsinrecipe INNER JOIN ingredients ON ingredientsinrecipe.ingredientID = ingredients.id " +
                                              "WHERE ingredientsinrecipe.recipeID = @RecipeID;";

            MySqlCommand msc = new MySqlCommand(ingredientsToRecipeQuery);
            msc.Parameters.AddWithValue("@RecipeID", recipeID);
            return Task.Run(() =>
            {
                foreach (DataRow r in new SQLConnect().DynamicSimpleListSQL(msc).Tables[0].Rows)
                {
                    Ingredient ingredient = new Ingredient((string)r[0], (string)r[1], (int)r[2]);
                    ingredients.Add(ingredient);
                }

                return ingredients;
            });
        }
        #endregion

        #region Functions handling AddRecipe
        public void AddList(Recipe recipe)
        {
            Task.Run(() =>
            {
                AddRecipeToDatabase(recipe);
                AddIngredientsToDatabase(recipe._ingredientList);
                CombineRecipeAndIngredient(recipe);
            });
        }

        private void AddRecipeToDatabase(Recipe recipe)
        {
            string recipeQuery = "INSERT INTO `Recipes`(`id`,`recipeName`,`amountPerson`,`recipeDesc`) VALUES(@RecipeID,@RecipeName,@RecipePersons,@RecipeDescription);";
            MySqlCommand msc = new MySqlCommand(recipeQuery);

            msc.Parameters.AddWithValue("@RecipeID", recipe._recipeID);
            msc.Parameters.AddWithValue("@RecipeName", recipe._Name);
            msc.Parameters.AddWithValue("@RecipePersons", recipe._PerPerson);
            msc.Parameters.AddWithValue("@RecipeDescription", recipe._description);

            new SQLConnect().NonQueryMSC(msc);
        }

        private void AddIngredientsToDatabase(List<Ingredient> ingredients)
        {
            foreach (Ingredient ingredient in ingredients)
            {
                if (!IngredientExist(ingredient))
                {
                    AddIngredientToDatabase(ingredient);
                }
            }
        }

        private void AddIngredientToDatabase(Ingredient ingredient)
        {
            string IngredientToDatabase = "INSERT INTO `Ingredients` (`ingredientName`) VALUES (@Ingredient);";
            MySqlCommand msc = new MySqlCommand(IngredientToDatabase);

            msc.Parameters.AddWithValue("@Ingredient", ingredient._ingredientName);

            new SQLConnect().NonQueryMSC(msc);
        }

        private bool IngredientExist(Ingredient ingredient)
        {
            string ingredientExist = "SELECT * FROM `id` WHERE `id` = @Id;";
            MySqlCommand msc = new MySqlCommand(ingredientExist);

            msc.Parameters.AddWithValue("@Id", ingredient._id);

            return new SQLConnect().CheckRecordExist(msc);
        }

        private void CombineRecipeAndIngredient(Recipe recipe)
        {
            foreach (Ingredient ingredient in recipe._ingredientList)
            {
                string addIngredientReferance = "INSERT INTO `IngredientsInRecipe` (`recipeID`,`ingredientID`,`amount`,`unit`)" +
                                                "VALUES(@RecipeID," +
                                                "@IngredientID" +
                                                ",@Amount,@Unit)";

                MySqlCommand msc = new MySqlCommand(addIngredientReferance);

                msc.Parameters.AddWithValue("@RecipeID", recipe._recipeID);
                msc.Parameters.AddWithValue("@IngredientID", getIngredientFromName(ingredient._ingredientName));
                msc.Parameters.AddWithValue("@Amount", ingredient._amount);
                msc.Parameters.AddWithValue("@Unit", ingredient._unit);

                new SQLConnect().NonQueryMSC(msc);
            }
        }

        private int getIngredientFromName(string ingredientName)
        {
            int ingredientID = -1;

            string getIngredient =
                "SELECT id FROM ingredients WHERE ingredientname = @IngredientName";

            MySqlCommand msc = new MySqlCommand(getIngredient);

            msc.Parameters.AddWithValue("@IngredientName", ingredientName);

            DataSet ds = new SQLConnect().DynamicSimpleListSQL(msc);

            try
            {
                ingredientID = (int)ds.Tables[0].Rows[0][0];
            }
            catch (IndexOutOfRangeException indexOutOfRangeException)
            {
                Console.WriteLine("Index not found: " + indexOutOfRangeException);
            }
            catch (NullReferenceException nullReferenceException)
            {
                Console.WriteLine("NullException: " + nullReferenceException);
            }

            return ingredientID;
        }
        #endregion
    }
}

