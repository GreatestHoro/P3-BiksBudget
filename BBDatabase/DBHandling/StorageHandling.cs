using BBCollection.BBObjects;
using BBCollection.DBConncetion;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BBCollection.DBHandling
{
    public class StorageHandling
    {
        public void Update(string username, List<Product> products)
        {
            Delete(username);
            AddList(username, products);
        }

        public List<Product> GetList(string username)
        {
            List<Product> productList = new List<Product>();

            string storageQuery;

            string checkProdIDExist =
                "SELECT Count(*) FROM userstorage WHERE prodid IS NOT NULL AND username = @Username";
            MySqlCommand prodmsc = new MySqlCommand(checkProdIDExist);
            prodmsc.Parameters.AddWithValue("@Username", username);

            string checkCustNameExist =
                "SELECT Count(*) FROM userstorage where custom_name IS NOT NULL AND username = @Username";
            MySqlCommand custmsc = new MySqlCommand(checkCustNameExist);
            custmsc.Parameters.AddWithValue("@Username", username);

            if (new SQLConnect().CheckRecordExist(prodmsc))
            {
                storageQuery =
                    "SELECT userstorage.prodid, products.productname , products.amount, products.price, products.image, products.store, userstorage.amountStored, userstorage.timeadded, userstorage.state, products.ingredient_reference " +
                    "FROM userstorage INNER JOIN products ON userstorage.prodid = products.id " +
                    "WHERE userstorage.username = @Username AND prodid IS NOT NULL";
                MySqlCommand msc = new MySqlCommand(storageQuery);
                msc.Parameters.AddWithValue("@Username", username);
                DataSet ds = new SQLConnect().DynamicSimpleListSQL(msc);

                if (ds.Tables.Count != 0)
                {
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        Product product = new Product((string)r[0], (string)r[1], (string)r[2], Convert.ToDouble(r[3]), (string)r[4], (string)r[5], (int)r[6], Convert.ToString(r[7]), (string)r[8], (string)r[9]);
                        productList.Add(product);
                    }
                }
            }
            if (new SQLConnect().CheckRecordExist(custmsc))
            {
                storageQuery =
                    "SELECT userstorage.custom_name, userstorage.amountStored, userstorage.timeadded, userstorage.state " +
                    "FROM userstorage " +
                    "WHERE username = @Username AND custom_name IS NOT NULL;";
                MySqlCommand msc = new MySqlCommand(storageQuery);
                msc.Parameters.AddWithValue("@Username", username);
                DataSet ds = new SQLConnect().DynamicSimpleListSQL(msc);


                if (ds.Tables.Count != 0)
                {
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        Product product = new Product("", (string)r[0], (int)r[1], Convert.ToString(r[2]), (string)r[3]);
                        productList.Add(product);
                    }
                }
            }

            return productList;
        }

        public void AddList(string username, List<Product> storage)
        {
            Console.WriteLine(storage.Count);

            foreach (Product p in storage)
            {
                string productQuery = "INSERT INTO `userstorage`(`username`,`prodid`,`custom_name`,`amountstored`,`state`) " +
                                      "VALUES(@Username,@ProductID,@CustomName,@AmountStored,@State);";
                string checkExist = "SELECT COUNT(*) FROM userstorage WHERE prodid = @ProductID or custom_name = @CustomName;";

                MySqlCommand exist = new MySqlCommand(checkExist);
                exist.Parameters.AddWithValue("@ProductID", p._id);
                exist.Parameters.AddWithValue("@CustomName", p._customname);

                Console.WriteLine(p._id);
                Console.WriteLine(new SQLConnect().CheckRecordExist(exist));
                if (!new SQLConnect().CheckRecordExist(exist))
                {

                    MySqlCommand msc = new MySqlCommand(productQuery);

                    msc.Parameters.AddWithValue("@Username", username);
                    msc.Parameters.AddWithValue("@ProductID", p._id);
                    msc.Parameters.AddWithValue("@CustomName", p._customname);
                    msc.Parameters.AddWithValue("@AmountStored", p._amountleft);
                    msc.Parameters.AddWithValue("@State", p._state);
                    new SQLConnect().NonQueryMSC(msc);
                }
            }
        }

        public void Add(string username, Product product)
        {
            string productQuery = "INSERT INTO `userstorage`(`username`,`prodid`,`amountstored`,`state`) " +
                                  "VALUES(@Username,@ProductID,@AmountStored,@State);";

            string checkExist = "SELECT COUNT(*) FROM userstorage WHERE prodid = @ProductID";

            MySqlCommand exist = new MySqlCommand(checkExist);
            exist.Parameters.AddWithValue("@ProductID", product._id);

            if (!new SQLConnect().CheckRecordExist(exist))
            {
                MySqlCommand msc = new MySqlCommand(productQuery);

                msc.Parameters.AddWithValue("@Username", username);
                msc.Parameters.AddWithValue("@ProductID", product._id);
                msc.Parameters.AddWithValue("@AmountStored", product._amountleft);
                msc.Parameters.AddWithValue("@State", product._state);

                new SQLConnect().NonQueryMSC(msc);
            }
        }

        public void Delete(string username)
        {
            string removeQuery = "DELETE FROM `userstorage` WHERE `username` = @Username";

            MySqlCommand msc = new MySqlCommand(removeQuery);

            msc.Parameters.AddWithValue("@Username", username);

            new SQLConnect().NonQueryMSC(msc);
        }
    }
}