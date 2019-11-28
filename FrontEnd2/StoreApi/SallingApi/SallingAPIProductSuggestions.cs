using System.Collections.Generic;

namespace FrontEnd2
{
    /// <summary>
    /// Class neeeded to pass a collection of salling stores using Newtonsoft.Json lib
    /// </summary>
    class SallingAPIProductSuggestions
    {
        public List<SallingAPIProduct> Suggestions { get; set; }
    }
}
