using System;
using System.Collections.Generic;
using System.Text;

namespace B3_BiksBudget.Webcrawler.Assisting_classes
{
    /// <summary>
    /// classe containing method for finding diffrent parts of a ingredient
    /// </summary>
    public class Ingrdient_Determiner
    {
        /// <summary>
        /// Finding the amount needed in a raw ingredient string
        /// </summary>
        /// <param name="ingrediens">a raw ingredient string which should include ingredient name amount and which unit the aamount is in</param>
        /// <returns> returns a float if it can find a amount value if not it return a zero</returns>
        #region(Determin ingredient values)
        public float DeterminAmount(String ingrediens)
        {
            String[] SplitString = ingrediens.Split(' ');
            float Amount;
            foreach (String part in SplitString)
            {
                if (float.TryParse(part, out Amount))
                {
                    return Amount;
                }
            }
            return 0;
        }

        /// <summary>
        /// Finding the unit in a raw ingredient string
        /// </summary>
        /// <param name="ingrediens">a raw ingredient string which should include ingredient name amount and which unit the aamount is in</param>
        /// <returns> returns a string that should be a unit such "kg" or "g"</returns>
        public String DeterminUnit(String ingrediens)
        {
            //string[] UnitTypes = new string[]{"tsk","spsk",""}; 
            string[] SplitString = ingrediens.Split(' ');
            
            return SplitString[1];
        }

        /// <summary>
        /// Finding the name in a raw ingredient string
        /// </summary>
        /// <param name="ingrediens">a raw ingredient string which should include ingredient name amount and which unit the aamount is in</param>
        /// <returns> returns a string that should be the name of the ingredient</returns>
        public String DeterminName(String ingrediens)
        {
            String[] SplitString = ingrediens.Split(' ');
            String ReturnString = "";
            for (int i = 2; i < SplitString.Length; i++)
            {
                ReturnString = ReturnString + " " + SplitString[i];

            }
            return ReturnString;
        }
        #endregion
    }
}
