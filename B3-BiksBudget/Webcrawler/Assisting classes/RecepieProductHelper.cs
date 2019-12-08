using BBCollection.BBObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace B3_BiksBudget.Webcrawler.Assisting_classes
{
    class RecepieProductHelper
    {
        Product? product = null;
        Recipe? recipe = null;

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
        public string GetUpdateImage()
        {
            return recipe.image ?? product._image;
        }


    }
}
