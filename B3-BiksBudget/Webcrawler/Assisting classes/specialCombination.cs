using BBCollection;
using System;
using System.Collections.Generic;


namespace B3_BiksBudget.Webcrawler.Assisting_classes
{
    class specialCombination
    {
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
        private string GetCombination(string[] str, int size, int i)
        {
            string ReturnString = "";
            foreach (string word in Combination(str, size, i))
            {
                ReturnString = ReturnString + " " + word;
            }
            return ReturnString.Trim(); ;
        }
        private List<string> Combination(string[] str, int size, int i)
        {
            List<string> Comb = new List<string>();
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
