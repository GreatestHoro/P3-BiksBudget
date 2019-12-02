using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BBCollection.DBConncetion
{
    public class SQLConnect
    {
        DatabaseInformation databaseInformation = new DatabaseInformation();

        public async void NonQueryString(string sqlQuery)
        {
            MySqlConnection connection = null;
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

        }
        public void NonQueryMSC(MySqlCommand msc)
        {
            MySqlConnection connection = null;

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
        }
        public DataSet DynamicSimpleListSQL(MySqlCommand mscom)
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

                /*string ingredientsToRecipeQuery = "SELECT * FROM IngredientsInRecipe WHERE recipeID = @RecipeID";
                MySqlCommand msc = new MySqlCommand(ingredientsToRecipeQuery, connection);
                msc.Parameters.AddWithValue("@RecipeID", recipeID);
                MySqlDataReader msdr = msc.ExecuteReader();

                while (msdr.Read())
                {
                    Ingredient ingredient = new Ingredient(msdr.GetString("ingredientName"), msdr.GetString("unit"), msdr.GetInt32("amount"));
                    ingredients.Add(ingredient);
                }*/
            }
            catch(MySqlException e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                if(connection != null)
                {
                    connection.Close();
                }
            }

            return ds;
        }
        public bool CheckRecordExist(MySqlCommand msc)
        {
            bool exist = false;
            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(databaseInformation.ConnectionString(true));
                connection.Open();

                msc.Connection = connection;

                int amountOfObjects = Convert.ToInt32(msc.ExecuteScalar());

                if(amountOfObjects > 0)
                {
                    exist = true;
                }

            } catch(MySqlException e)
            {
                Console.WriteLine(e);
            } finally
            {
                if(connection != null)
                {
                    connection.Close();
                }
            }
            return exist;
        }
    }
}
