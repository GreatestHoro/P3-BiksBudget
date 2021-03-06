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

            await Task.Run(() => new SQLConnect().NonQueryMSCAsync(msc));
        }

        public void AddLink(int id, string link)
        {
            string insertQuery =
                "INSERT INTO `bilkatogo_links`(`id`,`link`)values(@id,@link)";

            MySqlCommand msc = new MySqlCommand(insertQuery);

            msc.Parameters.AddWithValue("@id", id);
            msc.Parameters.AddWithValue("@link", link);

            new SQLConnect().NonQueryMSC(msc);
        }

        public async Task<Dictionary<int, string>> GetRelevantLinksAsync(string ingredient)
        {
            Dictionary<int, string> relevantLinks = new Dictionary<int, string>();
            string getLinksQuery =
                "SELECT * FROM BilkaToGo_Links WHERE link like @Ingredient";

            MySqlCommand msc = new MySqlCommand(getLinksQuery);

            msc.Parameters.AddWithValue("@Ingredient", "%" + ingredient + "%");

            DataSet ds = await new SQLConnect().DynamicSimpleListSQL(msc);

            try
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    relevantLinks.Add((int)r[0], r[1].ToString());
                }
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e);
            }
            return relevantLinks;
        }

        public async Task<bool> Exist(string productName)
        {
            string checkProduct =
                "SELECT Count(*) FROM Products WHERE productname = @ProductName";

            MySqlCommand msc = new MySqlCommand(checkProduct);

            msc.Parameters.AddWithValue("@ProductName", productName);

            return await new SQLConnect().CheckRecordExist(msc);
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

            await Task.Run(() => new SQLConnect().NonQueryMSCAsync(msc));
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
                    string? image = r[4].ToString(); string? store = r[5].ToString(); string? costumRef = r[6].ToString();
                    double? price = Convert.ToDouble(r[3]);

                    Product product = new Product(id ??= "Null", name ??= "Null", amount ??= "", price ??= 0, image ??= "", store ??= "", costumRef ??= "");
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

            await Task.Run(() => new SQLConnect().NonQueryMSCAsync(msc));
        }

        public async Task AddAutocompleteToDB()
        {
            string[] toAdd = await AddRefCol();
            double all = toAdd.Length;
            double done = 0;
            double percent = 0;
            int ToPrint = 0;

            Console.WriteLine("Autocomplete has begun...");
            foreach (string s in toAdd)
            {
                done++;
                percent = (done / all) * 100;

                if (percent > ToPrint)
                {
                    Console.WriteLine($"Status: {ToPrint}% done ({done} out of {all}).");
                    ToPrint += 10;
                }

                await ReferenceToAutcompleteToDB(s);
            }
        }

        public async Task<string[]> GetAutocompleteWords()
        {
            List<string> wordList = new List<string>();
            string table = "autocomplete_references";
            string collumn = "referenceName";

            DataSet ds = await new SQLConnect().DynamicSimpleListSQL(new SqlQuerySort().SortMSC("", table, collumn));

            if (ds.Tables.Count != 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    wordList.Add((string)r[0]);
                }
            }

            string[] returnArray = wordList.ToArray();
            Array.Sort(returnArray, (x, y) => x.Length.CompareTo(y.Length));

            return returnArray;
        }

        public async Task ReferenceToAutcompleteToDB(string s)
        {
            string[] toAdd = await AddRefCol();

            string addAutcomplete = "INSERT INTO `autocomplete_references`(`referenceName`)" +
                                    "VALUES(@ReferenceName)";

            MySqlCommand msc = new MySqlCommand(addAutcomplete);

            msc.Parameters.AddWithValue("@ReferenceName", s);

            await Task.Run(() => new SQLConnect().NonQueryMSCAsync(msc));
        }

        public async Task<string[]> AddRefCol()
        {
            List<Product> products = await ReferencesAsync("");
            List<string> viableWords = new List<string>();
            string[] prodRefs;
            products.ForEach(x => x._CustomReferenceField.ToLower());
            products.ForEach(x => viableWords.AddRange(FindViableWords(x._CustomReferenceField, x._productName)));
            products.ForEach(x => viableWords.Add(x._CustomReferenceField.Split(",").First()));

            prodRefs = new string[viableWords.Count];
            prodRefs = viableWords.Select(p => Convert.ToString(p)).ToArray();
            prodRefs = prodRefs.Distinct().ToArray();
            Array.Sort(prodRefs, (x, y) => x.Length.CompareTo(y.Length));

            return prodRefs;
        }

        public List<string> FindViableWords(string ReferenceField, string ProductName)
        {
            List<string> returnList = new List<string>();
            string[] refArray = verifyWords(ReferenceField.Split(","), ProductName);

            foreach (string sOne in refArray)
            {
                foreach (string sTwo in refArray)
                {
                    if (WordsVerified(sOne, sTwo))
                    {
                        returnList.AddRange(CheckAll(sOne, sTwo, ProductName.ToLower()));
                    }
                }
            }

            return returnList;
        }

        public string[] verifyWords(string[] wordsToTest, string pName)
        {
            pName = pName.ToLower();
            int wLength;
            int maxIndex = wordsToTest.Count();
            string[] returnArr = new string[maxIndex];
            string s;
            int j = 0;


            for (int i = 0; i < maxIndex; i++)
            {
                if (!String.IsNullOrEmpty(wordsToTest[i]) && !String.IsNullOrWhiteSpace(wordsToTest[i]))
                {
                    wLength = wordsToTest[i].Length;
                    s = wordsToTest[i];

                    if (bChar(pName, s) || aChar(pName, s, wLength))
                    {
                        returnArr[i] = s;
                        j++;
                    }
                }
            }


            returnArr = returnArr.Where(x => !String.IsNullOrEmpty(x)).ToArray();
            return returnArr;
        }

        public bool bChar(string s, string sub)
        {
            int i = s.IndexOf(sub) - 1;

            if (i > 0)
            {
                if (s[i] == ' ')
                {
                    char a = s[i];
                    return true;
                }
            }
            else
            {
                int p = i;
                return true;
            }

            char c = s[i];
            return false;
        }

        public bool aChar(string s, string sub, int l)
        {
            int t = s.IndexOf(sub);
            int i = s.IndexOf(sub) + sub.Length;
            int max = s.Count();

            if (i >= max)
            {
                int b = i;
                return true;
            }
            else if (s[i] == ' ')
            {
                char a = s[i];
                return true;
            }

            char c = s[i];
            return false;
        }

        public bool WordsVerified(string sOne, string sTwo)
        {
            return !String.IsNullOrEmpty(sOne) && !String.IsNullOrEmpty(sTwo) && sOne != sTwo;
        }

        public List<string> CheckAll(string wordOne, string wordTwo, string prodname)
        {
            List<string> returnList = new List<string>();
            string[] test = { "", " ", " i ", " med ", " fra ", " m." };
            string returnString = "";

            foreach (string s in test)
            {
                returnString = CheckCombination(wordOne, wordTwo, s, prodname);

                if (!String.IsNullOrEmpty(returnString))
                {
                    returnList.Add(returnString);
                }
            }

            return returnList;
        }

        public string CheckCombination(string wordOne, string wordTwo, string split, string prodName)
        {
            string isViable = $"{wordOne}{split}{wordTwo}";

            if (prodName.Contains(isViable))
            {
                return isViable;
            }

            return "";
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

            if (ds.Tables.Count != 0)
            {
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count != 0)
                {
                    return Convert.ToDouble(ds.Tables[0].Rows[0][0]);
                }
            }
            return 0;
        }

        public async Task SetReferenceEmpty(string prodID)
        {
            string updateColumn =
                "UPDATE products SET ingredient_reference = '' WHERE id = @ProdID";

            MySqlCommand msc = new MySqlCommand(updateColumn);

            msc.Parameters.AddWithValue("@ProdID", prodID);

            await new SQLConnect().NonQueryMSCAsync(msc);
        }
    }
}
