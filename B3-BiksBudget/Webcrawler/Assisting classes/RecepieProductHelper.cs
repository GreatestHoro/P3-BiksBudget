using BBCollection.BBObjects;
using System;
using System.Collections.Generic;
using System.Text;
using BBCollection.DBHandling;

namespace B3_BiksBudget.Webcrawler.Assisting_classes
{
    class RecepieProductHelper
    {

        Product? product = null;
        Recipe? recipe = null;
        ProductHandling ProductHandling = new ProductHandling();
        RecipeHandling RecipeHandling = new RecipeHandling();

        public RecepieProductHelper(Product product) 
        {
            this.product = product;
        }

        public RecepieProductHelper(Recipe recipe)
        {
            this.recipe = recipe;
        }

        public string GetName() 
        {
            return recipe._Name ?? product._productName;
        }

        public string GetImage()
        {
            return recipe.image ?? product._image;
        }
        public void UpdateImage(string url)
        {
            if (CheckProduct())
            {
                updateProduct(url);
            }
            else if (CheckRecipe())
            {
                updateRecipes(url);
            }
            else 
            {
                throw new Exception("trouble in the RecepieProductHelper");
            }
        }
        private bool CheckProduct() 
        {
            return product != null ? true : false;
        }

        private bool CheckRecipe()
        {
            return recipe != null ? true : false;
        }
        private void updateProduct(string url)
        {
            _ = ProductHandling.AddImage(url,product._id);
        }

        private void updateRecipes(string url)
        {
            _ = RecipeHandling.AddImage(url, recipe._recipeID.ToString());
        }

    }
}
