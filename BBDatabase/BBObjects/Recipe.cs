using System.Collections.Generic;

namespace BBCollection.BBObjects
{
    public class Recipe
    {
        public string _Name { get; }
        public string _description { get; }
        public List<Ingredient> _ingredientList { get; }
        public float _PerPerson { get; }
        public int _recipeID { get; }

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
