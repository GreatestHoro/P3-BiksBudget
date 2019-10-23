using System;
using System.Collections.Generic;
using System.Text;

namespace B3_BiksBudget.BBObjects
{
    class Ingredient
    {
        public int _Amount;
        public string _unit;
        public string _IngredientName;

        public Ingredient(String IngredientName,string unit, int Amount) 
        {
            _IngredientName = IngredientName;
            _unit = unit;
            _Amount = Amount;
        }
    }
}
