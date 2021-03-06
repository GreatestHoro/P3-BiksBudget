﻿using BBCollection.BBObjects;
using BBCollection.DBConncetion;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BBCollection.DBHandling
{
    public class StorageHandling
    {
        /// <summary>
        /// The 'Update' method is meant as a way, where the user deletes the old list in the storage
        /// and the inserts the new one, to update the content of that person's storage.
        /// It does so by using the Delete and AddList method's described later in the code.
        /// </summary>
        /// <param name="username"></param> The username, is the string of the user that are logged in.
        /// <param name="products"></param> The products is the list of the new products that have the user is going to have
        /// in his or her storage.
        public async Task Update(string username, List<Product> products)
        {
            await Task.Run(async () =>
            {
                await Delete(username);
                await AddList(username, products);
            });
        }

        /// <summary>
        /// The GetListAsync method in StorageHandling, serves the purpose of retrieving a List of products linked to the user 
        /// that are logged in.
        /// </summary>
        /// <param name="username"></param> The username parameter, is the username of the person logged in.
        /// <returns></returns>
        public async Task<List<Product>> GetList(string username)
        {
            // The productList and storageQuery, are temporary variables, that used to store
            // the productList and storageQuery respectivly to the search where there's a 
            // Custom_Field that ain't null or not.
            List<Product> productList = new List<Product>();
            string storageQuery;

            // The first step in the GetListAsync method, is to see if there's a product with the ID
            // inside the storage. We use two strings, one where the custom_name collumn is included,
            // and one where it is not. We then add the query as the command in MySqlCommand, 
            // and the username as the first and only parameter on both queries.
            string checkProdIDExist =
                "SELECT Count(*) FROM userstorage WHERE prodid IS NOT NULL AND username = @Username";
            MySqlCommand prodmsc = new MySqlCommand(checkProdIDExist);
            prodmsc.Parameters.AddWithValue("@Username", username);

            string checkCustNameExist =
                "SELECT Count(*) FROM userstorage where custom_name IS NOT NULL AND username = @Username";
            MySqlCommand custmsc = new MySqlCommand(checkCustNameExist);
            custmsc.Parameters.AddWithValue("@Username", username);


            // After this we have two if statements equal to eachother, one where the custom name null 
            // and one where it is not. 

            if (await new SQLConnect().CheckRecordExist(prodmsc))
            {
                // We first make the storageQuery, where we select collumns from both the products and userstorage tables,
                // we do that by joining the tables together with a INNER JOIN, with foreign key 'userstorage.prodid' from userstorage linked with the product 
                // id from the primary key in "Products", the WHERE clause specifies that it is the user that we want to get the storage from.
                storageQuery =
                    "SELECT userstorage.prodid, products.productname , products.amount, products.price, products.image, products.store, userstorage.amountStored, userstorage.timeadded, userstorage.state, products.ingredient_reference " +
                    "FROM userstorage INNER JOIN products ON userstorage.prodid = products.id " +
                    "WHERE userstorage.username = @Username AND prodid IS NOT NULL";

                // Then we add the query and parameter to the MySqlCommand and get the data from the database
                // and inserts it in a dataset. We do that with the DynamicSimpleListSQL method from the SQLConnect class.
                MySqlCommand msc = new MySqlCommand(storageQuery);
                msc.Parameters.AddWithValue("@Username", username);
                DataSet ds = await new SQLConnect().DynamicSimpleListSQL(msc);

                // Afterwards we loop through the dataset and add the products to the productList.
                if (ds.Tables.Count != 0)
                {
                    try
                    {
                        foreach (DataRow r in ds.Tables[0].Rows)
                        {

                            Product product = new Product(r[0].ToString(), r[1].ToString(), r[2].ToString(), Convert.ToDouble(r[3]),
                                         r[4].ToString(), r[5].ToString(), Convert.ToInt32(r[6]), Convert.ToString(r[7]), r[8].ToString(), r[9].ToString());
                            productList.Add(product);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                    }

                }
            }
            if (await new SQLConnect().CheckRecordExist(custmsc))
            {
                // The steps from the previous if statement is repeated, just without a inner join.
                storageQuery =
                    "SELECT userstorage.custom_name, userstorage.amountStored, userstorage.timeadded, userstorage.state " +
                    "FROM userstorage " +
                    "WHERE username = @Username AND custom_name IS NOT NULL;";
                MySqlCommand msc = new MySqlCommand(storageQuery);
                msc.Parameters.AddWithValue("@Username", username);
                DataSet ds = await new SQLConnect().DynamicSimpleListSQL(msc);


                if (ds.Tables.Count != 0)
                {
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        Product product = new Product("", (string)r[0], (int)r[1], Convert.ToString(r[2]), (string)r[3]);
                        productList.Add(product);
                    }
                }
            }

            // The method finishes by returning the productList.
            return productList;
        }

        /// <summary>
        /// In the AddList method, the user's storage gets saved. It does so
        /// by going through the product list in the user's storage and save them
        /// one by one.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="storage"></param>
        public async Task AddList(string username, List<Product> storage)
        {
            await Task.Run(async () =>
            {

                foreach (Product p in storage)
                {
                    // The AddList method, takes use of two queries, the productQuery and the checkExist.
                    // The productQuery is a INSERT query saved in a string, that first specifies that the table that product and user information 
                    // have to be saved in, is the userstorage, and then it specifies in what collumns each value have to be 
                    // placed in. Afterwards the values are specified with placeholders for the parameters marked with a @ prefix.
                    // The second query, which is also saved in a string, is the checkExist, it checks if the products already exists in the user's storage.
                    string productQuery = "INSERT INTO `userstorage`(`username`,`prodid`,`custom_name`,`amountstored`,`state`) " +
                                          "VALUES(@Username,@ProductID,@CustomName,@AmountStored,@State);";

                    MySqlCommand msc = new MySqlCommand(productQuery);
                    msc.Parameters.AddWithValue("@Username", username);
                    msc.Parameters.AddWithValue("@ProductID", p._id);
                    msc.Parameters.AddWithValue("@CustomName", p._customname);
                    msc.Parameters.AddWithValue("@AmountStored", p._amountleft);
                    msc.Parameters.AddWithValue("@State", p._state);
                    await new SQLConnect().NonQueryMSCAsync(msc);
                }
            });
        }

        /// <summary>
        /// The Delete method, uses a DELETE query where the WHERE clause is specified by the username.
        /// Then it deletes everything in the user's storage.
        /// </summary>
        /// <param name="username"></param> The username string parameter, is the user that are logged in.
        public async Task Delete(string username)
        {
            string removeQuery = "DELETE FROM `userstorage` WHERE `username` = @Username";

            MySqlCommand msc = new MySqlCommand(removeQuery);

            msc.Parameters.AddWithValue("@Username", username);

            await Task.Run(async () =>
            {
                await new SQLConnect().NonQueryMSCAsync(msc);
            });
        }
    }
}