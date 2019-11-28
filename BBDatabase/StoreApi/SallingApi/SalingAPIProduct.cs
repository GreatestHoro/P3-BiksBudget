using System.Collections.Generic;

namespace BBCollection.StoreApi.SallingApi
{
    /// <summary>
    /// Class needed to use newtonsoft.json to parse a salling store from their API
    /// </summary>
    public class SallingAPIProduct
    {
        public string title { get; set; }
        public string id { get; set; }
        public string prod_id { get; set; }
        public double price { get; set; }
        public string description { get; set; }
        public string link { get; set; }
        public string img { get; set; }
        public IList<string> gtins { get; set; }
    }
}
