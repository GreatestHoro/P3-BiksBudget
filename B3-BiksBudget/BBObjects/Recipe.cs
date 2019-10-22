using System;
using System.Collections.Generic;
using System.Text;

namespace B3_BiksBudget.BBObjects
{
    class Recipe
    {
        public string _Name;
        public string _description;
        public List<string> _ingrediensList;

        public Recipe(string name, string description, List<string> ingrediensList)
        {
            _Name = name;
            _description = description;
            _ingrediensList = ingrediensList;
        }
    }
}
