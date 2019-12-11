using BBCollection.BBObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace BBCollection.DBHandling
{
    public class SortBy
    {
        public List<Product> SortByMostRelevant(List<Product> productList)
        {
            productList.Sort((a, b) => a._productName.Length.CompareTo(b._productName.Length));

            return productList;
        }

        public List<Product> SortByPriceLH(List<Product> productList)
        {
            productList.Sort((a, b) => a._price.CompareTo(b._price));

            return productList;
        }

        public List<Product> SortBypriceHL(List<Product> productList)
        {
            productList.Sort((a, b) => b._price.CompareTo(a._price));

            return productList;
        }

        public List<Product> SortByStoreAZ(List<Product> productList)
        {
            productList.Sort((a, b) => a._storeName.CompareTo(b._storeName));

            return productList;
        }

        public List<Product> SortByStoreZA(List<Product> productList)
        {
            productList.Sort((a, b) => b._storeName.CompareTo(a._storeName));

            return productList;
        }

        public List<Product> SortByNameAZ(List<Product> productList)
        {
            productList.Sort((a, b) => a._productName.CompareTo(b._productName));

            return productList;
        }

        public List<Product> SortByNameZA(List<Product> productList)
        {
            productList.Sort((a, b) => b._productName.CompareTo(a._productName));

            return productList;
        }
    }
}
