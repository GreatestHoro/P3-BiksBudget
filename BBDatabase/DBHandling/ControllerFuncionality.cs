﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore;
using Newtonsoft.Json;
using BBCollection;
using BBCollection.BBObjects;

namespace BBCollection.DBHandling
{
    public class ControllerFuncionality
    {
        public string HandleInputstring(string productString, out string email)
        {
            int pNum;

            int indexNumber = productString.IndexOf("|");
            int number = Convert.ToInt32(productString.Substring(0, indexNumber));
            if (number >= 10)
            {
                pNum = 2;
            }
            else
            {
                pNum = 1;
            }

            email = productString.Substring(indexNumber.ToString().Length + pNum, number);
            productString = productString.Remove(0, indexNumber.ToString().Length + number + pNum);

            return productString;
        }

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
                        dublicateList.Add(i);
                        isFound = true;
                        break;
                    }
                }
                if (isFound == false)
                {
                    uniqueList.Add(i);
                }
            }

            foreach (Product d in dublicateList)
            {
                foreach (Product i in uniqueList)
                {
                    if (d._id == i._id)
                    {
                        i._amountleft += d._amountleft;
                        break;
                    }
                }
            }

            return uniqueList;
        }
        public List<Product> removeDublicates(List<Product> shoppingList)
        {
            Dictionary<string, Product> dict = new Dictionary<string, Product>();

            foreach (Product p in shoppingList)
            {
                if (p._id != null)
                {
                    if (dict.ContainsKey(p._id))
                    {
                        dict[p._id]._amountleft++;
                    }
                    else
                    {
                        if (p._amountleft == 0)
                        {
                            break;
                        }
                        dict.Add(p._id, p);
                    }
                }
            }

            shoppingList.Clear();
            foreach (KeyValuePair<string, Product> item in dict)
            {
                shoppingList.Add(dict[item.Key]);
            }

            return shoppingList;
        }
    }
}
