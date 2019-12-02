using BBCollection.BBObjects;
using BBCollection.DBConncetion;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BBCollection.DBHandling
{
    public class ProductHandling
    {
        /// <summary>
        /// This method adds the product variables to a INSERT sql query, that afterwards get send to the 
        /// SQLConnect() class, where it is added to the Database.
        /// </summary>
        /// <param name="product"></param> The product object contains the variables that have to be added to the database.
        public async Task Add(Product product)
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

            await Task.Run(() => new SQLConnect().NonQueryMSC(msc));
        }

        /// <summary>
        /// This method adds a ingredient reference and product id, to a UPDATE query, so that the reference can be connected to 
        /// the product with that specific product id.
        /// </summary>
        /// <param name="reference"></param> Reference is a string, with the ingredient references.
        /// <param name="prodid"></param> Prodid is a string that contains the ID of the object.
        public async Task AddReference(string reference, string prodid)
        {
            string insertQuery =
                "UPDATE `products` SET `ingredient_reference` = @Reference WHERE id = @Prodid";

            MySqlCommand msc = new MySqlCommand(insertQuery);

            msc.Parameters.AddWithValue("@Reference", reference);
            msc.Parameters.AddWithValue("@Prodid", prodid);

            await Task.Run(() => new SQLConnect().NonQueryMSC(msc));
        }

        public async Task<List<Product>> GetList(string productName)
        {
            List<Product> productList = new List<Product>();
            string table = "products";
            string collumn = "productname";

            return await Task.Run(async () => {

                DataSet ds = await new SQLConnect().DynamicSimpleListSQL(new SqlQuerySort().SortMSC(productName, table, collumn));

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
            });
        }

        public List<Product> GetListSyncAsync(string productName)
        {
            List<Product> productList = new List<Product>();
            string table = "products";
            string collumn = "productname";

                DataSet ds = new SQLConnect().DynamicSimpleListSQLSync(new SqlQuerySort().SortMSC(productName, table, collumn));

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

        public async Task<List<Product>> GetRange(string productName, int limit, int offset)
        {
            List<Product> productList = new List<Product>();
            string table = "products";
            string collumn = "productname";

            return await Task.Run(async () =>
            {
                DataSet ds = await new SQLConnect().DynamicSimpleListSQL(new SqlQuerySort().SortMSCInterval(productName, table, collumn, limit, offset));
                if (ds.Tables.Count != 0)
                {
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        Product product = new Product((string)r[0], (string)r[1], (string)r[2], Convert.ToDouble(r[3]), (string)r[4], (string)r[5]);
                        productList.Add(product);
                    }
                }

                return productList;
            });
        }
    }
}
