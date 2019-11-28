using System;
using System.Collections.Generic;
using System.Text;

namespace B3_BiksBudget.Webcrawler.Assisting_classes
{
    class Ingrdient_Determiner
    {
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

        public String DeterminUnit(String ingrediens)
        {
            String[] SplitString = ingrediens.Split(' ');
            return SplitString[1];
        }

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
