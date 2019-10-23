using System;
using System.Collections.Generic;
using System.Text;

namespace B3_BiksBudget.BBObjects
{
    class Recipe
    {
        public string _Name;
        public string _description;
        public List<Ingredient> _ingrediensList;
        public float _PerPerson;
        public int _recipeID;


        public Recipe(int recipeID, string name, string description, List<Ingredient> ingredientList, float PerPerson)
        {
            _recipeID = recipeID;
            _Name = name;
            _description = description;
            _ingrediensList = ingredientList;
        }
    }
}
