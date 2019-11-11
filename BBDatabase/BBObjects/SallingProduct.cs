using System;
using System.Collections.Generic;
using System.Text;

namespace BBCollection.BBObjects
{
    public class SallingProduct
    {
        public string _title;
        public string _id;
        public string _prod_id;
        public double _price;
        public string _description;
        public string _link;
        public string _img;

        public SallingProduct(string title, string id, string prod_id, double price, string description, string link, string img)
        {
            _title = title;
            _id = id;
            _prod_id = prod_id;
            _price = price;
            _description = description;
            _link = link;
            _img = img;
        }
    }
}
