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

        public List<Product> ProductsInStorageFromUsername(string username, DatabaseInformation dbInformation)
        {
            List<Product> productList = new List<Product>();

            string storageQuery =
                "SELECT userstorage.prodid, products.productname , products.amount, products.price, products.image, products.store, userstorage.amountStored, userstorage.timeadded, userstorage.state FROM userstorage INNER JOIN products ON userstorage.prodid = products.id WHERE userstorage.username = @Username";

            MySqlCommand msc = new MySqlCommand(storageQuery);
            msc.Parameters.AddWithValue("@Username", username);
            DataSet ds = new SQLConnect().DynamicSimpleListSQL(msc, dbInformation);

            

            if (ds.Tables.Count != 0) 
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Product product = new Product((string)r[0], (string)r[1], (string)r[2], Convert.ToDouble(r[3]), (string)r[4], (string)r[5], (int)r[6], Convert.ToString(r[7]), (string)r[8]);
                    productList.Add(product);
                }
            }

            return productList;
        }

        public bool AddToStorageFromUsername(string username, List<Product> storage, DatabaseInformation dbInforamtion)
        {
            foreach(Product p in storage)
            {
                string productQuery = "INSERT INTO `userstorage`(`username`,`prodid`,`amountstored`,`state`) VALUES(@Username,@ProductID,@AmountStored,@State);";
                string checkExist = "SELECT COUNT(*) FROM userstorage WHERE prodid = @ProductID";

                MySqlCommand exist = new MySqlCommand(checkExist);
                exist.Parameters.AddWithValue("@ProductID", p._id);

                Console.WriteLine(new SQLConnect().CheckRecordExist(exist, dbInforamtion));

                if(!new SQLConnect().CheckRecordExist(exist, dbInforamtion))
                {
                    MySqlCommand msc = new MySqlCommand(productQuery);

                    msc.Parameters.AddWithValue("@Username", username);
                    msc.Parameters.AddWithValue("@ProductID", p._id);
                    msc.Parameters.AddWithValue("@AmountStored", p._amountleft);
                    msc.Parameters.AddWithValue("@State", p._state);
                    Console.WriteLine("ADD ITEM HERE!");
                    new SQLConnect().NonQueryMSC(msc, dbInforamtion);
                }
            }

            return false;
        }

        public void AddSingleProductToStorage(string username, Product product, DatabaseInformation databaseInformation)
        {
            string productQuery = "INSERT INTO `userstorage`(`username`,`prodid`,`amountstored`,`state`) VALUES(@Username,@ProductID,@AmountStored,@State);";
            string checkExist = "SELECT COUNT(*) FROM userstorage WHERE prodid = @ProductID";

            MySqlCommand exist = new MySqlCommand(checkExist);
            exist.Parameters.AddWithValue("@ProductID", product._id);


            Console.WriteLine(new SQLConnect().CheckRecordExist(exist, databaseInformation));

            if (!new SQLConnect().CheckRecordExist(exist, databaseInformation))
            {
                MySqlCommand msc = new MySqlCommand(productQuery);

                msc.Parameters.AddWithValue("@Username", username);
                msc.Parameters.AddWithValue("@ProductID", product._id);
                msc.Parameters.AddWithValue("@AmountStored", product._amountleft);
                msc.Parameters.AddWithValue("@State", product._state);

                new SQLConnect().NonQueryMSC(msc, databaseInformation);
            }
        }

        

        public void RemoveStorageFromUsername(string username, DatabaseInformation databaseInformation)
        {
            string removeQuery = "REMOVE FROM `userstorage` WHERE `username` = @Username";

            MySqlCommand msc = new MySqlCommand(removeQuery);

            msc.Parameters.AddWithValue("@Username", username);

            new SQLConnect().NonQueryMSC(msc, databaseInformation);
        }
    }
}
