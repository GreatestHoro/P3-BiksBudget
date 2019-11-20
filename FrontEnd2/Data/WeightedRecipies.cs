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

        public WeightedRecipies(Recipe _recipie)
        {
            this._recipie = _recipie;
            complexity = _recipie._ingredientList.Count();
        }

        public void MatchFound(Ingredient ingrdient)
        {
            matchingIngrdient.Add(ingrdient);
            matchingIngrdientsNum = matchingIngrdient.Count();
        }
    }
}
