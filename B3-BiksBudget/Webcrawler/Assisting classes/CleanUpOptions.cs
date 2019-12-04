using System;
using System.Collections.Generic;
using System.Text;

namespace B3_BiksBudget.Webcrawler.Assisting_classes
{
    class CleanUpOptions
    {
        /// <summary>
        /// Method that contains the default charaters that should get removed from a string
        /// </summary>
        /// <param name="option">This is a string, it can be one of tree diffrent values, default, addetiv og custom</param>
        /// <param name="custom">A list of char that will be returned or added to the back of default or simply ignored depending on option</param>
        /// <returns>returns a list of chars</returns>
        public List<char> CleanUpOptionsChar(string option, List<char> custom)
        {
            List<char> _default = new List<char>(){ ',', '.', '+', '-', '!', '?', '&', '%', '–', '"', '*', '(', ')', '/', '½' };
            if (option.Equals("default"))
            {
                return _default;
            }
            else if (option.Equals("addetiv"))
            {
                _default.AddRange(custom);
                return _default;
            }
            else
            {
                return custom;
            }
        }

        /// <summary>
        /// Method that contains the default strings that should split the string, and delete everything after
        /// </summary>
        /// <param name="option">This is a string, it can be one of tree diffrent values, default, addetiv og custom</param>
        /// <param name="custom">A list of string that will be returned or added to the back of default or simply ignored depending on option</param>
        /// <returns></returns>
        public List<string> StringSplit(string option, List<string> custom) 
        {
            List<string> _defualt = new List<string>() {"eller","i","med","gerne","fra","fx","ekstra", "el", "og" , "såsom","uden"};
            if (option.Equals("default"))
            {
                return _defualt;
            }
            else if (option.Equals("addetiv"))
            {
                _defualt.AddRange(custom);
                return _defualt;
            }
            else
            {
                return custom;
            }
        }

        /// <summary>
        /// Method that contains the default strings that should get removed from another string
        /// </summary>
        /// <param name="option">This is a string, it can be one of tree diffrent values, default, addetiv og custom</param>
        /// <param name="custom">A list of string that will be returned or added to the back of default or simply ignored depending on option</param>
        /// <returns>returns a list of strings</returns>
        public List<string> SubStringRemove(string option, List<string> custom)
        {
            List<string> _defualt = new List<string>() { "l", "for", "par", "I", "op", "gr", "et", "de", "a", "à", "á", "i", "let", "på", "dl", "hel", "der", "er", "kg", "små", "evt", "ekstra", "tsk", "almindelig", "til", "el", "af", "stk", "ud", "g", "ca", "a", "pr", "sk", "hele", "cm", "nye", "frisk", "st", "u", "en", "uden", "ben", "løse" };
            if (option.Equals("default"))
            {
                return _defualt;
            }
            else if (option.Equals("addetiv"))
            {
                _defualt.AddRange(custom);
                return _defualt;
            }
            else
            {
                return custom;
            }
        }

        /// <summary>
        /// Method that contains the default strings that should get removed from a string if they are present in the first part of the string
        /// </summary>
        /// <param name="option">This is a string, it can be one of tree diffrent values, default, addetiv og custom</param>
        /// <param name="custom">A list of string that will be returned or added to the back of default or simply ignored depending on option</param>
        /// <returns>returtns a list of strings</returns>
        public List<string> RemoveIfFirst(string option, List<string> custom)
        {
            List<string> _defualt = new List<string>() { "af ", "after " };
            if (option.Equals("default"))
            {
                return _defualt;
            }
            else if (option.Equals("addetiv"))
            {
                _defualt.AddRange(custom);
                return _defualt;
            }
            else
            {
                return custom;
            }
        }

        /// <summary>
        /// Method that contains the default strings that shouldnt get removed, by the remove methods
        /// </summary>
        /// <param name="option">This is a string, it can be one of tree diffrent values, default, addetiv og custom</param>
        /// <param name="custom">A list of strings that will be returned or added to the back of default or simply ignored depending on option</param>
        /// <returns>returtns a list of strings</returns>
        public List<string> Exeptions(string option, List<string> custom)
        {
            List<string> _defualt = new List<string>() { "æg", "ål", "øl", "te" };
            if (option.Equals("default"))
            {
                return _defualt;
            }
            else if (option.Equals("addetiv"))
            {
                _defualt.AddRange(custom);
                return _defualt;
            }
            else 
            {
                return custom;
            }
        }


    }
}
