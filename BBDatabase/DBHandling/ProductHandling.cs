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
        public void Add(Product product)
        {
            string addProductQuery = "INSERT INTO `products`(`id`,`productname`,`amount`,`price`,`image`, `store`)" +
                                     "VALUES(@Id,@ProductName,@Amount,@Price,@Image,@Store);";

            MySqlCommand msc = new MySqlCommand(addProductQuery);

            msc.Parameters.AddWithValue("@Id", product._id);
            msc.Parameters.AddWithValue("@ProductName", product._productName);
            msc.Parameters.AddWithValue("@Amount", product._amount);
            msc.Parameters.AddWithValue("@Price", product._price);
            msc.Parameters.AddWithValue("@Image", product._image);
            msc.Parameters.AddWithValue("@Store", product._storeName);

            new SQLConnect().NonQueryMSC(msc);
        }
        public void AddReference(string reference, string prodid)
        {
            string insertQuery =
                "UPDATE `products` SET `ingredient_reference` = @Reference WHERE id = @Prodid";

            MySqlCommand msc = new MySqlCommand(insertQuery);

            msc.Parameters.AddWithValue("@Reference", reference);
            msc.Parameters.AddWithValue("@Prodid", prodid);

            new SQLConnect().NonQueryMSC(msc);
        }
        public List<Product> GetList(string productName)
        {
            List<Product> productList = new List<Product>();
            string table = "products";
            string collumn = "productname";

            DataSet ds = new SQLConnect().DynamicSimpleListSQL(new SqlQuerySort().SortMSC(productName, table, collumn));

            if (ds.Tables.Count != 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    if (r[6] != DBNull.Value)
                    {
                        Product product = new Product((string)r[0], (string)r[1], (string)r[2], Convert.ToDouble(r[3]), (string)r[4], (string)r[5], (string)r[6]);
                        productList.Add(product);
                    }
                    else
                    {
                        Product product = new Product((string)r[0], (string)r[1], (string)r[2], Convert.ToDouble(r[3]), (string)r[4], (string)r[5]);
                        productList.Add(product);
                    }

                }
            }

            return productList;
        }
        public List<Product> GetRange(string productName, int limit, int offset)
        {
            List<Product> productList = new List<Product>();
            string table = "products";
            string collumn = "productname";

            DataSet ds = new SQLConnect().DynamicSimpleListSQL(new SqlQuerySort().SortMSCInterval(productName, table, collumn, limit, offset));

            if (ds.Tables.Count != 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Product product = new Product((string)r[0], (string)r[1], (string)r[2], Convert.ToDouble(r[3]), (string)r[4], (string)r[5]);
                    productList.Add(product);
                }
            }

            return productList;
        }
    }
}
