using System;
using System.Collections.Generic;
using System.Text;
using BBCollection.BBObjects;

namespace BBGatherer.Queries
{
    public class ComplexRecipeComponent
    {
        //member for the total price of the recipe
        public double _recipeCost { get; set; }
        //dictionary for looking up products for the recipe's ingredients by ingredient name
        public Dictionary<string, List<Product>> _products = new Dictionary<string, List<Product>>();

        //constructor for the class v.1
        public ComplexRecipeComponent(double recipeCost, Dictionary<string, List<Product>> products)
        {
            _recipeCost = recipeCost;
            _products = products;
        }
        //constructur for the class v.2
        public ComplexRecipeComponent()
        {

        }
    }
}
