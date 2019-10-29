using System;
using System.Collections.Generic;
using System.Text;

namespace FrontEnd2
{
    class SallingAPIStoreAttributes
    {
        public bool garden {get; set;}
        public bool holidayOpen { get; set; }
        public bool nonFood { get; set; }
        public bool open247 { get; set; }
        public bool petFood { get; set; }
        public string smileyScheme { get; set; }

        public override string ToString()
        {
            string str = "Garden: " + garden + " | Holiday Open: " + holidayOpen + " | nonFood: " + nonFood + " | Open 24/7: " + open247 + " | Pet Food: " + petFood + "\n Smiley Scheme: " + smileyScheme;  
            return str;
        }
    }
}
