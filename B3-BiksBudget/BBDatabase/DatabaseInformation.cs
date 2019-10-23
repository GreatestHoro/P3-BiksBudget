using System;
using System.Collections.Generic;
using System.Text;

namespace B3_BiksBudget.BBDatabase
{
    class DatabaseInformation
    {
        public string serverName { get; }
        public string databaseName { get; }
        public string databaseUser { get; }
        public string databasePassword { get; }


        public DatabaseInformation(string SName, string DBName, string DBUser, string DBPw)
        {
            serverName = SName;
            databaseName = DBName;
            databaseUser = DBUser;
            databasePassword = DBPw;
        }

        public string connectionString(bool withDB)
        {
            if (withDB)
            {
                return @"server=" + serverName + ";database="+ databaseName +";userid=" + databaseUser + ";password=" + databasePassword;
            }
            else
            {
                return @"server=" + serverName + ";userid=" + databaseUser + ";password=" + databasePassword;
            }
        }
    }
}
