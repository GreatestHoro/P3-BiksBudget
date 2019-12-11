using BBCollection.BBObjects;
using BBCollection.DBConncetion;
using BBCollection.Queries;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

        public async Task<List<Product>> GetListAsync(string productName)
        {
            List<Product> productList = new List<Product>();
            string table = "products";
            string collumn = "productname";

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
        }

        public List<Product> GetList(string productName)
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

            DataSet ds = await new SQLConnect().DynamicSimpleListSQL(new SqlQuerySort().SortMSCInterval(productName, table, collumn, limit, offset));
            if (ds.Tables.Count != 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    string? id = r[0].ToString(); string? name = r[1].ToString(); string? amount = r[2].ToString();
                    string? image = r[4].ToString(); string? store = r[5].ToString();
                    double? price = Convert.ToDouble(r[3]);

                    Product product = new Product(id ??= "Null", name ??= "Null", amount ??= "", price ??= 0, image ??= "", store ??= "");
                    productList.Add(product);
                }
            }

            return productList;
        }



        public async Task<List<Product>> ReferencesAsync(string reference)
        {
            List<Product> products = new List<Product>();
            string query =
                "SELECT * FROM products WHERE ingredient_reference like @reference";

            MySqlCommand msc = new MySqlCommand(query);
            msc.Parameters.AddWithValue("@reference", "%" + reference + "%");

            DataSet ds = await new SQLConnect().DynamicSimpleListSQL(msc);
            if (ds.Tables.Count != 0)
            {
                try
                {
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        //(string id, string productName, string amount, double price, string image, string storeName, int amountleft, string customField)
                        Product product = new Product(r[0].ToString(), r[1].ToString(), r[2].ToString(), Convert.ToDouble(r[3]), r[4].ToString(), r[5].ToString(), 0, r[6].ToString());
                        products.Add(product);
                    }
                }
                catch (System.AggregateException e)
                {
                    Console.WriteLine(e);
                }
            }


            return products;
        }

        public async Task<List<Product>> MultipleReferencesAsync(List<string> references)
        {
            int count = 0;
            List<Product> products = new List<Product>();
            List<Tuple<string, string>> combinations = new List<Tuple<string, string>>();
            string query = "SELECT * FROM products WHERE ingredient_reference";
            foreach (string reference in references)
            {
                string parameter = "@parameter" + count;
                count++;

                if (query.Contains("like"))
                {
                    query = query + " and ingredient_reference like " + parameter;
                }
                else
                {
                    query = query + " like " + parameter;
                }



                combinations.Add(new Tuple<string, string>(parameter, reference));
            }

            MySqlCommand msc = new MySqlCommand(query);

            foreach (Tuple<string, string> combination in combinations)
            {
                msc.Parameters.AddWithValue(combination.Item1, "%" + combination.Item2 + "%");
            }

            Console.WriteLine(query);



            DataSet ds = await new SQLConnect().DynamicSimpleListSQL(msc);
            if (ds.Tables.Count != 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {

                    Product product = new Product((string)r[0], (string)r[1], (string)r[2], Convert.ToDouble(r[3]), (string)r[6]);
                    products.Add(product);
                }
            }
            return products;
        }

        public async Task AddImage(string image, string prodid)
        {
            string insertQuery =
                "UPDATE `products` SET `image` = @Image WHERE id = @Prodid AND (`image` IS NULL OR `image` = '')";

            MySqlCommand msc = new MySqlCommand(insertQuery);

            msc.Parameters.AddWithValue("@Image", image);
            msc.Parameters.AddWithValue("@Prodid", prodid);

            await Task.Run(() => new SQLConnect().NonQueryMSC(msc));
        }

        public async Task<double> CheapestPrice(string ingredient, Chain filter)
        {
            List<double> price = new List<double>();

            double bilkaPrice = await GetStorePrice("bilka", ingredient);
            double faktaPrice = await GetStorePrice("fakta", ingredient);
            double sbPrice = await GetStorePrice("superbrugsen", ingredient);

            if (filter == Chain.none)
            {
                price.Add(bilkaPrice);
                price.Add(faktaPrice);
                price.Add(sbPrice);
            }
            else
            {
                if ((filter & Chain.bilka) == Chain.bilka)
                {
                    price.Add(bilkaPrice);
                }
                if ((filter & Chain.fakta) == Chain.fakta)
                {
                    price.Add(faktaPrice);
                }
                if ((filter & Chain.superBrugsen) == Chain.superBrugsen)
                {
                    price.Add(sbPrice);
                }
            }
            if (price.Max() == 0) return 0;
            return price.Where(x => x > 0).Min(x => x);
        }

        private async Task<double> GetStorePrice(string store, string ingredient)
        {
            string ingredientQuery =
                "SELECT products.price FROM ingredient_store_link " +
                "INNER JOIN products on ingredient_store_link." + store + " = products.id " +
                "WHERE ingredientName = @Ingredient";

            MySqlCommand msc = new MySqlCommand(ingredientQuery);

            msc.Parameters.AddWithValue("@Ingredient", ingredient);

            DataSet ds = await new SQLConnect().DynamicSimpleListSQL(msc);

            if(ds.Tables.Count != 0) {
                DataTable dt = ds.Tables[0];
            if (dt.Rows.Count != 0)
            {
                return Convert.ToDouble(ds.Tables[0].Rows[0][0]);
            }
            }
            return 0;
        }
    }
}
