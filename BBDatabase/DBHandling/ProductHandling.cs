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
        public void UpdateStorage(string username, List<Product> products, DatabaseInformation databaseInformation)
        {
            RemoveStorageFromUsername(username, databaseInformation);

            AddToStorageFromUsername(username, products, databaseInformation);
        }

        public void Insert(Product product, DatabaseInformation dbInformation)
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

            new SQLConnect().NonQueryMSC(msc, dbInformation);
        }

        public List<Product> ListOfProductsFromName(string productName, DatabaseInformation dbInformation)
        {
            List<Product> productList = new List<Product>();
            string table = "products";
            string collumn = "productname";

            DataSet ds = new SQLConnect().DynamicSimpleListSQL(new SqlQuerySort().SortMSC(productName,table,collumn), dbInformation);

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

        public List<Product> ProductsInStorageFromUsername(string username, DatabaseInformation dbInformation)
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

            if (new SQLConnect().CheckRecordExist(prodmsc, dbInformation))
            {
                storageQuery =
                    "SELECT userstorage.prodid, products.productname , products.amount, products.price, products.image, products.store, userstorage.amountStored, userstorage.timeadded, userstorage.state, products.ingredient_reference " +
                    "FROM userstorage INNER JOIN products ON userstorage.prodid = products.id " +
                    "WHERE userstorage.username = @Username AND prodid IS NOT NULL";
                MySqlCommand msc = new MySqlCommand(storageQuery);
                msc.Parameters.AddWithValue("@Username", username);
                DataSet ds = new SQLConnect().DynamicSimpleListSQL(msc, dbInformation);

                if (ds.Tables.Count != 0)
                {
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        Product product = new Product((string)r[0], (string)r[1], (string)r[2], Convert.ToDouble(r[3]), (string)r[4], (string)r[5], (int)r[6], Convert.ToString(r[7]), (string)r[8], (string)r[9]);
                        productList.Add(product);
                    }
                }
            } 
            if (new SQLConnect().CheckRecordExist(custmsc, dbInformation))
            {
                storageQuery =
                    "SELECT userstorage.custom_name, userstorage.amountStored, userstorage.timeadded, userstorage.state " +
                    "FROM userstorage " +
                    "WHERE username = @Username AND custom_name IS NOT NULL;";
                MySqlCommand msc = new MySqlCommand(storageQuery);
                msc.Parameters.AddWithValue("@Username", username);
                DataSet ds = new SQLConnect().DynamicSimpleListSQL(msc, dbInformation);
                

                if (ds.Tables.Count != 0)
                {
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        Product product = new Product("", (string)r[0], (int) r[1], Convert.ToString(r[2]), (string)r[3]);
                        productList.Add(product);
                    }
                }
            }

            return productList;
        }

        public void AddToStorageFromUsername(string username, List<Product> storage, DatabaseInformation dbInforamtion)
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
                Console.WriteLine(new SQLConnect().CheckRecordExist(exist, dbInforamtion));
                if (!new SQLConnect().CheckRecordExist(exist, dbInforamtion))
                {
                    
                    MySqlCommand msc = new MySqlCommand(productQuery);

                    msc.Parameters.AddWithValue("@Username", username);
                    msc.Parameters.AddWithValue("@ProductID", p._id);
                    msc.Parameters.AddWithValue("@CustomName", p._customname);
                    msc.Parameters.AddWithValue("@AmountStored", p._amountleft);
                    msc.Parameters.AddWithValue("@State", p._state);
                    new SQLConnect().NonQueryMSC(msc, dbInforamtion);
                }
            }
        }

        public void AddSingleProductToStorage(string username, Product product, DatabaseInformation databaseInformation)
        {
            string productQuery = "INSERT INTO `userstorage`(`username`,`prodid`,`amountstored`,`state`) " +
                                  "VALUES(@Username,@ProductID,@AmountStored,@State);";

            string checkExist = "SELECT COUNT(*) FROM userstorage WHERE prodid = @ProductID";

            MySqlCommand exist = new MySqlCommand(checkExist);
            exist.Parameters.AddWithValue("@ProductID", product._id);

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
            string removeQuery = "DELETE FROM `userstorage` WHERE `username` = @Username";

            MySqlCommand msc = new MySqlCommand(removeQuery);

            msc.Parameters.AddWithValue("@Username", username);

            new SQLConnect().NonQueryMSC(msc, databaseInformation);
        }

        public void AddShoppingListsToDB(string username, List<Shoppinglist> shoppinglists, DatabaseInformation databaseInformation)
        {
            string addQuery =
                "INSERT INTO `shoppinglists`(`username`, `shoppinglist_name`,`product_id`,`amount`) " +
                "VALUES(@Username,@ShoppinglistName,@ProductID,@Amount)";

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

                    new SQLConnect().NonQueryMSC(msc, databaseInformation);
                }
            }


        }

        public List<Shoppinglist> GetShoppinglistsFromUsername(string username, DatabaseInformation databaseInformation)
        {
            List<Shoppinglist> ShoppingLists = new List<Shoppinglist>();

            string getSLQuery =
                "SELECT shoppinglists.shoppinglist_name, products.*, shoppinglists.amount " +
                "FROM shoppinglists INNER JOIN products ON shoppinglists.product_id = products.id " +
                "WHERE username = @Username";

            MySqlCommand msc = new MySqlCommand(getSLQuery);
            msc.Parameters.AddWithValue("@Username", username);
            DataSet ds = new SQLConnect().DynamicSimpleListSQL(msc, databaseInformation);

            if (ds.Tables.Count != 0)
            {
                if (ds.Tables[0].Rows.Count != 0)

                {
                    List<Product> products = new List<Product>();
                    string SLName = (string)ds.Tables[0].Rows[0][0];
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        string reference = "";
                        if(r[7] != DBNull.Value)
                        {
                            reference = (string) r[7];
                        }
                        Product product = new Product((string)r[1], (string)r[2], (string)r[3], Convert.ToDouble(r[4]), (string)r[5], (string)r[6], (int)r[8],(string)r[7]);

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
                    ShoppingLists.Add(new Shoppinglist(SLName, products));
                }
            }
            return ShoppingLists;
        }

        public void DeleteShoppingListFromName(string slName, string username, DatabaseInformation databaseInformation)
        {
            string sLQuery =
                "DELETE FROM `shoppinglists` WHERE `shoppinglist_name` = @SLName AND username = @Username";

            MySqlCommand msc = new MySqlCommand(sLQuery);

            msc.Parameters.AddWithValue("@SLName", slName);

            msc.Parameters.AddWithValue("@Username", username);

            

            new SQLConnect().NonQueryMSC(msc, databaseInformation);

            
        }

        public void GoThroughIngredientsToFindProducts(DatabaseInformation databaseInformation)
        {
            string ingredientsQuery =
                "SELECT * FROM INGREDIENTS";
            MySqlCommand mscIng = new MySqlCommand(ingredientsQuery);
            DataSet ds = new SQLConnect().DynamicSimpleListSQL(mscIng, databaseInformation);

            Console.WriteLine("GOT HERE!");

            if (ds.Tables.Count != 0)
            {
                if (ds.Tables[0].Rows.Count != 0)
                {
                    foreach(DataRow r in ds.Tables[0].Rows)
                    {
                        string productsWithIngredientQuery =
                        "SELECT id FROM products WHERE productname LIKE @IngredientName";

                        MySqlCommand mscProd = new MySqlCommand(productsWithIngredientQuery);

                        mscProd.Parameters.AddWithValue("@IngredientName", r[1]);

                        DataSet pds = new SQLConnect().DynamicSimpleListSQL(mscProd, databaseInformation);

                        Console.WriteLine("GOT HERE!");

                        if (ds.Tables.Count != 0)
                        {
                            if (ds.Tables[0].Rows.Count != 0)
                            {
                                foreach(DataRow p in pds.Tables[0].Rows)
                                {
                                    string insertCombinationQuery =
                                    "INSERT INTO `products_matching_ingredients`(`ingredient_id`,`product_id`) VALUES(@IngredientID,@ProductsID)";

                                    MySqlCommand mscCombine = new MySqlCommand(insertCombinationQuery);

                                    mscCombine.Parameters.AddWithValue("@IngredientID", r[0]);
                                    mscCombine.Parameters.AddWithValue("@ProductsID", p[0]);

                                    new SQLConnect().NonQueryMSC(mscCombine, databaseInformation);
                                }
                                
                            }
                        }
                    }
                    


                }
            }

        }

        public void InsertIngredientReferenceFromId(string reference, string prodid, DatabaseInformation databaseInformation)
        {
            string insertQuery =
                "UPDATE `products` SET `ingredient_reference` = @Reference WHERE id = @Prodid";

            MySqlCommand msc = new MySqlCommand(insertQuery);

            msc.Parameters.AddWithValue("@Reference", reference);
            msc.Parameters.AddWithValue("@Prodid", prodid);

            new SQLConnect().NonQueryMSC(msc, databaseInformation);
        }

        public Product GetProductWithReferenceFromId(string id, DatabaseInformation databaseInformation)
        {
            Product product = new Product();
            ProductHandling handle = new ProductHandling();
            string getProductQuery =
                "SELECT * FROM products WHERE id = @ProdId";

            MySqlCommand msc = new MySqlCommand(getProductQuery);

            msc.Parameters.AddWithValue("@ProdId", id);

            DataSet ds = new SQLConnect().DynamicSimpleListSQL(msc, databaseInformation);

            if (ds.Tables.Count != 0)
            {
                if (ds.Tables[0].Rows.Count != 0)
                {
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        string reference = "";
                        
                        if(r[6] == DBNull.Value)
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

        public List<Product> GetProductsWhereReferenceIncludesString(string reference, DatabaseInformation databaseInformation)
        {
            List<Product> productList = new List<Product>();

            string getListQuery =
                "SELECT * FROM products WHERE reference LIKE = @Reference";

            MySqlCommand msc = new MySqlCommand(getListQuery);

            msc.Parameters.AddWithValue("@Reference", "%" + reference + "%");

            DataSet ds = new SQLConnect().DynamicSimpleListSQL(msc, databaseInformation);

            if (ds.Tables.Count != 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Product product = new Product((string)r[0], (string)r[1], (string)r[2], Convert.ToDouble(r[3]), (string)r[4], (string)r[5], (string) r[6]);
                    productList.Add(product);
                }
            }

            return productList;
        }
    }
}
