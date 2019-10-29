using System;
using System.Collections.Generic;
using System.Text;

namespace FrontEnd2
{
    class SallingAPIProduct
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
