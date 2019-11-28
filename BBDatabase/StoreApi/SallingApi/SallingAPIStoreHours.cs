namespace BBCollection.StoreApi.SallingApi
{
    /// <summary>
    /// Class neeeded to pass a salling store opening hours using Newtonsoft.Json lib
    /// </summary>
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
