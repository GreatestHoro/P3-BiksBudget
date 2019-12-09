using MySql.Data.MySqlClient;

namespace BBCollection.DBConncetion
{
    public class SqlQuerySort
    {
        public MySqlCommand SortMSC(string sortItem, string table, string collumn)
        {
            string recipesQuery =
                "SELECT * FROM " + table + " WHERE " + collumn + " LIKE @itemName " +
                " ORDER BY case " +
                "WHEN " + collumn + " like @itemNameSort1 then 1 " +
                "WHEN " + collumn + " like @itemNameSort2 then 2 " +
                "WHEN " + collumn + " like @itemNameSort3 then 3 " +
                "WHEN " + collumn + " like @itemNameSort4 then 4 " +
                "WHEN " + collumn + " like @itemNameSort5 then 5 " +
                "ELSE 6 END;";

            MySqlCommand msc = new MySqlCommand(recipesQuery);
            msc.Parameters.AddWithValue("@itemName", "%" + sortItem + "%");

            msc.Parameters.AddWithValue("@itemNameSort1", "% " + sortItem);
            msc.Parameters.AddWithValue("@itemNameSort2", "% " + sortItem + " %");
            msc.Parameters.AddWithValue("@itemNameSort3", sortItem + " %");
            msc.Parameters.AddWithValue("@itemNameSort4", sortItem + "%");
            msc.Parameters.AddWithValue("@itemNameSort5", "%" + sortItem);

            return msc;
        }
        public MySqlCommand SortMSCInterval(string sortItem, string table, string collumn, int limit, int offset)
        {
            string recipesQuery =
                "SELECT * FROM " + table + " WHERE " + collumn + " LIKE @itemName " +
                " ORDER BY case " +
                "WHEN " + collumn + " like @itemNameSort1 then 1 " +
                "WHEN " + collumn + " like @itemNameSort2 then 2 " +
                "WHEN " + collumn + " like @itemNameSort3 then 3 " +
                "WHEN " + collumn + " like @itemNameSort4 then 4 " +
                "WHEN " + collumn + " like @itemNameSort5 then 5 " +
                "ELSE 6 END LIMIT @Limit OFFSET @Offset;";

            MySqlCommand msc = new MySqlCommand(recipesQuery);
            msc.Parameters.AddWithValue("@itemName", "%" + sortItem + "%");

            msc.Parameters.AddWithValue("@itemNameSort1", "% " + sortItem);
            msc.Parameters.AddWithValue("@itemNameSort2", "% " + sortItem + " %");
            msc.Parameters.AddWithValue("@itemNameSort3", sortItem + " %");
            msc.Parameters.AddWithValue("@itemNameSort4", sortItem + "%");
            msc.Parameters.AddWithValue("@itemNameSort5", "%" + sortItem);
            msc.Parameters.AddWithValue("@Limit", limit);
            msc.Parameters.AddWithValue("@Offset", offset);

            return msc;
        }
    }
}
