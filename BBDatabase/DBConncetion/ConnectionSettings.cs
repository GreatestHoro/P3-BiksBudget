using System;
using System.Collections.Generic;
using System.Text;

namespace BBCollection.DBConncetion
{
    public class ConnectionSettings
    {
        public string _DatabaseName;
        private bool _onlineDB = false;
        private bool _onlineAPI = false;

        // Server = biksbudgetdb.mysql.database.azure.com, Database = BiksBudgetDB, User id = BBAdmin@biksbudgetdb, Password = BiksBudget1
        // Database connection strings for online use
        private string _MySqlOnlineWithtDB;
        private string _MySqlOnlineWithouthDB;

        // Server = "localhost", Database = "biksbudgetDB", User id = "root", Password = "BiksBudget123"
        // Database connection strings for local use
        private string _MySqlLocalWithDB;
        private string _MySqlLocalWithoutDB;

        // API Online
        private string _AzureApiLink;

        //API Localhost
        private string _LocalApiLink;
        
        public ConnectionSettings()
        {
            _DatabaseName = "BiksBudgetDB";

            _MySqlOnlineWithtDB = @"server=biksbudgetdb.mysql.database.azure.com;database=" + _DatabaseName + ";userid=BBAdmin@biksbudgetdb;password=BiksBudget1";
            _MySqlOnlineWithouthDB = @"server=biksbudgetdb.mysql.database.azure.com;userid=BBAdmin @biksbudgetdb; password=BiksBudget1";

            _MySqlLocalWithDB = @"server=localhost;database="+ _DatabaseName +";userid=root;password=BiksBudget123";
            _MySqlLocalWithoutDB = @"server=localhost;userid=root;password=BiksBudget123";

            _AzureApiLink = "https://backend20191127044711.azurewebsites.net/";
            _LocalApiLink = "https://localhost:44325/";
        }


        public Tuple<string, string> GetDatabaseStrings()
        {
            if (_onlineDB)
            {
                return new Tuple<string, string>(_MySqlOnlineWithtDB, _MySqlOnlineWithouthDB);
            }
            else
            {
                return new Tuple<string, string>(_MySqlLocalWithDB, _MySqlLocalWithoutDB);
            }
        }

        public string GetApiLink()
        {
            if (_onlineAPI)
            {
                return _AzureApiLink;
            }
            else
            {
                return _LocalApiLink;
            }
        }
    }
}
