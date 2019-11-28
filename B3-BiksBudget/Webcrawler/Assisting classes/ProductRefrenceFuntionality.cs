using BBCollection;
using BBCollection.BBObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using BBCollection.DBHandling;
using BBCollection.DBConncetion;

namespace B3_BiksBudget.Webcrawler.Assisting_classes
{
    class ProductRefrenceFuntionality
    {
        DatabaseConnect dc = new DatabaseConnect();
        #region Interpret
        public string InterpretAndEditProductRef(string _ref)
        {
            string[] refs = _ref.Split(",");
            List<string> newRef = new List<string>();
            string returnRef = "";
            refs.OrderByDescending(x => x.Length);
            newRef.AddRange(GetBiggestStrings(refs, 2));

            foreach (string s in newRef)
            {
                returnRef += s + ",";
            }
            return returnRef;
        }
        private List<string> GetBiggestStrings(string[] sortedStrings, int Levels)
        {
            List<string> returnList = new List<string>();
            returnList.AddRange(GetBiggest(sortedStrings));
            while (Levels-- > 0)
            {
                returnList = GetNextBiggest(sortedStrings, returnList.Last());
            }
            return returnList;
        }
        private List<string> GetBiggest(string[] strings)
        {
            List<string> retrunList = new List<string>();
            int biggestYet = strings.First().Length;
            foreach (string s in strings)
            {
                if (biggestYet == s.Length)
                {
                    retrunList.Add(s);
                }
                else
                {
                    break;
                }
            }
            return retrunList;
        }

        private List<string> GetNextBiggest(string[] strings, string StringList)
        {
            List<string> retrunList = new List<string>();
            bool flag = false;
            int biggestYet = StringList.Length;

            foreach (string s in strings)
            {
                if (biggestYet != s.Length && !flag)
                {
                    flag = true;
                    biggestYet = s.Length;
                }

                if (flag && biggestYet == s.Length)
                {
                    retrunList.Add(s);
                }
                else
                {
                }
            }
            return retrunList;
        }
        #endregion

        public string UpdateProductRefrence(string CurrentRefrence, string searchterm)
        {
            if (!String.IsNullOrWhiteSpace(CurrentRefrence))
            {
                if (!CurrentRefrence.Contains(searchterm + ","))
                {

                    CurrentRefrence += searchterm + ",";
                }
                return CurrentRefrence;
            }
            else
            {
                return searchterm + ",";
            }

        }
    }
}
