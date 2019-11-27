using System;
using System.Collections.Generic;
using System.Text;

namespace BBCollection.DBConncetion
{
    public class DatabaseInformation
    {

        public DatabaseInformation GetConnect()
        {
            return new DatabaseInformation();
        }

        public string ConnectionString(bool withDB)
        {
            if (withDB)
            {
                return new ConnectionSettings().GetDatabaseStrings().Item1;
            }
            else
            {
                return new ConnectionSettings().GetDatabaseStrings().Item2;
            }
        }
    }
}
