using Microsoft.VisualStudio.TestTools.UnitTesting;
using BBGatherer.Webcrawler;
using B3_BiksBudget.Webcrawler.Assisting_classes;
using System.Collections.Generic;

namespace BBGatherer_TEST
{
    [TestClass]
    public class UnitTest1
    {
        #region Cross ref tests
        [TestMethod]
        public void CrossRefrenceFuntionality_Single_TEST()
        {
            CrossRefrenceFuntionality _RefTEST = new CrossRefrenceFuntionality();
            string testString = "lam";
            bool fatalError;

            string Expected = "lam";
            string result = _RefTEST.CheckForValidIndgredients(testString,out fatalError);

            Assert.AreEqual(Expected,result);
        }
        [TestMethod]
        public void CrossRefrenceFuntionality_Pair_TEST()
        {
            CrossRefrenceFuntionality _RefTEST = new CrossRefrenceFuntionality();
            string testString = "hakket okse";
            bool fatalError;

            string Expected = "hakket okse";
            string result = _RefTEST.CheckForValidIndgredients(testString, out fatalError);

            Assert.AreEqual(Expected, result);
        }
        [TestMethod]
        public void CrossRefrenceFuntionality_FatalError_TEST()
        {
            CrossRefrenceFuntionality _RefTEST = new CrossRefrenceFuntionality();
            string testString = "ThisIsMadeToFail";
            bool fatalError;

            _RefTEST.CheckForValidIndgredients(testString, out fatalError);

            bool Expected = true;
            bool result = fatalError;

            Assert.AreEqual(Expected, result);
        }
        #endregion
        #region remove tests
        [TestMethod]
        public void StringStringCleanUpFuntionality_RemoveStringOfLentgh_TEST()
        {
            StringCleanUpFuntionality SCF_TEST = new StringCleanUpFuntionality();
            List<string> exceptions = new List<string>() {"Hi"};
            string testString = "Hi this is an simple example made to test the method";

            string Expected = "this simple example made test the method";
            string result = SCF_TEST.removeSubstringOfLentgh(testString,2, exceptions);

            Assert.Equals(Expected, result);
        }
        [TestMethod]
        public void StringStringCleanUpFuntionality_RemoveSubstring_TEST()
        {
            StringCleanUpFuntionality SCF_TEST = new StringCleanUpFuntionality();
            string testString = "This is not a test";
            string RemoveString = "not";

            string Expected = "This is a test";
            string result = SCF_TEST.RemoveSubstring(testString, RemoveString);

            Assert.AreEqual(Expected, result);
        }
        [TestMethod]
        public void StringStringCleanUpFuntionality_RemoveEverythingAfter_TEST()
        {
            StringCleanUpFuntionality SCF_TEST = new StringCleanUpFuntionality();
            string testString = "milk or bacon";
            string RemoveStrat = "or";

            string Expected = "milk";
            string result = SCF_TEST.RemoveEverythingAfter(testString,RemoveStrat);

            Assert.AreEqual(Expected, result);
        }

        [TestMethod]
        public void StringStringCleanUpFuntionality_RemoveInBetween_TEST()
        {
            StringCleanUpFuntionality SCF_TEST = new StringCleanUpFuntionality();
            string testString = "hi this is (not) a test";
            char a = '(';
            char b = ')';

            string Expected = "hi this is  a test";
            string result = SCF_TEST.RemoveInBetween(testString,a,b);

            Assert.AreEqual(Expected, result);
        }

        [TestMethod]
        public void StringStringCleanUpFuntionality_RemoveCharFromString_TEST()
        {
            StringCleanUpFuntionality SCF_TEST = new StringCleanUpFuntionality();
            string testString = "banana";
            char a = 'a';

            string Expected = "bnn";
            string result = SCF_TEST.RemoveCharFromString(testString, a);

            Assert.AreEqual(Expected, result);
        }

        [TestMethod]
        public void StringStringCleanUpFuntionality_RemoveIfFirstInString_TEST()
        {
            StringCleanUpFuntionality SCF_TEST = new StringCleanUpFuntionality();
            string testString = "en banana";
            string test = "en";

            string Expected = "banana";
            string result = SCF_TEST.RemoveIfFirstInString(testString,test);

            Assert.AreEqual(Expected, result);
        }
        #endregion
    }
}
