using BBCollection.BBObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BBGatherer_TEST
{
    [TestClass]
    public class EmptyFridge_TEST
    {
        [TestMethod]
        public async Task EmptyFridge_GetRelevantRecepiesFromProd_TEST()
        {
            List<Product> Storage = InstantiateStorage();
            EmptyFridgeFuntionality emptyFridgeFuntionality = new EmptyFridgeFuntionality(Storage);
            List<WeightedRecipies> resultRecepies = new List<WeightedRecipies>();

            bool exspected = true, result = false;

            foreach (var item in emptyFridgeFuntionality.allRefs)
            {
                resultRecepies.AddRange(await emptyFridgeFuntionality.GetRelevantRecepiesFromProd(item));
            }
            result = CheckIfRerencesArePresentInRecepies(resultRecepies, Storage);

            Assert.AreEqual(result, exspected);
        }

        [TestMethod]
        public async Task EmptyFridge_SortMatchnum_TEST()
        {
            List<Product> Storage = InstantiateStorage();

            int testVlaue;
            bool exspected = true, result = false;

            EmptyFridgeFuntionality emptyFridgeFuntionality = new EmptyFridgeFuntionality(Storage);
            List<WeightedRecipies> resultRecepies = new List<WeightedRecipies>();

            foreach (var item in emptyFridgeFuntionality.allRefs)
            {
                resultRecepies.AddRange(await emptyFridgeFuntionality.GetRelevantRecepiesFromProd(item));
            }

            emptyFridgeFuntionality.SortByMatchnum(resultRecepies);
            resultRecepies = await emptyFridgeFuntionality.GetSortedList();

            testVlaue = resultRecepies.First().matchingIngrdientsNum;
            foreach (WeightedRecipies w in resultRecepies)
            {
                if (w.matchingIngrdientsNum == testVlaue || w.matchingIngrdientsNum < testVlaue)
                {
                    result = true;
                }
                else
                {
                    result = false;
                    break;
                }
            }

            Assert.AreEqual(result, exspected);
        }

        [TestMethod]
        public async Task EmptyFridge_SortPmatch_TEST()
        {
            List<Product> Storage = InstantiateStorage();

            float testVlaue;
            bool result = false;
            bool exspected = true;

            EmptyFridgeFuntionality emptyFridgeFuntionality = new EmptyFridgeFuntionality(Storage);
            List<WeightedRecipies> resultRecepies = new List<WeightedRecipies>();

            foreach (var item in emptyFridgeFuntionality.allRefs)
            {
                resultRecepies.AddRange(await emptyFridgeFuntionality.GetRelevantRecepiesFromProd(item));
            }

            emptyFridgeFuntionality.SortByPmatch(resultRecepies);
            resultRecepies = await emptyFridgeFuntionality.GetSortedList();

            testVlaue = resultRecepies.First().pMatch;
            foreach (WeightedRecipies w in resultRecepies)
            {
                if (w.pMatch == testVlaue || w.pMatch < testVlaue)
                {
                    result = true;
                }
                else
                {
                    result = false;
                    break;
                }
            }

            Assert.AreEqual(result, exspected);
        }

        #region Helper Merhods
        private List<Product> InstantiateStorage()
        {
            Product product = new Product();
            product._productName = "test1";
            product._CustomReferenceField = "salt";
            Product product1 = new Product();
            product1._productName = "test2";
            product1._CustomReferenceField = "peber";
            Product product2 = new Product();
            product2._productName = "test3";
            product2._CustomReferenceField = "okse";

            return new List<Product>() { product, product1, product2 };
        }

        private bool CheckIfRerencesArePresentInRecepies(List<WeightedRecipies> recepies, List<Product> storage)
        {
            bool SaltFound = false, PeberFound = false, OkseFound = false;
            bool result = false;

            foreach (WeightedRecipies w in recepies)
            {
                foreach (Ingredient I in w._recipie._ingredientList)
                {
                    if (I._ingredientName.ToLower().Contains("salt"))
                    {
                        SaltFound = true;
                    }

                    if (I._ingredientName.ToLower().Contains("okse"))
                    {
                        PeberFound = true;
                    }

                    if (I._ingredientName.ToLower().Contains("peber"))
                    {
                        OkseFound = true;
                    }

                    if (SaltFound && PeberFound && OkseFound)
                    {
                        result = true;
                        goto Done;
                    }
                }
            }
        Done:;
            return result;
        }
        #endregion

    }
}
