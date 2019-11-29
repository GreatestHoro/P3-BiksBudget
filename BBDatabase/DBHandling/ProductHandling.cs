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
        /// <summary>
        /// This method adds the product variables to a INSERT sql query, that afterwards get send to the 
        /// SQLConnect() class, where it is added to the Database.
        /// </summary>
        /// <param name="product"></param> The product object contains the variables that have to be added to the database.
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

        /// <summary>
        /// This method adds a ingredient reference and product id, to a UPDATE query, so that the reference can be connected to 
        /// the product with that specific product id.
        /// </summary>
        /// <param name="reference"></param> Reference is a string, with the ingredient references.
        /// <param name="prodid"></param> Prodid is a string that contains the ID of the object.
        public void AddReference(string reference, string prodid)
        {
            string insertQuery =
                "UPDATE `products` SET `ingredient_reference` = @Reference WHERE id = @Prodid";

            MySqlCommand msc = new MySqlCommand(insertQuery);

            msc.Parameters.AddWithValue("@Reference", reference);
            msc.Parameters.AddWithValue("@Prodid", prodid);

            new SQLConnect().NonQueryMSC(msc);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Product Get(string id)
        {
            Product product = new Product();
            ProductHandling handle = new ProductHandling();
            string getProductQuery =
                "SELECT * FROM products WHERE id = @ProdId";

            MySqlCommand msc = new MySqlCommand(getProductQuery);

            msc.Parameters.AddWithValue("@ProdId", id);

            DataSet ds = new SQLConnect().DynamicSimpleListSQL(msc);

            if (ds.Tables.Count != 0)
            {
                if (ds.Tables[0].Rows.Count != 0)
                {
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        string reference = "";

                        if (r[6] == DBNull.Value)
                        {
                            reference = "";
                            product = new Product((string)r[0], (string)r[1], (string)r[2], Convert.ToDouble(r[3]), (string)r[4], (string)r[5], reference);
                        }
                        else
                        {
                            product = new Product((string)r[0], (string)r[1], (string)r[2], Convert.ToDouble(r[3]), (string)r[4], (string)r[5], (string)r[6]);
                        }
                    }
                }
            }
            return product;
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
        public List<Product> GetReferences(string reference)
        {
            List<Product> productList = new List<Product>();

            string getListQuery =
                "SELECT * FROM products WHERE reference LIKE = @Reference";

            MySqlCommand msc = new MySqlCommand(getListQuery);

            msc.Parameters.AddWithValue("@Reference", "%" + reference + "%");

            DataSet ds = new SQLConnect().DynamicSimpleListSQL(msc);

            if (ds.Tables.Count != 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Product product = new Product((string)r[0], (string)r[1], (string)r[2], Convert.ToDouble(r[3]), (string)r[4], (string)r[5], (string)r[6]);
                    productList.Add(product);
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
