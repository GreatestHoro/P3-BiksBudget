using BBCollection.BBObjects;
using BBCollection.DBConncetion;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace BBCollection.DBHandling
{
    public class ShoppinglistHandling
    {
        public async void AddList(string username, List<Shoppinglist> shoppinglists)
        {
            string addQuery =
                "INSERT INTO `shoppinglists`(`username`, `shoppinglist_name`,`product_id`,`amount`) " +
                "VALUES(@Username,@ShoppinglistName,@ProductID,@Amount)";

            await Task.Run(() =>
            {
                foreach (Shoppinglist sl in shoppinglists)
                {
                    foreach (Product p in sl._products)
                    {
                        MySqlCommand msc = new MySqlCommand(addQuery);
                        Console.WriteLine("INSERT INTO `shoppinglists`(`username`, `shoppinglist_name`,`product_id`,`amount`)VALUES(\'" + username + "\',\'" + sl._name + "\',\'" + p._id + "\'," + p._amountleft + ")");
                        msc.Parameters.AddWithValue("@Username", username);
                        msc.Parameters.AddWithValue("@ShoppinglistName", sl._name);
                        msc.Parameters.AddWithValue("@ProductID", p._id);
                        msc.Parameters.AddWithValue("@Amount", p._amountleft);

                        new SQLConnect().NonQueryMSC(msc);
                    }
                }
            });
        }

        public async Task<List<Shoppinglist>> GetList(string username)
        {
            List<Shoppinglist> ShoppingLists = new List<Shoppinglist>();

            string getSLQuery =
                "SELECT shoppinglists.shoppinglist_name, products.*, shoppinglists.amount " +
                "FROM shoppinglists INNER JOIN products ON shoppinglists.product_id = products.id " +
                "WHERE username = @Username";

            MySqlCommand msc = new MySqlCommand(getSLQuery);
            msc.Parameters.AddWithValue("@Username", username);

            return await Task.Run(() =>
            {
                DataSet ds = new SQLConnect().DynamicSimpleListSQL(msc);

                if (ds.Tables.Count != 0)
                {
                    if (ds.Tables[0].Rows.Count != 0)

                    {
                        List<Product> products = new List<Product>();
                        string SLName = (string)ds.Tables[0].Rows[0][0];
                        foreach (DataRow r in ds.Tables[0].Rows)
                        {
                            string reference = "";
                            if (r[7] != DBNull.Value)
                            {
                                reference = (string)r[7];
                                Product product = new Product((string)r[1], (string)r[2], (string)r[3], Convert.ToDouble(r[4]), (string)r[5], (string)r[6], (int)r[8], (string)r[7]);
                                if (SLName == (string)r[0])
                                {
                                    products.Add(product);
                                }
                                else
                                {
                                    ShoppingLists.Add(new Shoppinglist(SLName, products));
                                    products = new List<Product>();
                                    products.Add(product);
                                }
                                SLName = (string)r[0];
                            }
                            else
                            {
                                Product product = new Product((string)r[1], (string)r[2], (string)r[3], Convert.ToDouble(r[4]), (string)r[5], (string)r[6], (int)r[8]);
                                if (SLName == (string)r[0])
                                {
                                    products.Add(product);
                                }
                                else
                                {
                                    ShoppingLists.Add(new Shoppinglist(SLName, products));
                                    products = new List<Product>();
                                    products.Add(product);
                                }
                                SLName = (string)r[0];

                            }


                        }
                        ShoppingLists.Add(new Shoppinglist(SLName, products));
                    }
                }
                return ShoppingLists;
            });
        }


        public async void Delete(string slName, string username)
        {
            string sLQuery =
                "DELETE FROM `shoppinglists` WHERE `shoppinglist_name` = @SLName AND username = @Username";

            MySqlCommand msc = new MySqlCommand(sLQuery);

            msc.Parameters.AddWithValue("@SLName", slName);

            msc.Parameters.AddWithValue("@Username", username);

            await Task.Run(() =>
            {
                new SQLConnect().NonQueryMSC(msc);
            });
        }
    }
}
