using System.Collections.Generic;

namespace BBCollection.StoreApi.SallingApi
{
    /// <summary>
    /// Class neeeded to pass a collection of salling stores using Newtonsoft.Json lib
    /// </summary>
    public class SallingAPIProductSuggestions
    {
        public List<SallingAPIProduct> Suggestions { get; set; }
    }
}
