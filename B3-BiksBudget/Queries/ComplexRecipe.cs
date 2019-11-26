using System;
using System.Collections.Generic;
using System.Text;
using BBGatherer.Queries;

namespace B3_BiksBudget.BBGatherer.Queries
{
    //Complex Recipe inherits the standard recipe, but extends it with an member/field of ComplexRecipeComponent, 
    //which can be seen in the file ComplexRecipeComponent.cs
    public class ComplexRecipe : BBCollection.BBObjects.Recipe
    {
        public ComplexRecipeComponent _complexRecipeComponent = new ComplexRecipeComponent();

        //Calls the the parent class' constructor, and initializes the _complexRecipeComponent
        public ComplexRecipe(int recipeID, string name, string description, List<BBCollection.BBObjects.Ingredient> ingredientList, float PerPerson, ComplexRecipeComponent complexRecipeComponent) : 
                        base(recipeID, name, description, ingredientList, PerPerson)
        {
            _complexRecipeComponent = complexRecipeComponent;
        }
    }
}
