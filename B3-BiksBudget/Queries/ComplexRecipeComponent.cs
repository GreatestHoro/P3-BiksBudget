using System;
using System.Collections.Generic;
using System.Text;
using BBCollection.BBObjects;

namespace B3_BiksBudget.BBGatherer.Queries
{
    public class ComplexRecipeComponent
    {
        public double _recipeCost { get; set; }
        public Dictionary<string, List<Product>> _products = new Dictionary<string, List<Product>>();

        public ComplexRecipeComponent(double recipeCost, Dictionary<string, List<Product>> products)
        {
            _recipeCost = recipeCost;
            _products = products;
        }

        public ComplexRecipeComponent()
        {

        }
    }
}
