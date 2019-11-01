using System;

namespace BBCollection.BBObjects
{
    public class Ingredient
    {
        public float _Amount { get; }
        public string _unit { get; }
        public string _IngredientName { get; }

        public Ingredient(String IngredientName, string unit, float Amount)
        {
            _IngredientName = IngredientName;
            _unit = unit;
            _Amount = Amount;
        }
    }
}
