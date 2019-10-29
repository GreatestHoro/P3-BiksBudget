using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace B3_BiksBudget.StoreApi.CoopApi
{
    class CoopStoreData
    {
        public int Kardex { get; set; }
        public string RetailGroupName { get; set; }
        public string Address { get; set; }
        public int Zipcode { get; set; }
        public string FacebookLink { get; set; }
        public string City { get; set; }
        public string Phonenumber { get; set; }
        public string Manager { get; set; }
        public CoopLocationData Location { get; set; }
        public List<CoopOpeningHourData> OpeningHours { get; set; }

    }
}
