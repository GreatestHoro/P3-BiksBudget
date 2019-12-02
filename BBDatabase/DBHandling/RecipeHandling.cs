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
        public async Task<List<Recipe>> GetList(string recipeName)
        {
            List<Recipe> recipeList = new List<Recipe>();
            string table = "recipes";
            string collumn = "recipename";

            return await Task.Run(async () =>
            {
                MySqlCommand msc = new SqlQuerySort().SortMSC(recipeName, table, collumn);
                try
                {
                    DataSet ds = await new SQLConnect().DynamicSimpleListSQL(msc);
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {

                        Recipe recipe = new Recipe((int)r[0], (string)r[1], (string)r[3], await GetIngredients((int)r[0]), Convert.ToSingle(r[2]));

                        recipeList.Add(recipe);
                    }
                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine(e);
                }
                return recipeList;
            });
        }

        public async Task<List<Recipe>> GetRange(string recipeName, int limit, int offset)
        {
            List<Recipe> recipeList = new List<Recipe>();
            string table = "recipes";
            string collumn = "recipename";

            return await Task.Run(async() =>
            {
                MySqlCommand msc = new SqlQuerySort().SortMSCInterval(recipeName, table, collumn, limit, offset);
                DataSet ds = await new SQLConnect().DynamicSimpleListSQL(msc);
                foreach (DataRow r in ds.Tables[0].Rows)
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
            return await Task.Run(async() =>
            {
                DataSet ds = await new SQLConnect().DynamicSimpleListSQL(msc);
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Ingredient ingredient = new Ingredient((string)r[0], (string)r[1], (int)r[2]);
                    ingredients.Add(ingredient);
                }

                return ingredients;
            });
        }
        #endregion

        #region Functions handling AddRecipe
        public async Task AddList(Recipe recipe)
        {
            await Task.Run(async() =>
            {
                await AddRecipeToDatabase(recipe);
                await AddIngredientsToDatabase(recipe._ingredientList);
                await CombineRecipeAndIngredient(recipe);
            });
        }

        private async Task AddRecipeToDatabase(Recipe recipe)
        {
            string recipeQuery = "INSERT INTO `Recipes`(`id`,`recipeName`,`amountPerson`,`recipeDesc`) VALUES(@RecipeID,@RecipeName,@RecipePersons,@RecipeDescription);";
            MySqlCommand msc = new MySqlCommand(recipeQuery);

            msc.Parameters.AddWithValue("@RecipeID", recipe._recipeID);
            msc.Parameters.AddWithValue("@RecipeName", recipe._Name);
            msc.Parameters.AddWithValue("@RecipePersons", recipe._PerPerson);
            msc.Parameters.AddWithValue("@RecipeDescription", recipe._description);
            
            await Task.Run(async() =>
            {
                await new SQLConnect().NonQueryMSC(msc);
            });
        }

        private async Task AddIngredientsToDatabase(List<Ingredient> ingredients)
        {
            await Task.Run(async() =>
            {
                foreach (Ingredient ingredient in ingredients)
                {
                    if (!await IngredientExist(ingredient))
                    {
                        await AddIngredientToDatabase(ingredient);
                    }
                }
            });
        }

        private async Task AddIngredientToDatabase(Ingredient ingredient)
        {
            string IngredientToDatabase = "INSERT INTO `Ingredients` (`ingredientName`) VALUES (@Ingredient);";
            MySqlCommand msc = new MySqlCommand(IngredientToDatabase);

            msc.Parameters.AddWithValue("@Ingredient", ingredient._ingredientName);

            await Task.Run(async() =>
            {
                await new SQLConnect().NonQueryMSC(msc);
            });
        }

        private async Task<bool> IngredientExist(Ingredient ingredient)
        {
            string ingredientExist = "SELECT * FROM `id` WHERE `id` = @Id;";
            MySqlCommand msc = new MySqlCommand(ingredientExist);

            msc.Parameters.AddWithValue("@Id", ingredient._id);

            return await Task.Run(() =>
            {
                return new SQLConnect().CheckRecordExist(msc);
            });
        }

        private async Task CombineRecipeAndIngredient(Recipe recipe)
        {
            await Task.Run(async() =>
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

                    await new SQLConnect().NonQueryMSC(msc);
                }
            });
        }

        private async Task<int> getIngredientFromName(string ingredientName)
        {
            int ingredientID = -1;

            string getIngredient =
                "SELECT id FROM ingredients WHERE ingredientname = @IngredientName";

            MySqlCommand msc = new MySqlCommand(getIngredient);

            msc.Parameters.AddWithValue("@IngredientName", ingredientName);

            return await Task.Run(async () =>
            {
                DataSet ds = await new SQLConnect().DynamicSimpleListSQL(msc);
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
            });
        }
        #endregion
    }
}

