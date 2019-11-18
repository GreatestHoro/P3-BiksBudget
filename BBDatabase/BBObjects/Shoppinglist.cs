using System;
using System.Collections.Generic;
using System.Text;

namespace BBCollection.BBObjects
{
    public class Shoppinglist
    {
        public string _name;
        public List<Product> _products;
        public int amountOfProducts;
        public double _totalPrice;

        public Shoppinglist(string name, List<Product> products)
        {
            _name = name;
            _products = products;
            foreach (Product p in products)
            {
                _totalPrice = _totalPrice + p._price;
            }

        }

        public void RemoveProduct(Product product)
        {
            _totalPrice = _totalPrice - product._price;
            _products.Remove(product);
        }
    }
}
