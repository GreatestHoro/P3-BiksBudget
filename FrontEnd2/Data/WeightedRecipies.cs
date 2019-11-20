using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BBCollection.BBObjects;

namespace FrontEnd2.Data
{
    public class WeightedRecipies
    {
        public Recipe _recipie { get; set; }
        public int complexity { get; set; }
        public int matchingIngrdientsNum { get; set; }
        public List<Ingredient> matchingIngrdient = new List<Ingredient>();
        public List<Product> products = new List<Product>();

        public WeightedRecipies(Recipe _recipie)
        {
            this._recipie = _recipie;
            complexity = _recipie._ingredientList.Count();
        }

        public void MatchFound(Ingredient ingrdient, Product product)
        {
            matchingIngrdient.Add(ingrdient);
            this.products.Add(product);
            matchingIngrdientsNum = matchingIngrdient.Count();
        }
    }
}
