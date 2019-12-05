using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BBCollection.BBObjects;
using System.Threading.Tasks;

namespace BBGatherer_TEST
{
    [TestClass]
    public class EmptyFridge_TEST
    {
        [TestMethod]
        public async Task EmptyFridge_GetRelevantRecepiesFromProd_TEST() 
        {
            List<Product> products = new List<Product>();
            Product product = new Product();
            product._productName = "test1";
            product._CustomReferenceField = "salt";
            Product product1 = new Product();
            product1._productName = "test2";
            product1._CustomReferenceField = "peber";
            Product product2 = new Product();
            product2._productName = "test3";
            product2._CustomReferenceField = "okse";
            products.AddRange(new List<Product>() {product,product1,product2});

            EmptyFridgeFuntionality emptyFridgeFuntionality = new EmptyFridgeFuntionality(products);
            List<WeightedRecipies> resultRecepies = new List<WeightedRecipies>();
            bool SaltFound = false;
            bool PeberFound = false;
            bool OkseFound = false;
            bool result = false;
            bool exspected = true;

            foreach (var item in emptyFridgeFuntionality.allRefs)
            {
                resultRecepies.AddRange(await emptyFridgeFuntionality.GetRelevantRecepiesFromProd(item));
            }

            foreach (WeightedRecipies w in resultRecepies) 
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

            Assert.AreEqual(result, exspected);
        }

        [TestMethod]
        public async Task EmptyFridge_SortMatchnum_TEST() 
        {
            List<Product> products = new List<Product>();
            Product product = new Product();
            product._productName = "test1";
            product._CustomReferenceField = "salt";
            Product product1 = new Product();
            product1._productName = "test2";
            product1._CustomReferenceField = "peber";
            Product product2 = new Product();
            product2._productName = "test3";
            product2._CustomReferenceField = "okse";
            products.AddRange(new List<Product>() { product, product1, product2 });
            int testVlaue;
            bool result = false;
            bool exspected = true;

            EmptyFridgeFuntionality emptyFridgeFuntionality = new EmptyFridgeFuntionality(products);
            List<WeightedRecipies> resultRecepies = new List<WeightedRecipies>();

            foreach (var item in emptyFridgeFuntionality.allRefs)
            {
                resultRecepies.AddRange(await emptyFridgeFuntionality.GetRelevantRecepiesFromProd(item));
            }

            emptyFridgeFuntionality.SortByMatchnum(resultRecepies);
            resultRecepies =await emptyFridgeFuntionality.GetSortedList();

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

            Assert.AreEqual(result,exspected);
        }

        [TestMethod]
        public async Task EmptyFridge_SortPmatch_TEST()
        {
            List<Product> products = new List<Product>();
            Product product = new Product();
            product._productName = "test1";
            product._CustomReferenceField = "salt";
            Product product1 = new Product();
            product1._productName = "test2";
            product1._CustomReferenceField = "peber";
            Product product2 = new Product();
            product2._productName = "test3";
            product2._CustomReferenceField = "okse";
            products.AddRange(new List<Product>() { product, product1, product2 });
            float testVlaue;
            bool result = false;
            bool exspected = true;

            EmptyFridgeFuntionality emptyFridgeFuntionality = new EmptyFridgeFuntionality(products);
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
    }
}
