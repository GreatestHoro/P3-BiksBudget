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
using FrontEnd2.Data;

namespace FrontEnd2.Data
{
    class StoreFilterList
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
            int i = 0;

            foreach (var item in storeList)
            {
                item.Id = i++;
            }

            return storeList;
        }
    }

    class WordFilterList
    {
        public List<FilterItem> worFilterlist = new List<FilterItem>()
        {
            new FilterItem("Økologisk", true),
            new FilterItem("Glutenfri", true),
        };

        public List<FilterItem> GetStoreList()
        {
            int i = 0;

            foreach (var item in worFilterlist)
            {
                item.Id = i++;
            }

            return worFilterlist;
        }
    }

    public class FilterItem
    {
        public string FilterName { get; set; }
        public bool IsEnabled { get; set; }
        public int Id { get; set; }

        public FilterItem(string _filterName, bool _isEnabled)
        {
            FilterName = _filterName;
            IsEnabled = _isEnabled;
        }
    }
}