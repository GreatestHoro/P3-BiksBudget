using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using System.Web;
using Json.Net;
using System.IO;
using Newtonsoft.Json.Linq;


namespace BBCollection.BBObjects
{
    public class StoreFilterList
    {
        public List<FilterItem> storeList = new List<FilterItem>()
        {
            new FilterItem("Bilka", true),
            new FilterItem("Føtex", true),
            new FilterItem("Fakta", true),
            new FilterItem("DagliBrugsen", true),
            new FilterItem("Irma", true),
            new FilterItem("Kvivkly", true),
            new FilterItem("Salling", true),
            new FilterItem("SuperBrugsen", true),
            new FilterItem("Netto", true),
        };

        public List<FilterItem> GetStoreList()
        {
            return storeList;
        }

        public FilterItem[] GetStoreArray()
        {
            return storeList.ToArray();
        }
    }

    public class WordFilterList
    {
        public List<FilterItem> wordFilterlist = new List<FilterItem>()
        {
            new FilterItem("Økologisk", false),
            new FilterItem("Glutenfri", false),
        };

        public List<FilterItem> GetStoreList()
        {
            return wordFilterlist;
        }

        public FilterItem[] GetWordArray()
        {
            return wordFilterlist.ToArray();
        }
    }

    public class FilterItem
    {
        public string FilterName { get; set; }
        public bool IsEnabled { get; set; }

        public FilterItem(string _filterName, bool _isEnabled)
        {
            FilterName = _filterName;
            IsEnabled = _isEnabled;
        }
    }
}