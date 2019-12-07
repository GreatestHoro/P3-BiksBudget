using System.Collections.Generic;
using System.Linq;

namespace BBCollection.BBObjects
{
    public class Recipe
    {
        public string _Name;
        public string _description;
        public List<Ingredient> _ingredientList;
        public float _PerPerson;
        public int _recipeID;
        public string image;

        public Recipe(int recipeID, string name)
        {
            _recipeID = recipeID;
            _Name = name;
        }

        public Recipe(int recipeID, string name, string description, List<Ingredient> ingredientList, float PerPerson)
        {
            _recipeID = recipeID;
            _Name = name;
            _description = description;
            _ingredientList = ingredientList;
            _PerPerson = PerPerson;
        }
        public Recipe() { }

        public void deleteDuplicates() 
        {
            IDictionary<string, Ingredient> dict = new Dictionary<string, Ingredient>();
            foreach (Ingredient ind in _ingredientList) 
            {
                if (!dict.ContainsKey(ind._ingredientName)) 
                {
                    dict.Add(ind._ingredientName, ind);
                }
            }
            _ingredientList = dict.Values.ToList();
        }
    }
}
