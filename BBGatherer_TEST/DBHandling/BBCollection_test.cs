using Microsoft.VisualStudio.TestTools.UnitTesting;
using BBCollection.DBHandling;
using System;
using System.Collections.Generic;
using System.Text;
using BBCollection.DBConncetion;
using System.Threading.Tasks;
using BBCollection.BBObjects;

namespace BBCollection.DBHandling.Tests
{
    [TestClass()]
    public class BBCollection_test
    {
        DatabaseConnect databaseConnect = new DatabaseConnect();
        SQLConnect sqlConnect = new SQLConnect();

        #region Product_Test
        [TestMethod]
        public async Task GetProductsNull_Test()
        {
            List<Product> products = new List<Product>();
            string nullString = null;

            products = await databaseConnect.Product.GetList(nullString);

            Assert.IsTrue(products.Count > 0);
            CollectionAssert.AllItemsAreUnique(products);
        }

        [TestMethod]
        public async Task GetProductsNotNull_Test()
        {
            List<Product> products = new List<Product>();
            string testString = "mælk";

            products = await databaseConnect.Product.GetList(testString);

            Assert.IsTrue(products.Count > 0);
            CollectionAssert.AllItemsAreUnique(products);
        }

        [TestMethod]
        public async Task GetMultipleLists_Test()
        {
            List<Product> products1 = new List<Product>();
            List<Product> products2 = new List<Product>();
            List<Product> products3 = new List<Product>();
            List<Product> products4 = new List<Product>();
            List<Product> products5 = new List<Product>();

            string test1 = "mælk";
            string test2 = "chokolade";
            string test3 = "is";
            string test4 = "kaffe";
            string test5 = "Oksekød";

            products1 = await databaseConnect.Product.GetList(test1);
            products2 = await databaseConnect.Product.GetList(test2);
            products3 = await databaseConnect.Product.GetList(test3);
            products4 = await databaseConnect.Product.GetList(test4);
            products5 = await databaseConnect.Product.GetList(test5);

            Assert.IsTrue(products1.Count > 0);
            CollectionAssert.AllItemsAreUnique(products1);
            Assert.IsTrue(products2.Count > 0);
            CollectionAssert.AllItemsAreUnique(products2);
            Assert.IsTrue(products3.Count > 0);
            CollectionAssert.AllItemsAreUnique(products3);
            Assert.IsTrue(products4.Count > 0);
            CollectionAssert.AllItemsAreUnique(products4);
            Assert.IsTrue(products5.Count > 0);
            CollectionAssert.AllItemsAreUnique(products5);
        }

        [TestMethod]
        public async Task GetNonsense_Test()
        {
            List<Product> products = new List<Product>();
            string testString = "lædfslsgfds¤#!'as23121";

            products = await databaseConnect.Product.GetList(testString);

            Assert.IsTrue(products.Count == 0);
            CollectionAssert.AllItemsAreUnique(products);
        }

        [TestMethod]
        public async Task AddProduct_Test()
        {
            Product testProduct = new Product("test", 50);

            await databaseConnect.Product.Add(testProduct);

            List<Product> products = new List<Product>();
            products = await databaseConnect.Product.GetList("test");

            Assert.IsTrue(products.Count > 0);
            CollectionAssert.AllItemsAreUnique(products);
        }

        [TestMethod]
        public async Task GetRange_Test()
        {
            List<Product> products = new List<Product>();

            products = await databaseConnect.Product.GetRange("mælk", 10, 0);

            Assert.IsTrue(products.Count == 10);
            CollectionAssert.AllItemsAreUnique(products);
        }

        [TestMethod]
        public async Task GetReference_Test()
        {
            List<Product> products = new List<Product>();
            string testString = "mælk";

            products = await databaseConnect.Product.ReferencesAsync(testString);

            Assert.IsTrue(products.Count > 0);
            CollectionAssert.AllItemsAreUnique(products);
        }

        [TestMethod]
        public async Task MultipleReferences_Test()
        {
            List<Product> products = new List<Product>();
            List<string> testList = new List<string>();

            string testString = "mælk"; string testString2 = "let";

            testList.Add(testString); testList.Add(testString2);

            products = await databaseConnect.Product.MultipleReferencesAsync(testList);

            Assert.IsTrue(products.Count > 0);
            CollectionAssert.AllItemsAreUnique(products);
        }
        #endregion

        #region Recipe_Test
        [TestMethod()]
        public async Task GetReferenceNullTest()
        {
            List<string> strings = new List<String>();

            string str1 = null;
            string str2 = null;
            string str3 = null;

            strings.Add(str1);
            strings.Add(str2);
            strings.Add(str3);

            List<Recipe> recipes = await databaseConnect.Recipe.GetReferencesAsync(strings);

            Assert.IsTrue(recipes.Count == 0);
        }

        [TestMethod()]
        public async Task GetOneReferenceTest()
        {
            List<string> strings = new List<String>();

            string str1 = "mælk";

            strings.Add(str1);

            List<Recipe> recipes = await databaseConnect.Recipe.GetReferencesAsync(strings);

            Assert.IsTrue(recipes.Count > 0);
            CollectionAssert.AllItemsAreUnique(recipes);
        }

        [TestMethod()]
        public async Task GetMultipleReferencesTest()
        {
            List<string> strings = new List<String>();

            string str1 = "mælk";
            string str2 = "ost";
            string str3 = "salt";

            strings.Add(str1);
            strings.Add(str2);
            strings.Add(str3);

            List<Recipe> recipes = await databaseConnect.Recipe.GetReferencesAsync(strings);

            Assert.IsTrue(recipes.Count > 0);
            CollectionAssert.AllItemsAreUnique(recipes);
        }

        [TestMethod()]
        public void GetListTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetRangeTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetIngredientsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void AddListTest()
        {
            Assert.Fail();
        }

        #endregion

        #region Shoppinglist_Test
        [TestMethod()]
        public void AddListTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetListTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteTest()
        {
            Assert.Fail();
        }


        #endregion

        #region Storage_Test
        [TestMethod()]
        public void UpdateTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetListTest2()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void AddListTest2()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteTest1()
        {
            Assert.Fail();
        }
        #endregion

        #region User_Test
        [TestMethod()]
        public void AddTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void VerifyTest()
        {
            Assert.Fail();
        }
        #endregion

    }
}