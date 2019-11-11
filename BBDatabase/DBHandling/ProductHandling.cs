using BBCollection.BBObjects;
using BBCollection.DBConncetion;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace BBCollection.DBHandling
{
    public class ProductHandling
    {
        public void Insert(Product product, DatabaseInformation dbInformation)
        {
            string addProductQuery = "INSERT INTO `products`(`ean`,`productName`,`productName2`,`price`,`productHierarchyID`)" +
                                     "VALUES(@Ean,@ProductName,@ProductName2,@Price,@ProductHierarchyID);";

            MySqlCommand msc = new MySqlCommand(addProductQuery);

            msc.Parameters.AddWithValue("@Ean", product._ean);
            msc.Parameters.AddWithValue("@ProductName", product._name);
            msc.Parameters.AddWithValue("@ProductName2", product._name2);
            msc.Parameters.AddWithValue("@Price", product._price);
            msc.Parameters.AddWithValue("@ProductHierarchyID", product._productHierarchyID);

            new SQLConnect().NonQueryMSC(msc, dbInformation);
        }

        public List<Product> ListOfProductsFromName(string productName, DatabaseInformation dbInformation)
        {
            List<Product> productList = new List<Product>();

            string recipesQuery = "SELECT * FROM products WHERE productName LIKE @ProductName";

            MySqlCommand msc = new MySqlCommand(recipesQuery);
            msc.Parameters.AddWithValue("@ProductName", "%" + productName + "%");



            foreach(DataRow r in new SQLConnect().DynamicSimpleListSQL(msc, dbInformation).Tables[0].Rows)
            {
                Console.WriteLine((string) r[1]);
                Product product = new Product((string)r[1], (string)r[2], (string)r[3], Convert.ToDouble(r[4]), (int)r[5]);
                productList.Add(product);
            }

            return productList;
        }
    }
}
