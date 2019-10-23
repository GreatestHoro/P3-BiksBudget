using System;
using System.Collections.Generic;
using System.Text;

namespace B3_BiksBudget.BBObjects
{
    class Ingriedient
    {
        public float _Amount;
        public string _unit;
        public string _IngredientName;

        public Ingriedient(String IngredientName,string unit, float Amount) 
        {
            _IngredientName = IngredientName;
            _unit = unit;
            _Amount = Amount;
        }
    }
}
