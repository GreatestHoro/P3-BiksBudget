using BBCollection.DBHandling;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BBCollection.BBObjects
{
    public class EmptyFridgeFuntionality
    {
        public List<List<string>> allRefs = new List<List<string>>();
        public List<WeightedRecipies> WeightedRecipies = new List<WeightedRecipies>();

        public EmptyFridgeFuntionality(List<Product> products)
        {
            allRefs = GetAllProductRefs(products);
        }
        public async Task<List<WeightedRecipies>> GetRecepies()
        {
            if (WeightedRecipies.Count == 0)
            {
                await CalculateWeightedRecipies();
            }

            return WeightedRecipies;
        }

        private async Task CalculateWeightedRecipies() 
        {
            foreach (List<string> refs in allRefs)
            {
                WeightedRecipies.AddRange(await GetRelevantRecepiesFromProd(refs));
            }
        }

        private async Task<List<WeightedRecipies>> GetRelevantRecepiesFromProd(List<string> prodRefs)
        {
            return await GetRelevantRecpies(prodRefs);
        }

        public async Task<List<WeightedRecipies>> GetSortedList()
        {
            return WeightedRecipies;
        }

        #region GenerateRecepies
        private async Task<List<WeightedRecipies>> GetRelevantRecpies(List<string> ProductRefrence)
        {
            RecipeHandling recipeHandling = new RecipeHandling();
            Dictionary<Recipe, Dictionary<Ingredient, List<string>>> dict = new Dictionary<Recipe, Dictionary<Ingredient, List<string>>>();
            List<Recipe> recipes = new List<Recipe>();
            foreach (string refs in ProductRefrence)
            {
                recipes.AddRange(await recipeHandling.GetReferencesAsync(new List<string>() { refs }));
            }
            foreach (Recipe r in recipes)
            {
                foreach (Ingredient I in r._ingredientList)
                {
                    foreach (List<string> sList in allRefs)
                    {
                        foreach (string s in sList)
                        {
                            if (I._ingredientName.Contains(s))
                            {
                                if (dict.ContainsKey(r))
                                {
                                    if (dict[r].ContainsKey(I))
                                    {
                                        dict[r][I].Add(s);
                                    }
                                    else
                                    {
                                        dict[r].Add(I, new List<string> { s });
                                    }
                                }
                                else
                                {
                                    dict.Add(r, new Dictionary<Ingredient, List<string>>() { { I, new List<string> { s } } });
                                }
                            }
                        }
                    }
                }
            }
            return NewGeneratWeighted(dict);
        }

        private List<WeightedRecipies> NewGeneratWeighted(Dictionary<Recipe, Dictionary<Ingredient, List<string>>> Recepies)
        {
            List<WeightedRecipies> weightedRecipies = new List<WeightedRecipies>();
            foreach (Recipe r in Recepies.Keys)
            {
                r.deleteDuplicates();
                weightedRecipies.Add(new WeightedRecipies(r, Recepies[r]));
            }
            return weightedRecipies;
        }
        #endregion
        #region sortMethods
        public void SortByPmatch(List<WeightedRecipies> recpies)
        {
            WeightedRecipies = recpies.OrderByDescending(x => x.pMatch).ToList();
        }

        public void SortByMatchnum(List<WeightedRecipies> recpies)
        {
            WeightedRecipies = recpies.OrderByDescending(x => x.matchingIngrdientsNum).ToList();
        }
        #endregion
        #region refs
        private List<List<string>> GetAllProductRefs(List<Product> products)
        {
            List<List<string>> allRefs = new List<List<string>>();
            foreach (Product p in products)
            {
                allRefs.Add(GetCustomRefrences(p));
            }
            return allRefs;
        }

        private List<string> GetCustomRefrences(Product product)
        {
            List<string> ProductRefrence = product._CustomReferenceField.Split(",").ToList();
            List<string> returnList = new List<string>();

            foreach (string s in ProductRefrence)
            {
                if (!(s.Length < 2) && !string.IsNullOrWhiteSpace(s))
                {
                    returnList.Add(s);
                }
            }
            return returnList;
        }
        #endregion
    }
}
