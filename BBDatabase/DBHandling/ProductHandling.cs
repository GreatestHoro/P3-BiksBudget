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
            string addProductQuery = "INSERT INTO `products`(`id`,`productname`,`amount`,`price`,`image`, `store`)" +
                                     "VALUES(@Id,@ProductName,@Amount,@Price,@Image,@Store);";

            MySqlCommand msc = new MySqlCommand(addProductQuery);

            msc.Parameters.AddWithValue("@Id", product._id);
            msc.Parameters.AddWithValue("@ProductName", product._productName);
            msc.Parameters.AddWithValue("@Amount", product._amount);
            //msc.Parameters.AddWithValue("@AmountLeft", product._amountleft);
            //msc.Parameters.AddWithValue("@TimeAdded", product._timeAdded);
            msc.Parameters.AddWithValue("@Price", product._price);
            msc.Parameters.AddWithValue("@Image", product._image);
            msc.Parameters.AddWithValue("@Store", product._storeName);

            /*"CREATE TABLE IF NOT EXISTS `products` (" +
                "`id` VARCHAR(255) UNIQUE, " +
                "`productname` VARCHAR(255), " +
                "`amount` VARCHAR(255), " +
                "`price` DECIMAL(6,2), " +
                "`image` varchar(255), " +
                "`store` varchar(255), " +
                "PRIMARY KEY(id)); ";*/


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
                Product product = new Product((string)r[0], (string)r[1], (string)r[2], Convert.ToDouble(r[3]), (string)r[4], (string)r[5]);
                productList.Add(product);
            }

            return productList;
        }

        

        public List<SallingProduct> ListOfSallingProductsFromName(string productName, DatabaseInformation dbInformation)
        {
            List<SallingProduct> productList = new List<SallingProduct>();

            string productQuery = "SELECT * FROM sallingproducts WHERE title LIKE @ProductName";

            MySqlCommand msc = new MySqlCommand(productQuery);
            msc.Parameters.AddWithValue("@ProductName", "%" + productName + "%");



            foreach (DataRow r in new SQLConnect().DynamicSimpleListSQL(msc, dbInformation).Tables[0].Rows)
            {
                SallingProduct sallingProduct = new SallingProduct((string)r[0], (string)r[1], (string)r[2], Convert.ToDouble(r[3]),
                                              (string)r[4], (string)r[5], (string)r[6]);
                productList.Add(sallingProduct);
            }

            return productList;
        }
    }
}
