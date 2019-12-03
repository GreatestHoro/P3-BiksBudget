using BBCollection;
using BBCollection.BBObjects;
using BBCollection.DBConncetion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BBGatherer_TEST
{
    [TestClass]
    public class BBCollection_test
    {
        DatabaseConnect databaseConnect = new DatabaseConnect();
        SQLConnect sqlConnect = new SQLConnect();

        #region Setup_Test_Environment 
        [TestInitialize]
        public void Setup_Database()
        {

        }
        #endregion

        #region Product_Test
        [TestMethod]
        public void GetProductsNull_Test()
        {

            List<Product> products = new List<Product>();
            string nullString = null;
            //products = Task.Run(async () => { return await databaseConnect.Product.GetList(nullString); });

            Assert.AreEqual(0, products.Count);
        }
        #endregion

        #region Recipe_Test
        #endregion

        #region Shoppinglist_Test
        #endregion

        #region Storage_Test
        #endregion

        #region User_Test
        #endregion


    }
}
