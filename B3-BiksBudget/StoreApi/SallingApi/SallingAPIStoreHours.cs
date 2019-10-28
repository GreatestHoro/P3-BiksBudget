using System;
using System.Collections.Generic;
using System.Text;

namespace BiksAPI
{
    class SallingAPIStoreHours
    {
        public string date { get; set; }
        public string type { get; set; }
        public string open { get; set; }
        public string close { get; set; }
        public bool closed { get; set; }

        public override string ToString()
        {
            string str = "Date: " + date + "\nType: " + type + "\nOpen: " + open + "\nClose: " + close + "\nClosed: " + closed;
            return str;
        }
    }
}
