using BBCollection.BBObjects;
using BBCollection.DBConncetion;
using BBCollection.Queries;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BBCollection.DBHandling
{
    public class RecipeHandling
    {
        #region Functions handling GetRecipe and GetIngredient.
        public async Task<List<Recipe>> GetListAsync(string recipeName)
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

            return await Task.Run(async () =>
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
            
                DataSet ds = await new SQLConnect().DynamicSimpleListSQL(msc);
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Ingredient ingredient = new Ingredient((string)r[0], (string)r[1], (int)r[2]);
                    ingredients.Add(ingredient);
                }

                IEnumerable<Ingredient> distinctIngredients = ingredients.GroupBy(o => o._ingredientName).Select(g => g.First());

                return distinctIngredients.ToList();
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

            await new SQLConnect().NonQueryMSCAsync(msc);
        }

        private async Task AddIngredientsToDatabase(List<Ingredient> ingredients)
        {
            await Task.Run(async () =>
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

            await Task.Run(async () =>
            {
                await new SQLConnect().NonQueryMSCAsync(msc);
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
            
                foreach (Ingredient ingredient in recipe._ingredientList)
                {
                ingredient._id = await getIngredientFromName(ingredient._ingredientName);
                    string addIngredientReferance = "INSERT INTO `IngredientsInRecipe` (`recipeID`,`ingredientID`,`amount`,`unit`)" +
                                                    "VALUES(@RecipeID," +
                                                    "@IngredientID" +
                                                    ",@Amount,@Unit)";

                    MySqlCommand msc = new MySqlCommand(addIngredientReferance);

                    msc.Parameters.AddWithValue("@RecipeID", recipe._recipeID);
                    msc.Parameters.AddWithValue("@IngredientID", ingredient._id);
                    msc.Parameters.AddWithValue("@Amount", ingredient._amount);
                    msc.Parameters.AddWithValue("@Unit", ingredient._unit);

                    await new SQLConnect().NonQueryMSCAsync(msc);
                }
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
                    ComplexRecipe recipe = new ComplexRecipe((int)r[0], (string)r[1], (string)r[3],
                        await GetIngredients((int)r[0]), Convert.ToSingle(r[2]),
                        new ComplexRecipeComponent(Convert.ToDouble(r[4])));
                    complexRecipes.Add(recipe);
                }
            }
            return complexRecipes;
        }

        public async Task<List<string>> GetAllIngredientNames()
        {
            List<string> ingredientNames = new List<string>();
            string getNamesQuery =
                "SELECT ingredientName FROM ingredients";

            MySqlCommand msc = new MySqlCommand(getNamesQuery);

            DataSet ds = await new SQLConnect().DynamicSimpleListSQL(msc);

            try
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    ingredientNames.Add(r[0].ToString());
                }
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e);
            }
            return ingredientNames;
        }

        public async Task GenerateTotalPriceAsync()
        {
            List<ComplexRecipe> CRC = await new RecipeQuery().CheapestCRecipes("");

            Console.WriteLine(CRC.Count);

            foreach (ComplexRecipe c in CRC)
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

            await new SQLConnect().NonQueryMSCAsync(msc);
        }

        public async Task AddImage(string image, string prodid)
        {
            string insertQuery =
                "UPDATE `recipes` SET `image` = @Image WHERE id = @Prodid";

            MySqlCommand msc = new MySqlCommand(insertQuery);

            msc.Parameters.AddWithValue("@Image", image);
            msc.Parameters.AddWithValue("@Prodid", prodid);

            await Task.Run(() => new SQLConnect().NonQueryMSCAsync(msc));
        }

        public async Task PopulateIngredientLink()
        {
            string copyColumn =
                "INSERT INTO ingredient_store_link (ingredientName)" +
                "SELECT ingredientName FROM ingredients";

            await new SQLConnect().NonQueryString(copyColumn);
        }
        public async Task GenerateCheapestPL()
        {
            RecipeQuery rq = new RecipeQuery();
            List<Recipe> recipeList = await GetListAsync("");

            List<string> distinctIngredients = rq.DistinctIngredients(recipeList);

            Dictionary<string, List<Product>> productsDictBilka = await rq.MatchingProductsChain(distinctIngredients, "Bilka");
            Dictionary<string, List<Product>> productsDictFakta = await rq.MatchingProductsChain(distinctIngredients, "Fakta");
            Dictionary<string, List<Product>> productsDictSuperBrugsen = await rq.MatchingProductsChain(distinctIngredients, "SuperBrugsen");

            await new DatabaseConnect().Recipe.InsertIngredientLink(productsDictBilka, "bilka");
            await new DatabaseConnect().Recipe.InsertIngredientLink(productsDictFakta, "fakta");
            await new DatabaseConnect().Recipe.InsertIngredientLink(productsDictSuperBrugsen, "superbrugsen");
        }

        public async Task InsertIngredientLink(Dictionary<string, List<Product>> ingredientToProducts, string store)
        {
            string updateLink =
                "Update `ingredient_store_link` SET `" + store + "` = @ProductID WHERE ingredientName = @IngredientName";

            foreach (KeyValuePair<string, List<Product>> pair in ingredientToProducts)
            {
                if (pair.Value.Count != 0)
                {
                    MySqlCommand msc = new MySqlCommand(updateLink);
                    msc.Parameters.AddWithValue("@ProductID", pair.Value.First()._id);
                    msc.Parameters.AddWithValue("@IngredientName", pair.Key);

                    await new SQLConnect().NonQueryMSCAsync(msc);
                }
            }
        }

        public async Task<List<ComplexRecipe>> GetListAsync(string recipeName, Chain chain, int limit, int offset)
        {
            List<ComplexRecipe> complexRecipes = new List<ComplexRecipe>();
            List<string> stores = ConverteChain(chain);
            MySqlCommand msc;

            if (stores.Count == 0)
            {
                msc = new MySqlCommand(MultipleRecipeQuery(stores));
            }
            else if (stores.Count == 1)
            {
                msc = new MySqlCommand(SingleRecipeQuery(stores[0]));
            }
            else
            {
                msc = new MySqlCommand(MultipleRecipeQuery(stores));
            }

            msc.Parameters.AddWithValue("@RecipeName", "%" + recipeName + "%");
            msc.Parameters.AddWithValue("@Limit", limit);
            msc.Parameters.AddWithValue("@Offset", offset);

            DataSet ds = await new SQLConnect().DynamicSimpleListSQL(msc);
            if (ds.Tables.Count != 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    double price = 0;
                    if(r[3] != DBNull.Value)
                    {
                        price = Convert.ToDouble(r[4]);
                    }

                    ComplexRecipe recipe = new ComplexRecipe((int)r[0], (string)r[1], (string)r[2],
                        await GetIngredients((int)r[0]), Convert.ToSingle(r[3]),
                        new ComplexRecipeComponent(price));
                    complexRecipes.Add(recipe);
                }
            }
            return complexRecipes;

        }

        private string SingleRecipeQuery(string store)
        {
            string singleQuery =
                "SELECT t1.id, t1.recipeName, t1.recipeDesc, t1.amountPerson, sum(price) as min_price, count(*) FROM recipes t1 " +
                "inner join ingredientsinrecipe t2 on t1.id = t2.recipeID " +
                "inner join ingredients t3 on t2.ingredientID = t3.id " +
                "inner join(select isl.ingredientName as ingredientName, p1.price as price " +
                "from ingredient_store_link isl " +
                "left join products p1 on p1.id = isl." + store + ") as t4 on t3.ingredientName = t4.ingredientName " +
                "WHERE t1.recipeName like @RecipeName AND price IS NOT NULL " +
                "group by t1.id order by min_price LIMIT @Limit OFFSET @Offset";


            return singleQuery;
        }

        private string MultipleRecipeQuery(List<string> stores)
        {
            string mrQuery =
                "SELECT t1.id, t1.recipeName, t1.recipeDesc, t1.amountPerson, sum(price) as min_price, count(*) FROM recipes t1 " +
                "inner join ingredientsinrecipe t2 on t1.id = t2.recipeID " +
                "inner join ingredients t3 on t2.ingredientID = t3.id " +
                "inner join( " +
                "select " +
                "isl.ingredientName as ingredientName, least(" + GetCoalesce(stores.Count()) + ") as price " +
                "from ingredient_store_link isl "
                + GetJoins(stores) +
                ") as t4 on t3.ingredientName = t4.ingredientName " +
                "WHERE t1.recipeName like @RecipeName AND price IS NOT NULL " +
                "group by t1.id order by min_price LIMIT @Limit OFFSET @Offset";

            return mrQuery;
        }

        private string GetCoalesce(int stores)
        {
            string coalesce = "";
            for (int i = 0; i < stores; i++)
            {
                string tc = "";
                int u = 1; int d = stores;
                int ul = stores - i; int dl = (stores + 1) - i;

                while (d >= dl)
                {
                    tc = "p" + d + ".price," + tc; // d + coalesce;
                    d--;
                }
                while (u <= ul)
                {
                    tc += "p" + u + ".price,";
                    u++;
                }

                tc = tc.Remove(tc.Length - 1);
                coalesce += "COALESCE(" + tc + "), ";
            }
            return coalesce = coalesce.Remove(coalesce.Length - 2);
        }

        private string GetJoins(List<string> stores)
        {
            string joins = "";
            int count = 1;
            foreach (string store in stores)
            {
                joins += "left join products p" + count + " on p" + count + ".id = isl." + store + " ";
                count++;
            }
            return joins;
        }

        private List<string> ConverteChain(Chain chain)
        {
            List<string> chainList = new List<string>();
            if (chain == Chain.none)
            {
                chainList.Add("bilka");
                chainList.Add("fakta");
                chainList.Add("superbrugsen");
            }
            else
            {
                if ((chain & Chain.bilka) == Chain.bilka)
                {
                    chainList.Add("bilka");
                }
                if ((chain & Chain.fakta) == Chain.fakta)
                {
                    chainList.Add("fakta");
                }
                if ((chain & Chain.superBrugsen) == Chain.superBrugsen)
                {
                    chainList.Add("superbrugsen");
                }
            }
            return chainList;
        }

        public async Task<int> Count(string recipeName, Chain chain)
        {
            List<string> stores = ConverteChain(chain);
            MySqlCommand msc;
            

            if (stores.Count == 0)
            {
                msc = new MySqlCommand(MultipleRecipeQuery(stores));
            }
            else if (stores.Count == 1)
            {
                msc = new MySqlCommand(SingleRecipeQuery(stores[0]));
            }
            else
            {
                msc = new MySqlCommand(MultipleRecipeQuery(stores));
            }

            msc.Parameters.AddWithValue("@RecipeName", "%" + recipeName + "%");

            return await new SQLConnect().ElementCount(msc);
        }

        public string returnSingleQuery(string store)
        {
            string singleQuery =
                "SELECT count(*) FROM recipes t1 " +
                "inner join ingredientsinrecipe t2 on t1.id = t2.recipeID " +
                "inner join ingredients t3 on t2.ingredientID = t3.id " +
                "inner join(select isl.ingredientName as ingredientName, p1.price as price " +
                "from ingredient_store_link isl " +
                "left join products p1 on p1.id = isl." + store + ") as t4 on t3.ingredientName = t4.ingredientName " +
                "WHERE t1.recipeName like @RecipeName AND price IS NOT NULL ";

            return singleQuery;
        }

        public string multiString(List<string> stores)
        {
            string mrQuery =
                "SELECT t1.id, t1.recipeName, t1.recipeDesc, t1.amountPerson, sum(price) as min_price, count(*) FROM recipes t1 " +
                "inner join ingredientsinrecipe t2 on t1.id = t2.recipeID " +
                "inner join ingredients t3 on t2.ingredientID = t3.id " +
                "inner join( " +
                "select " +
                "isl.ingredientName as ingredientName, least(" + GetCoalesce(stores.Count()) + ") as price " +
                "from ingredient_store_link isl "
                + GetJoins(stores) +
                ") as t4 on t3.ingredientName = t4.ingredientName " +
                "WHERE t1.recipeName like @RecipeName AND price IS NOT NULL " +
                "group by t1.id order by min_price LIMIT @Limit OFFSET @Offset";

            return mrQuery;
        }
    }
}

