using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace IpData
{
    public class IpItem
    {
        public string ip { get; set; }
        public string type { get; set; }
        public string continent_code { get; set; }
        public string continent_name { get; set; }
        public string country_code { get; set; }
        public string country_name { get; set; }
        public string region_code { get; set; }
        public string region_name { get; set; }
        public string city { get; set; }
        public string zip { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public Location location { get; set; }
        public TimeZone time_zoe { get; set; }
        public Currency currency { get; set; }
        public Connection connection { get; set; }

    }

    public class Connection
    {
        public string asn { get; set; }
        public string isp { get; set; }
    }

    public class Currency
    {
        public string code { get; set; }
        public string name { get; set; }
        public string plural { get; set; }
        public string symbol { get; set; }
        public string symbol_native { get; set; }
    }

    public class TimeZone
    {
        public string id { get; set; }
        public string current_time { get; set; }
        public int gmt_offset { get; set; }
        public string EDT { get; set; }
        public bool is_dayligh_saving { get; set; }
    }

    public class languages
    {
        public string code { get; set; }
        public string name { get; set; }
        public string native { get; set; }
    }

    public class Location
    {
        public int genoname_id { get; set; }
        public string capital { get; set; }
        public languages languages { get; set; }
        public string country_flag { get; set; }
        public string country_flag_emoji { get; set; }
        public string country_flag_emoji_unicode { get; set; }
        public string calling_code { get; set; }


    }
}
