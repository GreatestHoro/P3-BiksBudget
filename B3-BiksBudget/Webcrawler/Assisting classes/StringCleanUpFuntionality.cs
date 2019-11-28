using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Net.Http;
using HtmlAgilityPack;

namespace B3_BiksBudget.Webcrawler.Assisting_classes
{
    class StringCleanUpFuntionality
    {
        string options;
        List<char> customCharList = new List<char>();
        List<string> CustomStringSplit = new List<string>();
        List<string> CustomSubStringToRemove = new List<string>();
        List<string> CustomRemoveIfFirst = new List<string>();
        List<string> CustomException = new List<string>();

        CleanUpOptions CleanUpValues = new CleanUpOptions();
        public StringCleanUpFuntionality() 
        {
            options = "default";
        }

        public StringCleanUpFuntionality(string option,List<char> customCharList,List<string> CustomStringSplit,List<string> CustomSubStringToRemove,List<string> CustomRemoveIfFirst, List<string> CustomException)
        {
            options = option;
            this.customCharList = customCharList;
            this.CustomStringSplit = CustomStringSplit;
            this.CustomSubStringToRemove = CustomSubStringToRemove;
            this.CustomRemoveIfFirst = CustomRemoveIfFirst;
        }

        #region stringCleanup
        public String NameCleanUp(String name)
        {
            List<char> _char = CleanUpValues.CleanUpOptionsChar(options, customCharList);
            List<string> splitString = CleanUpValues.StringSplit(options, CustomStringSplit);
            List<string> removeSub = CleanUpValues.SubStringRemove(options, CustomSubStringToRemove);
            List<string> removeIfFisrt = CleanUpValues.RemoveIfFirst(options, CustomRemoveIfFirst);
            List<string> exeptions = CleanUpValues.Exeptions(options,CustomException);

            name = new String(name.Where(c => c != '-' && (c < '0' || c > '9')).ToArray());

            name = RemoveInBetween(name, '(', ')');
            name = RemoveInBetween(name, '"', '"');
            foreach (string s in splitString)
            {
                name = RemoveEverythingAfter(name, s);
            }
            foreach (char c in _char)
            {
                name = RemoveCharFromString(name, c);
            }
            foreach (string s in removeSub)
            {
                name = RemoveSubstring(name, s);
            }
            foreach (string s in removeIfFisrt)
            {
                name = RemoveIfFirstInString(name, s);
            }

            name = removeSubstringOfLentgh(name, 2, exeptions);

            name = name.ToLower();

            return name.Trim();

        }

        public String EdgeCaseCleanUp(String name)
        {
            name = new String(name.Where(c => c != '-' && (c < '0' || c > '9')).ToArray());
            List<String> Substring = new List<string>() { "u", "dl", "ca", "á", "g", "kg", "til" };
            foreach (String s in Substring)
            {
                name = RemoveSubstring(name.Trim(), s);
            }

            if (name.Trim().Equals("i tern") || name.Trim().Equals("i") || name.Trim().Equals("med"))
            {
                name = "";
            }
            return name;
        }

        public float CleanUpPerPerson(HtmlNodeCollection _PerPerson)
        {

            if (_PerPerson != null)
            {
                String PerPerson = _PerPerson.ElementAt<HtmlNode>(0).InnerText;
                String cleanUp = "";
                float numb;
                String[] characters = PerPerson.Split(' ', '&', '-');
                foreach (String c in characters)
                {
                    if (float.TryParse(c, out numb))
                    {
                        return numb;
                    }
                }
                return float.Parse(cleanUp);
            }
            else
            {
                return 4;
            }

        }
        #endregion

        #region(remove)
        private string removeSubstringOfLentgh(string name, int length, List<string> exceptions)
        {
            bool flag = true;
            string[] words = name.Split(" ");
            string ReturnString = "";
            foreach (string word in words)
            {
                if (word.Length < length)
                {
                    foreach (string sE in exceptions)
                    {
                        if (sE.Equals(name))
                        {
                            flag = true;
                        }
                    }
                }
                else
                {
                    flag = true;
                }

                if (flag)
                {
                    ReturnString += word + " ";
                }
            }
            return ReturnString;
        }

        private string RemoveSubstring(String name, String substring)
        {
            String[] Words = name.Split(" ");
            string ReturnString = "";
            foreach (string w in Words)
            {
                if (!w.Equals(substring))
                {
                    ReturnString = ReturnString + w + " ";
                }
            }

            return ReturnString;
        }

        private string RemoveEverythingAfter(string _string, string RemoveStart)
        {
            String[] Words = _string.Split(" ");
            String _return = "";
            bool KeyFlag = false;
            foreach (String s in Words)
            {
                if (s.Equals(RemoveStart))
                {
                    KeyFlag = true;
                }
                if (!KeyFlag)
                {
                    _return = _return + s + " ";
                }
            }
            return _return;
        }

        private String RemoveInBetween(string name, char a, char b)
        {
            char[] chars = name.ToCharArray();
            bool ParenthesesFlag = false;
            String _string = "";

            foreach (char character in chars)
            {
                if (ParenthesesFlag)
                {
                    if (character.Equals(b))
                    {
                        ParenthesesFlag = false;
                    }
                }
                else if (character.Equals(a))
                {
                    ParenthesesFlag = true;
                }
                else
                {
                    _string += character;
                }
            }
            return _string;
        }

        private String RemoveCharFromString(string _string, char _char)
        {
            Char[] charArray = _string.ToCharArray();
            string returnString = "";

            foreach (Char c in charArray)
            {
                if (c != _char)
                {
                    returnString = returnString + c;
                }
            }

            return returnString;
        }

        private String RemoveIfFirstInString(string name, string Remove)
        {
            char[] _name = name.Trim().ToCharArray();
            char[] _Remove = Remove.ToCharArray();
            int i = 0;

            if (name.Contains(Remove))
            {
                foreach (char r in _Remove)
                {
                    i++;
                    if (r == _name[i])
                    {
                        _name[i] = '*';
                    }
                    else
                    {
                        return name.Trim();
                    }
                }
            }

            return RemoveCharFromString(name, '*').Trim(); ;
        }
    }
    #endregion
}
