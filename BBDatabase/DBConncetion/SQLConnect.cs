using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Threading.Tasks;

namespace BBCollection.DBConncetion
{
    public class SQLConnect
    {
        DatabaseInformation databaseInformation = new DatabaseInformation();

        public async Task NonQueryString(string sqlQuery)
        {
            MySqlConnection connection = null;

            await Task.Run(() =>
            {
                try
                {
                    connection = new MySqlConnection(databaseInformation.ConnectionString(true));
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
            });

        }
        public async Task NonQueryMSC(MySqlCommand msc)
        {
            MySqlConnection connection = null;

            await Task.Run(() =>
            {
                try
                {
                    connection = new MySqlConnection(databaseInformation.ConnectionString(true));
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
            });
        }

        public async Task<DataSet> DynamicSimpleListSQL(MySqlCommand mscom)
        {
            MySqlConnection connection = null;
            DataSet ds = null;

            return await Task.Run(() =>
           {
               try
               {
                   connection = new MySqlConnection(databaseInformation.ConnectionString(true));
                   connection.Open();

                   mscom.Connection = connection;

                   MySqlDataAdapter msda = new MySqlDataAdapter(mscom);

                   msda.Fill(ds = new DataSet());
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
               return ds;
           });
        }

        public DataSet DynamicSimpleListSQLSync(MySqlCommand mscom)
        {
            MySqlConnection connection = null;
            DataSet ds = null;
            try
            {
                connection = new MySqlConnection(databaseInformation.ConnectionString(true));
                connection.Open();

                mscom.Connection = connection;

                MySqlDataAdapter msda = new MySqlDataAdapter(mscom);

                msda.Fill(ds = new DataSet());
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
            return ds;
        }

        public async Task<bool> CheckRecordExist(MySqlCommand msc)
        {
            bool exist = false;
            MySqlConnection connection = null;

            return await Task.Run(() =>
            {
                try
                {
                    connection = new MySqlConnection(databaseInformation.ConnectionString(true));
                    connection.Open();

                    msc.Connection = connection;

                    int amountOfObjects = Convert.ToInt32(msc.ExecuteScalar());

                    if (amountOfObjects > 0)
                    {
                        exist = true;
                    }

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
                return exist;
            });
        }
    }
}
