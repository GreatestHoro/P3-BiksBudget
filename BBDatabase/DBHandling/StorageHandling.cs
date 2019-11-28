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
        /// <summary>
        /// The 'Update' method is meant as a way, where the user deletes the old list in the storage
        /// and the inserts the new one, to update the content of that person's storage.
        /// It does so by using the Delete and AddList method's described later in the code.
        /// </summary>
        /// <param name="username"></param> The username, is the string of the user that are logged in.
        /// <param name="products"></param> The products is the list of the new products that have the user is going to have
        /// in his or her storage.
        public void Update(string username, List<Product> products)
        {
            Delete(username);
            AddList(username, products);
        }

        /// <summary>
        /// The GetList method in StorageHandling, serves the purpose of retrieving a List of products linked to the user 
        /// that are logged in.
        /// </summary>
        /// <param name="username"></param> The username parameter, is the username of the person logged in.
        /// <returns></returns>
        public List<Product> GetList(string username)
        {
            // The productList and storageQuery, are temporary variables, that used to store
            // the productList and storageQuery respectivly to the search where there's a 
            // Custom_Field that ain't null or not.
            List<Product> productList = new List<Product>();
            string storageQuery;

            // The first step in the GetList method, is to see if there's a product with the ID
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
            if (new SQLConnect().CheckRecordExist(prodmsc))
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
                DataSet ds = new SQLConnect().DynamicSimpleListSQL(msc);

                // Afterwards we loop through the dataset and add the products to the productList.
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
                // The steps from the previous if statement is repeated, just without a inner join.
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
        public void AddList(string username, List<Product> storage)
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
                string checkExist = "SELECT COUNT(*) FROM userstorage WHERE prodid = @ProductID or custom_name = @CustomName AND username = @Username;";

                // Then the checkExist MySqlCommand is created, where the sql command is the string, and the placeholder's values is specified.
                MySqlCommand exist = new MySqlCommand(checkExist);
                exist.Parameters.AddWithValue("@ProductID", p._id);
                exist.Parameters.AddWithValue("@CustomName", p._customname);
                exist.Parameters.AddWithValue("@Username", username);

                //If the product doesn't exist, the product is added to the user's storage.
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

        /// <summary>
        /// The Delete method, uses a DELETE query where the WHERE clause is specified by the username.
        /// Then it deletes everything in the user's storage.
        /// </summary>
        /// <param name="username"></param> The username string parameter, is the user that are logged in.
        public void Delete(string username)
        {
            string removeQuery = "DELETE FROM `userstorage` WHERE `username` = @Username";

            MySqlCommand msc = new MySqlCommand(removeQuery);

            msc.Parameters.AddWithValue("@Username", username);

            new SQLConnect().NonQueryMSC(msc);
        }
    }
}