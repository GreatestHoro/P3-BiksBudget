using System;
using System.Collections.Generic;
using System.Text;

namespace B3_BiksBudget.StoreApi.SallingApi
{
    class SallingAPIStoreAdress
    {
        public string city { get; set; }
        public string country { get; set; }
        public  string extra { get; set; }
        public string street { get; set; }
        public string zip { get; set; }

        public override string ToString()
        {
            string str = extra == null ? street + ", " + zip + ", " + city + ", " + country : street + ", " + zip + ", " + extra + ", " + city + ", " + country;
            return str;
        }
    }
}
