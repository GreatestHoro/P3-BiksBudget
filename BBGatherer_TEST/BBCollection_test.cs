using BBCollection.DBConncetion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BBGatherer_TEST
{
    [TestClass]
    public class BBCollection_test
    {
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
