using BBCollection.BBObjects;
using System;
using System.Collections.Generic;

namespace BBCollection.Queries
{
    public class ComplexRecipeComponent
    {
        //member for the total price of the recipe
        private double _recipeCost;

        public double RecipeCost
        {
            get => _recipeCost;
            set
            {
                _recipeCost = Math.Round(value, 2);
            }
        }
        //dictionary for looking up products for the recipe's ingredients by ingredient name
        public Dictionary<string, List<Product>> _products = new Dictionary<string, List<Product>>();

        //constructor for the class v.1
        public ComplexRecipeComponent(double recipeCost, Dictionary<string, List<Product>> products)
        {
            RecipeCost = recipeCost;
            _products = products;
        }
        public ComplexRecipeComponent(double recipeCost)
        {
            RecipeCost = recipeCost;
        }
        //constructur for the class v.2
        public ComplexRecipeComponent()
        {

        }
    }
}
