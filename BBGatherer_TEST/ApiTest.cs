using BBCollection.BBObjects;
using BBCollection.DBHandling;
using BBCollection.StoreApi.CoopApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BBGatherer_TEST
{
    [TestClass]
    public class ApiTest
    {
        UserData user;
        ControllerFuncionality cFunc = new ControllerFuncionality();
        LoginRegister account = new LoginRegister();
        Filters sallingSearch = new Filters();
        TestData testList = new TestData();
        LoginRegister acount = new LoginRegister();

        List<CoopProduct> coopProduct = new List<CoopProduct>();
        List<Product> sallingProduct = new List<Product>();

        HttpResponseMessage response = new HttpResponseMessage();

        string username = "unitTestUser";

        // IMPORTANT: 
        // File: ConnectoinSettings 
        // bool: _onlineAPI
        // This variable needs to be true to run these tests
        // Also change the username, to ensure a new account is regiseret. Otherwise the register test will fail.
        #region API

        [TestMethod]
        public async Task TestUserApi()
        {
            user = new UserData(username);

            await RegisterUser();
        }

        #region Login

        [TestMethod]
        public async Task RegisterUser()
        {
            User user = new User(username, "unitTestUser123!");

            response = await acount.Register(user);

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task LoginUser()
        {
            User user = new User(username, "unitTestUser123!");

            response = await acount.Login(user);

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        #endregion

        #region GetOnStart

        // This method tests GetShoppinglistOnStart(string userId)
        [TestMethod]
        public async Task GetShoppinglist()
        {
            await user.shoppinglist.GetWhenLoggedIn();

            Assert.IsTrue(user.shoppinglist.shoppinglist.Count > 0);
        }

        //// This method tests GetStorageOnStart(string userId)
        [TestMethod]
        public async Task GetStorage()
        {
            await user.storage.Get();

            Assert.IsTrue(user.storage.storageList.Count > 0);
        }

        #endregion

        #region Put/Post

        // This method tests PutToApi(string productString)
        [TestMethod]
        public async Task PutStorage()
        {
            Product toTest = testList.dummyProductOne;

            toTest._amount = "Full";
            toTest._timeAdded = DateTime.Now.ToString();

            response = await user.storage.AddProduct(toTest);

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        // This method tests SendToApi(string productString)
        [TestMethod]
        public async Task TestDeleteStorage()
        {
            response = await user.storage.DeleteStorage();

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        #endregion

        #region Salling

        //Calls the Salling API and checks whether or not an products are returned
        [TestMethod]
        public void TestSalling()
        {
            sallingProduct = sallingSearch.SearchForProducts("mælk");

            Assert.IsTrue(sallingProduct.Count > 1);
        }

        #endregion

        #region Coop

        //Calls the Coop API and checks whether or not an products are returned
        [TestMethod]
        public void TestCoop()
        {
            CoopDoStuff coop = new CoopDoStuff("f0cabde6bb8d4bd78c28270ee203253f");

            coopProduct = coop.CoopFindEverythingInStore("24073");

            Assert.IsTrue(coopProduct.Count > 1);
        }

        #endregion

        #endregion

        #region HandleDublicats

        // The HandleDublicat method is called to see if can handle a list only containg dublicats
        [TestMethod]
        public void HandleListOfOneDublicat()
        {
            string test = JsonConvert.SerializeObject(cFunc.HandleDublicats(testList.GetInputlistDublicat()));
            string result = JsonConvert.SerializeObject(testList.GetOutputlist(testList.GetInputlistDublicat().Count));

            Assert.AreEqual(result, test);
        }

        // The HandleDublicat method is called to see if it can handle a list containing two objects in a random order
        [TestMethod]
        public void HandleListOfTwoDublicats()
        {
            string test = JsonConvert.SerializeObject(cFunc.HandleDublicats(testList.GetInputlistDublicat()));
            string result = JsonConvert.SerializeObject(testList.GetOutputlist(testList.GetInputlistDublicat().Count));

            Assert.AreEqual(result, test);
        }

        #endregion
    }

    #region TestDataClass
    class TestData
    {
        public Product dummyProductOne = new Product(
                "S84107100100",
                "Carlsberg Elephant Dåse",
                "elephant ds. carlsberg",
                16.95,
                "https://image.prod.iposeninfra.com/bilkaimg.php?pid=18641&imgType=jpeg",
                "Bilka",
                1,
                "carlsberg,carls");

        public Product dummyProductTwo = new Product(
            "S84107100101",
            "Carlsberg Elephant Dåse Test",
            "elephant ds. carlsberg Test",
            16.99,
            "https://image.prod.iposeninfra.com/bilkaimg.php?pid=18641&imgType=jpeg",
            "Bilka Test",
            1,
            "carlsberg,carls");

        List<Product> inputList;
        List<Product> outputList;

        public List<Product> GetInputlistDublicat()
        {
            inputList = new List<Product>()
            {
                new Product(dummyProductOne),
                new Product(dummyProductOne),
                new Product(dummyProductOne),
                new Product(dummyProductOne),
                new Product(dummyProductOne),
                new Product(dummyProductOne),
            };

            return inputList;
        }

        public List<Product> GetInputlistDiffer()
        {
            inputList = new List<Product>()
            {
                new Product(dummyProductOne),
                new Product(dummyProductOne),
                new Product(dummyProductTwo),
                new Product(dummyProductOne),
                new Product(dummyProductOne),
                new Product(dummyProductTwo),
                new Product(dummyProductTwo),
                new Product(dummyProductTwo),
            };

            return inputList;
        }

        public List<Product> GetOutputlist(int amountLeft)
        {
            outputList = new List<Product>()
            {
                new Product(dummyProductOne)
            };

            outputList[0]._amountleft = amountLeft;

            return outputList;
        }

        public List<Product> GetOutputlistTwo(int amountLeft)
        {
            outputList = new List<Product>()
            {
                new Product(dummyProductOne)
            };

            outputList[0]._amountleft = amountLeft / 2;
            outputList[1]._amountleft = amountLeft / 2;


            return outputList;
        }
    }
    #endregion
}
