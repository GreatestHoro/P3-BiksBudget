using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace BBCollection.BBObjects
{
    public class Ingredient
    {
        public int _id;
        public float _amount;
        public string _unit;
        public string _ingredientName;

        public Ingredient(int id, string ingredientName, string unit, float amount)
        {
            _id = id;
            _ingredientName = ingredientName;
            _unit = unit;
            _amount = amount;
        }

        public Ingredient()
        {

        }

        public Ingredient(string ingredientName, string unit, float amount)
        {
            _ingredientName = ingredientName;
            _unit = unit;
            _amount = amount;
        }
    }
}
