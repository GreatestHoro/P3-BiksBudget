using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace BBCollection.BBObjects
{
    public class Ingredient : EqualityComparer<Ingredient>
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

        public override bool Equals(Ingredient i1, Ingredient i2)
        {
            if (i1 == null && i2 == null)
                return true;
            else if (i1 == null || i2 == null)
                return false;

            return i1._ingredientName.CompareTo(i2._ingredientName) == 0;
        }

        public override int GetHashCode(Ingredient ingredient)
        {
            int hCode = ingredient._id;
            return hCode.GetHashCode();
        }


    }
}
