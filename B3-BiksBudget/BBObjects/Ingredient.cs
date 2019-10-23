using System;
using System.Collections.Generic;
using System.Text;

namespace B3_BiksBudget.BBObjects
{
    class Ingredient
    {
        public float _Amount;
        public string _unit;
        public string _IngredientName;

        public Ingredient(String IngredientName,string unit, float Amount)
        {
            _IngredientName = IngredientName;
            _unit = unit;
            _Amount = Amount;
        }
    }
}
