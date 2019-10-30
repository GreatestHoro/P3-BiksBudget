using MySql.Data.MySqlClient;
using System;

namespace BBCollection.DBConncetion
{
    class SQLConnect
    {
        public void NonQueryString(string sqlQuery, DatabaseConnect dbConnect)
        {
            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(dbConnect.ConnectionString(true));
                connection.Open();

                new MySqlCommand(sqlQuery, connection).ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }

        }

        public void NonQueryMSC(MySqlCommand msc, DatabaseConnect dbConnect)
        {
            MySqlConnection connection = null;

            try
            {
                connection = new MySqlConnection(dbConnect.ConnectionString(true));
                connection.Open();

                msc.Connection = connection;

                msc.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                Console.Write(e);
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }
    }
}
