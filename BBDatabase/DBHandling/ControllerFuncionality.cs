using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Newtonsoft.Json;
using BBCollection;
using BBCollection.BBObjects;

namespace BBCollection.DBHandling
{
    public class ControllerFuncionality
    {
        string replacementUlr = "https://cdn.discordapp.com/attachments/519096261408718908/649685312083197973/coopdicoop.jpg";
        string shopString;

        /// <summary>
        /// The inputlist is split up in two lists.
        /// One is the unique list.
        /// And one is the dublicats list.
        /// At the end the unique products are counted up
        /// based on the dublicat list.
        /// </summary>
        /// <param name="inputList"></param>
        /// <returns></returns>

        public List<Product> HandleDublicats(List<Product> inputList)
        {
            bool isFound = false;
            List<Product> uniqueList = new List<Product>();
            List<Product> dublicateList = new List<Product>();

            foreach (Product i in inputList)
            {
                isFound = false;
                foreach (Product u in uniqueList)
                {
                    if (i._id == u._id)
                    {
                        // If the product already exists, it is added 
                        // to the dublicat list.
                        dublicateList.Add(i);
                        isFound = true;
                        break;
                    }
                }
                if (isFound == false)
                {
                    // If the product is not found, it is added
                    // to the unique list
                    uniqueList.Add(i);
                }
            }

            foreach (Product d in dublicateList)
            {
                foreach (Product i in uniqueList)
                {
                    if (d._id == i._id)
                    {
                        // When the match is found, the amountLeft are added up.
                        i._amountleft += d._amountleft;
                        break;
                    }
                }
            }

            return uniqueList;
        }

        /// <summary>
        /// Finds the index of a product in a list of products
        /// </summary>
        /// <param name="inputList"></param>
        /// <param name="item"></param>
        /// <returns></returns>

        public int FindIdex(List<Product> inputList, Product item)
        {
            for (int i = 0; i < inputList.Count; i++)
            {
                if (inputList[i]._id.Equals(item._id))
                {
                    return i;
                }
            }

            return -1;
        }

        public string SetImageUrl(string url)
        {
            if (String.IsNullOrEmpty(url) || String.IsNullOrWhiteSpace(url) || url.Equals(replacementUlr))
            {
                return replacementUlr;
            }
            else
            {
                return ProductImage(url);
            }
        }

        private string ProductImage(string prod_image)
        {
            if (prod_image.Contains("&imgSize=1080x1080"))
            {
                return prod_image;
            }
            else
            {
                return prod_image + "&imgSize=1080x1080";
            }
        }

        public List<Product> SetAllPicture(List<Product> inputList)
        {
            inputList.ForEach(x => x._image = SetImageUrl(x._image));

            return inputList;
        }

        public string SetOnePicture(string image)
        {
            image = SetImageUrl(image);

            return image;
        }
    }
}
