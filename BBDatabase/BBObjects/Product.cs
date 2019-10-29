using System;
using System.Collections.Generic;
using System.Text;

namespace BBCollection.BBObjects
{
    public class Product
    {
        public string _ean;
        public string _name;
        public string _name2;
        public double _price;
        public int _productHierarchyID;

        public Product(string ean, string name, string name2, double price, int productHierachyID)
        {
            _ean = ean;
            _name = name;
            _name2 = name2;
            _price = price;
            _productHierarchyID = productHierachyID;
        }
    }
}
