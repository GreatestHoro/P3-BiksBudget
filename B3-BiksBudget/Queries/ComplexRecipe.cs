using System;
using System.Collections.Generic;
using System.Text;
using BBGatherer.Queries;

namespace B3_BiksBudget.BBGatherer.Queries
{
    public class ComplexRecipe : BBCollection.BBObjects.Recipe
    {
        public ComplexRecipeComponent _complexRecipeComponent = new ComplexRecipeComponent();
        public ComplexRecipe(int recipeID, string name, string description, List<BBCollection.BBObjects.Ingredient> ingredientList, float PerPerson, ComplexRecipeComponent complexRecipeComponent) : 
                        base(recipeID, name, description, ingredientList, PerPerson)
        {
            _complexRecipeComponent = complexRecipeComponent;
        }
    }
}
