using System;
using System.Collections.Generic;
using System.Text;

namespace B3_BiksBudget.Webcrawler.Assisting_classes
{
    class CleanUpOptions
    {
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
    

        public List<string> StringSplit(string option, List<string> custom) 
        {
            List<string> _defualt = new List<string>() { "eller", "i", "med", "gerne", "fra", "fx", "ekstra", "el", "og" };
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
