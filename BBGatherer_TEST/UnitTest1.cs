using Microsoft.VisualStudio.TestTools.UnitTesting;
using BBGatherer.Webcrawler;
using B3_BiksBudget.Webcrawler.Assisting_classes;

namespace BBGatherer_TEST
{
    [TestClass]
    public class UnitTest1
    {
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

        public void CrossRefrenceFuntionality_Pair_TEST()
        {
            CrossRefrenceFuntionality _RefTEST = new CrossRefrenceFuntionality();
            string testString = "hakket okse";
            bool fatalError;

            string Expected = "hakket okse";
            string result = _RefTEST.CheckForValidIndgredients(testString, out fatalError);

            Assert.AreEqual(Expected, result);
        }

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
    }
}
