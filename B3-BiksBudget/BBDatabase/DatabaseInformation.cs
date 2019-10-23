using System;
using System.Collections.Generic;
using System.Text;

namespace B3_BiksBudget.BBDatabase
{
    class DatabaseInformation
    {
        public string ServerName { get; }
        public string DatabaseName { get; }
        public string DatabaseUser { get; }
        public string DatabasePassword { get; }


        public DatabaseInformation(string sName, string dBName, string dBUser, string dBPw)
        {
            ServerName = sName;
            DatabaseName = dBName;
            DatabaseUser = dBUser;
            DatabasePassword = dBPw;
        }

        public string connectionString(bool withDB)
        {
            if (withDB)
            {
                return @"server=" + ServerName + ";database="+ DatabaseName +";userid=" + DatabaseUser + ";password=" + DatabasePassword;
            }
            else
            {
                return @"server=" + ServerName + ";userid=" + DatabaseUser + ";password=" + DatabasePassword;
            }
        }
    }
}
