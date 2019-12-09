using BBCollection.BBObjects;
using BBCollection.DBConncetion;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using BBCollection.Queries;

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
                foreach (Recipe r in recipeList)
                {
                    r.deleteDuplicates();
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

                IEnumerable<Ingredient> distinctIngredients = ingredients.GroupBy(o => o._ingredientName).Select(g => g.First());

                return distinctIngredients.ToList();
            });
        }
        #endregion

        #region Functions handling AddRecipe
        public async Task AddList(Recipe recipe)
        {
            await AddRecipeToDatabase(recipe);
            await AddIngredientsToDatabase(recipe._ingredientList);
            await CombineRecipeAndIngredient(recipe);
        }

        private async Task AddRecipeToDatabase(Recipe recipe)
        {
            string recipeQuery = "INSERT INTO `Recipes`(`id`,`recipeName`,`amountPerson`,`recipeDesc`) VALUES(@RecipeID,@RecipeName,@RecipePersons,@RecipeDescription);";
            MySqlCommand msc = new MySqlCommand(recipeQuery);

            msc.Parameters.AddWithValue("@RecipeID", recipe._recipeID);
            msc.Parameters.AddWithValue("@RecipeName", recipe._Name);
            msc.Parameters.AddWithValue("@RecipePersons", recipe._PerPerson);
            msc.Parameters.AddWithValue("@RecipeDescription", recipe._description);
            
            await new SQLConnect().NonQueryMSC(msc);
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

        #region Finding ingredients based on references in products.
        public async Task<List<Recipe>> GetReferencesAsync(List<string> references)
        {
            List<Recipe> recipes = new List<Recipe>();
            string table = "t"; string statement = "s"; string parameter = "@Parameter"; int count = 0;

            string getRecipesQuery =
                "SELECT DISTINCT " + statement + count + ".* FROM ";

            List<string> tables = new List<string>();

            MySqlCommand msc = await Task.Run(() =>
            {
                MySqlCommand msc = new MySqlCommand();
                foreach (string reference in references)
                {
                    if (count == 0)
                    {
                        getRecipesQuery += "(SELECT " + table + count + ".*," + table + count + ".id as tempid" + count + " FROM recipes " + table + count + " " +
                        "inner join ingredientsinrecipe on " + table + count + ".id = ingredientsinrecipe.recipeid " +
                        "inner join ingredients on ingredientsinrecipe.ingredientid = ingredients.id" +
                        " WHERE ingredientName like " + parameter + count + ") " + statement + count;
                    }
                    else
                    {
                        getRecipesQuery += "(SELECT " + table + count + ".id as tempid" + count + " FROM recipes " + table + count + " " +
                        "inner join ingredientsinrecipe on " + table + count + ".id = ingredientsinrecipe.recipeid " +
                        "inner join ingredients on ingredientsinrecipe.ingredientid = ingredients.id " +
                        " WHERE ingredientName like " + parameter + count + ") " + statement + count;
                    }


                    tables.Add(statement + count + ".tempid" + count);
                    msc.Parameters.AddWithValue(parameter + count, reference);

                    count++;
                    if (references.Count != count)
                    {
                        getRecipesQuery += ",";
                    }
                }

                var tArr = tables.ToArray();
                string check = " WHERE ";
                for (int i = 1; i < tArr.Length; i++)
                {
                    int j = 0;

                    while (j < i)
                    {
                        check += $"{tArr[i]} = {tArr[j]}";
                        j++;
                        if (i != 1)
                        {
                            if (j != i)
                            {
                                check += " AND ";

                            }
                        }
                    }
                    if (i < tArr.Length - 1)
                    {
                        check += " AND ";
                    }

                }

                msc.CommandText = getRecipesQuery;

                return msc;
            });

            try 
            { 
                DataSet ds = await new SQLConnect().DynamicSimpleListSQL(msc);
                foreach (DataRow r in ds.Tables[0].Rows)
                {

                    Recipe recipe = new Recipe((int)r[0], (string)r[1], (string)r[3], await GetIngredients((int)r[0]), Convert.ToSingle(r[2]));

                    recipes.Add(recipe);
                }
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e);
            }

            return recipes;
        }
        #endregion

        public async Task<List<ComplexRecipe>> GetPriceAsync(string productName, int limit, int offset)
        {
            string getRecipesPrice =
                "SELECT * FROM biksbudgetdb.recipes WHERE recipename like @ProductName AND recipe_totalprice > 0 " +
                "ORDER BY recipe_totalprice LIMIT @Limit OFFSET @Offset";
            List<ComplexRecipe> complexRecipes = new List<ComplexRecipe>();

            MySqlCommand msc = new MySqlCommand(getRecipesPrice);

            msc.Parameters.AddWithValue("@ProductName", "%" + productName + "%");
            msc.Parameters.AddWithValue("@Limit", limit);
            msc.Parameters.AddWithValue("@Offset", offset);

            DataSet ds = await new SQLConnect().DynamicSimpleListSQL(msc);
            if (ds.Tables.Count != 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    ComplexRecipe recipe = new ComplexRecipe((int)r[0], (string)r[1], (string)r[3], await GetIngredients((int)r[0]), Convert.ToSingle(r[2]), new ComplexRecipeComponent(Convert.ToDouble(r[4])));
                    complexRecipes.Add(recipe);
                }
            }
            return complexRecipes;
        }

        public async Task GenerateTotalPriceAsync()
        {
            List<ComplexRecipe> CRC = await new RecipeQuery().CheapestCRecipes("");

            Console.WriteLine(CRC.Count);

            foreach(ComplexRecipe c in CRC)
            {
                await UpdatePriceAsync(c._recipeID, c._complexRecipeComponent.RecipeCost);
            }
        }

        private async Task UpdatePriceAsync(int recipeID, double price)
        {
            string recipeQuery = "UPDATE `recipes` SET `recipe_totalprice` = @Price WHERE id = @RecipeId";
            MySqlCommand msc = new MySqlCommand(recipeQuery);

            msc.Parameters.AddWithValue("@Price", price);
            msc.Parameters.AddWithValue("@RecipeId", recipeID);

            await new SQLConnect().NonQueryMSC(msc);
        }

        public async Task AddImage(string image, string prodid)
        {
            string insertQuery =
                "UPDATE `recipes` SET `image` = @Image WHERE id = @Prodid";

            MySqlCommand msc = new MySqlCommand(insertQuery);

            msc.Parameters.AddWithValue("@Image", image);
            msc.Parameters.AddWithValue("@Prodid", prodid);

            await Task.Run(() => new SQLConnect().NonQueryMSC(msc));
        }
    }
}

