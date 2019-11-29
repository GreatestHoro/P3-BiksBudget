using BBCollection;
using System;
using System.Collections.Generic;


namespace B3_BiksBudget.Webcrawler.Assisting_classes
{
    /// <summary>
    /// A class that contains method about a special way to combine diffrent word from the searchterm.
    /// Example: if we have a string "hi this is a comment" the reslting out put would first be each word the each word connected with and adjencent word and then one more.
    /// So for this exampe it would look somethin like this {"hi","this","is","a","comment","hi this","this is","is a","a comment","hi this is","this is a","is a comment","hi this is a","this is a comment","hi this is a comment"}
    /// </summary>
    public class specialCombination
    {
        /// <summary>
        /// Gets all the combinations given a string
        /// </summary>
        /// <param name="name"></param>
        /// <returns>returns a list of the combinations as shown in the example</returns>
        public List<string> GetAllCombinations(string name)
        {
            string[] str = name.Split(" ");
            int CombinationSize = 1;
            int Start = 0;
            List<string> Combinations = new List<string>();

            while (CombinationSize <= str.Length)
            {
                while (Start <= str.Length - CombinationSize)
                {
                    Combinations.Add(GetCombination(str, CombinationSize, Start++));
                }
                Start = 0;
                CombinationSize++;
            }
            return Combinations;
        }
        /// <summary>
        /// This is a helper method for the recursive method  Combination().
        /// </summary>
        /// <param name="str">a string array</param>
        /// <param name="size"> the amount of words wanted in the combinations</param>
        /// <param name="i">The starting position in the array</param>
        /// <returns>returns the combination as a string</returns>
        private string GetCombination(string[] str, int size, int i)
        {
            string ReturnString = "";
            foreach (string word in Combination(str, size, i))
            {
                ReturnString = ReturnString + " " + word;
            }
            return ReturnString.Trim(); ;
        }
        /// <summary>
        /// A recursive funtion that generates a combination
        /// </summary>
        /// <param name="str">a string array</param>
        /// <param name="size">the amount of words wanted in the combination</param>
        /// <param name="i">start postition in array</param>
        /// <returns>returns a string list which is the combination</returns>
        private List<string> Combination(string[] str, int size, int i)
        {
            List<string> Comb;
            List<string> ReturnArray = new List<string>();
            if (size == 0)
            {
                return ReturnArray;
            }
            else
            {
                ReturnArray.Add(str[i]);

                Comb = Combination(str, --size, ++i);
                foreach (string strList in Comb)
                {
                    ReturnArray.Add(strList);
                }
                return ReturnArray;

            }
        }
    }
}
