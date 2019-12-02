using Microsoft.VisualStudio.TestTools.UnitTesting;
using BBCollection.DBHandling;
using BBCollection.BBObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;

namespace BBGatherer_TEST
{
    [TestClass]
    public class ApiTest
    {
        ShoppinlistFunctionality slFunc = new ShoppinlistFunctionality("api/Shoppinglist");
        ShoppinlistFunctionality stFunc = new ShoppinlistFunctionality("api/Storage");
        ControllerFuncionality cFunc = new ControllerFuncionality();
        TestData testList = new TestData();

        HttpResponseMessage response = new HttpResponseMessage();

        #region API

        #region Shoppinglist
        public async Task<HttpResponseMessage> HelpGetShoppinglist()
        {
            response = await stFunc.GetStorageOnStart("USERID");

            return response;
        }

        [TestMethod]
        public async void GetShoppinglist()
        {
            response = await HelpGetShoppinglist();

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        // This method tests SendToApi(string productString)
        [TestMethod]
        public async void PostShoppinglistToStorage()
        {
            response = await HelpGetShoppinglist();

            Product toSend = testList.dummyProductOne;

            response = await slFunc.AddItemToStorage(toSend);

            Assert.IsTrue(response.IsSuccessStatusCode);
        }
        #endregion

        #region Storage

        public async Task<HttpResponseMessage> HelpGetStorage()
        {
            response = await stFunc.GetStorageOnStart("USERID");

            return response;
        }

        // This method tests GetStorageOnStart(string userId)
        [TestMethod]
        public async void GetStorage()
        {
            response = await HelpGetStorage();

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        // This method tests PutToApi(string productString)
        [TestMethod]
        public async void PutStorage()
        {
            await HelpGetStorage();

            Product toTest = testList.dummyProductOne;

            toTest._amount = "Full";
            toTest._timeAdded = DateTime.Now.ToString();

            response = await stFunc.ChangeItemInStorage(toTest);

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        // This method tests DeleteStorage()
        [TestMethod]
        public async void TestDeleteStorage()
        {
            await HelpGetStorage();

            response = await stFunc.DeleteStorage();

            Assert.IsTrue(response.IsSuccessStatusCode);
        }
        #endregion

        #region Salling
        [TestMethod]
        public void TestSalling()
        {

        }

        #endregion

        #region Coop

        [TestMethod]
        public void TestCoop()
        {

        }

        #endregion

        #endregion

        #region HandleDublicats

        [TestMethod]
        public void HandleListOfOneDublicat()
        {
            string test = JsonConvert.SerializeObject(cFunc.HandleDublicats(testList.GetInputlistDublicat()));
            string result = JsonConvert.SerializeObject(testList.GetOutputlist(testList.GetInputlistDublicat().Count));

            Assert.AreEqual(result, test);
        }

        [TestMethod]
        public void HandleListOfTwoDublicats()
        {
            string test = JsonConvert.SerializeObject(cFunc.HandleDublicats(testList.GetInputlistDublicat()));
            string result = JsonConvert.SerializeObject(testList.GetOutputlist(testList.GetInputlistDublicat().Count));

            Assert.AreEqual(result, test);
        }

        #endregion
    }
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
}
