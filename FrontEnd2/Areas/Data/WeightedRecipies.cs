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
        public float pMatch { get; set; }
        public int matchingIngrdientsNum { get; set; }
        public List<Ingredient> matchingIngrdient = new List<Ingredient>();
        public Dictionary<Ingredient, List<string>> ingrdients = new Dictionary<Ingredient, List<string>>();
        public List<string> custmomRef = new List<string>();
        public List<Product> products = new List<Product>();

        public WeightedRecipies(Recipe _recipie, Dictionary<Ingredient, List<string>> ingrdients)
        {
            this._recipie = _recipie;
            this.ingrdients = ingrdients;
            matchingIngrdient.AddRange(ingrdients.Keys);
            foreach (Ingredient i in ingrdients.Keys) 
            {
                custmomRef.AddRange(ingrdients[i]);
            }
            matchingIngrdientsNum = matchingIngrdient.Count;
            complexity = _recipie._ingredientList.Count();
            calculateP();
        }

        public void calculateP()
        {
            int matchs = 0,NonMatches = 0,allTested = 0;

            foreach (Ingredient I in _recipie._ingredientList)
            {
                if (compareIngredientsWtihRefs(I, custmomRef))
                {
                    matchs++;
                    allTested++;
                }
                else
                {
                    NonMatches++;
                    allTested++;
                }
            }
            if (matchs == 0)
            {
                pMatch = 0;
            }
            else if (NonMatches == 0)
            {
                pMatch = 100;
            }
            else 
            {
                pMatch = (float)matchs / (float)allTested;
            }


        }

        private bool compareIngredientsWtihRefs(Ingredient ingredient, List<string> refs)
        {
            foreach (string _ref in refs)
            {
                if (ingredient._ingredientName.Contains(_ref))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
