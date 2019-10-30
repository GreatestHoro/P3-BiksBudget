using System.Collections.Generic;

namespace FrontEnd2
{
    class SallingAPIStore
    {
        public SallingAPIStoreAdress address { get; set; }
        public string brand { get; set; }
        public double[] coordinates { get; set; }
        public string created { get; set; }
        public string distance_km { get; set; }
        public List<SallingAPIStoreHours> hours { get; set; }
        public string modified { get; set; }
        public string name { get; set; }
        public string phoneNumber { get; set; }
        public string sapSiteId { get; set; }
        public string type { get; set; }
        public string vikingStoreId { get; set; }
        public SallingAPIStoreAttributes attributes { get; set; }
        public string id { get; set; }

        public override string ToString()
        {
            string str = "Address: " + address.ToString();
            str += "\nBrand: " + brand;
            str += "\nCoordinates: " + coordinates[0] + ", " + coordinates[1];
            str += "\nCreated: " + created;
            str += "\nDistance km: " + distance_km;
            str += "\nHours: " + hoursString();
            str += "\nModified: " + modified;
            str += "\nName: " + name;
            str += "\nphoneNumber: " + phoneNumber;
            str += "\nsapSiteId: " + sapSiteId;
            str += "\ntype: " + type;
            str += "\nvikingStoreId: " + vikingStoreId;
            str += "\nAttributes: " + attributes.ToString();
            str += "\nid: " + id;
            return str;
        }

        private string hoursString()
        {
            string resStr = "";

            foreach (SallingAPIStoreHours h in hours)
            {
                resStr += "\n\n" + h.ToString();
            }

            return resStr;
        }
    }
}