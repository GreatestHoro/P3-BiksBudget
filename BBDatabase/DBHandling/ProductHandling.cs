using BBCollection.BBObjects;
using BBCollection.DBConncetion;
using MySql.Data.MySqlClient;

namespace BBCollection.DBHandling
{
    public class ProductHandling
    {
        public void Insert(Product product, DatabaseConnect dbConnect)
        {
            string addProductQuery = "INSERT INTO `products`(`ean`,`productName`,`productName2`,`price`,`productHierarchyID`)" +
                                     "VALUES(@Ean,@ProductName,@ProductName2,@Price,@ProductHierarchyID);";

            MySqlCommand msc = new MySqlCommand(addProductQuery);

            msc.Parameters.AddWithValue("@Ean", product._ean);
            msc.Parameters.AddWithValue("@ProductName", product._name);
            msc.Parameters.AddWithValue("@ProductName2", product._name2);
            msc.Parameters.AddWithValue("@Price", product._price);
            msc.Parameters.AddWithValue("@ProductHierarchyID", product._productHierarchyID);

            new SQLConnect().NonQueryMSC(msc, dbConnect);
        }
    }
}
