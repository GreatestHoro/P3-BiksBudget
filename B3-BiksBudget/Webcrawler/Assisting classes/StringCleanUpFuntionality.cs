using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Net.Http;
using HtmlAgilityPack;

namespace B3_BiksBudget.Webcrawler.Assisting_classes
{
    /// <summary>
    /// Class containing asn handeling many of the string editing methods mainly used for name cleanup and to remove problematic elements
    /// </summary>
    public class StringCleanUpFuntionality
    {
        string options;
        List<char> customCharList = new List<char>();
        List<string> CustomStringSplit = new List<string>();
        List<string> CustomSubStringToRemove = new List<string>();
        List<string> CustomRemoveIfFirst = new List<string>();
        List<string> CustomException = new List<string>();

        CleanUpOptions CleanUpValues = new CleanUpOptions();

        //Default consturcter: will use all the defualt values
        public StringCleanUpFuntionality() 
        {
            options = "default";
        }

        //option: custom or addetiv this contructor require lists for each of the required parameters
        public StringCleanUpFuntionality(string option,List<char> customCharList,List<string> CustomStringSplit,List<string> CustomSubStringToRemove,List<string> CustomRemoveIfFirst, List<string> CustomException)
        {
            options = option;
            this.customCharList = customCharList;
            this.CustomStringSplit = CustomStringSplit;
            this.CustomSubStringToRemove = CustomSubStringToRemove;
            this.CustomRemoveIfFirst = CustomRemoveIfFirst;
            this.CustomException = CustomException;
        }

        #region stringCleanup
        /// <summary>
        /// Method containing the other clean up and remove methods for a general clean up
        /// </summary>
        /// <param name="name">The string to clean</param>
        /// <returns>A string</returns>
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

        //OUTDATED REMOVE
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

        /// <summary>
        /// Taks the html element and find the pr.person for a recepie
        /// </summary>
        /// <param name="_PerPerson">htmlnode containing perperson</param>
        /// <returns>returns a float</returns>
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
                return 4; //4 because all the unreadable recepies by default is 4 on the website
            }

        }
        #endregion

        #region(remove)
        /// <summary>
        /// Remove substring in a string of given length
        /// </summary>
        /// <param name="name">String to edit</param>
        /// <param name="length">The desired lentgh</param>
        /// <param name="exceptions">Words that should ignore the condition and get passed along anyway</param>
        /// <returns></returns>
        public string removeSubstringOfLentgh(string name, int length, List<string> exceptions)
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
        /// <summary>
        /// Removes a substring from a string
        /// </summary>
        /// <param name="name">string</param>
        /// <param name="substring">substring to remove</param>
        /// <returns></returns>
        public string RemoveSubstring(String name, String substring)
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
        /// <summary>
        /// Remove everything in the string after a given substring
        /// </summary>
        /// <param name="_string"> string</param>
        /// <param name="RemoveStart"> substring</param>
        /// <returns></returns>
        public string RemoveEverythingAfter(string _string, string RemoveStart)
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
        /// <summary>
        /// Removes everything in between two charaters
        /// </summary>
        /// <param name="name">string</param>
        /// <param name="a">frist charater</param>
        /// <param name="b">end charater</param>
        /// <returns></returns>
        public String RemoveInBetween(string name, char a, char b)
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
        /// <summary>
        /// Rmoves a specific charater from a string
        /// </summary>
        /// <param name="_string">string</param>
        /// <param name="_char">char to remove</param>
        /// <returns></returns>
        public String RemoveCharFromString(string _string, char _char)
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
        /// <summary>
        /// removes a substring only if its comes first in the string
        /// </summary>
        /// <param name="name">string</param>
        /// <param name="Remove">substring</param>
        /// <returns></returns>
        public String RemoveIfFirstInString(string name, string Remove)
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
