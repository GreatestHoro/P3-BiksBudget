using System.Collections.Generic;

namespace BBDatabase.DBObjects
{
    class Recipe
    {
        public string _Name;
        public string _description;
        public List<Ingredient> _ingredientList;
        public float _PerPerson;
        public int _recipeID;


        public Recipe(int recipeID, string name, string description, List<Ingredient> ingredientList, float PerPerson)
        {
            _recipeID = recipeID;
            _Name = name;
            _description = description;
            _ingredientList = ingredientList;
            _PerPerson = PerPerson;
        }
    }
}
