﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace B3_BiksBudget.Webcrawler.Assisting_classes
{
    public class ProductRefrenceFuntionality
    {
        #region Interpret
        /// <summary>
        /// This method attempts to take a product refrence and make it more clear, and easy to interpret
        /// </summary>
        /// <param name="_ref">raw string of refs each ref is seperated by a ','</param>
        /// <returns>A new ref string that will be shorter and more accurate than the original input</returns>
        public string InterpretAndEditProductRef(string _ref)
        {
            string[] refs = _ref.Split(",");
            List<string> newRef = new List<string>();
            string returnRef = "";
            refs = refs.OrderByDescending(x => x.Length).ToArray();
            newRef.AddRange(GetBiggestStrings(refs, 2));


            foreach (string s in newRef)
            {
                returnRef += s + ",";
            }
            return returnRef;
        }
        /// <summary>
        /// A method that can take a sorted string array and return the biggest of the containing string based on an input
        /// </summary>
        /// <param name="sortedStrings">The sorted string where the biggiest strings are first</param>
        /// <param name="Levels">The input to determin how many diffrent dtring lengs are outputtet</param>
        /// <returns>The output is the a string list only containing the largest string based on the levels
        /// example: level 1 will return all the string of the largest string length,
        /// level 2 would then also return the strings of the next biggiest length</returns>
        public List<string> GetBiggestStrings(string[] sortedStrings, int Levels)
        {
            List<string> returnList = new List<string>();
            returnList.AddRange(GetBiggest(sortedStrings));
            while (--Levels > 0)
            {
                returnList.AddRange(GetNextBiggest(sortedStrings, returnList.Last()));
            }
            return returnList;
        }
        /// <summary>
        /// A method that return the string of the biggist length strings of a string array
        /// </summary>
        /// <param name="strings">a sorted string array where the biggist lentgth is first</param>
        /// <returns>Return s a string list containing all of the strings with the biggist length</returns>
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
        /// <summary>
        /// method that returns the next biggest strings from the array
        /// </summary>
        /// <param name="strings"></param>
        /// <param name="StringList"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Takes the current refrencs string a checks if the search terms is already present inside it
        /// </summary>
        /// <param name="CurrentRefrence">The current refrence string</param>
        /// <param name="searchterm">a searchterm which have gone trough check to determin validity</param>
        /// <returns>returns the new refrence string taht will get added to the database later</returns>
        public string UpdateProductRefrence(string CurrentRefrence, string searchterm)
        {
            if (checkForBannedRefs(searchterm))
            {
                if (!String.IsNullOrWhiteSpace(CurrentRefrence))
                {
                    if (!CurrentRefrence.Contains(searchterm + ","))
                    {

                        CurrentRefrence += searchterm + ",";
                    }
                    return CurrentRefrence;
                }
            }
            return searchterm;
        }

        public string CheckRefrenceContent(string CurrentRefrence)
        {
            if (string.IsNullOrEmpty(CurrentRefrence))
            {
                CurrentRefrence = "";
            }
            string[] _ref = CurrentRefrence.Split(",");
            string returnString = "";
            foreach (string s in _ref)
            {
                if (!string.IsNullOrEmpty(s) && !string.IsNullOrWhiteSpace(s))
                {
                    returnString += s + ",";
                }
            }

            return returnString;
        }

        public bool checkForBannedRefs(string _ref)
        {
            string[] Banned = new string[] { "glutenfri", "gluten", "øko", "økologisk", "coop" };

            foreach (string ban in Banned)
            {
                if (_ref.Equals(ban))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
