using System.Collections.Generic;


namespace BBCollection.BBObjects
{
    public class StoreFilterList
    {
        public List<FilterItem> storeList = new List<FilterItem>()
        {
            new FilterItem("Bilka", true),
            //new FilterItem("Foetex", true),
            new FilterItem("Fakta", true),
            //new FilterItem("Dagli'Brugsen", true),
            //new FilterItem("Irma", true),
            //new FilterItem("Kvickly", true),
            //new FilterItem("Salling", true),
            new FilterItem("SuperBrugsen", true),
            //new FilterItem("Netto", true),
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
            new FilterItem("Organic", "øko", false),
            new FilterItem("Glutenfree", "gluten", false),
            new FilterItem("Lactose free", "laktosefri", false)
        };

        public List<FilterItem> GetWordList()
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
        public string SearchName { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsSortable { get; set; }

        public FilterItem(string _filterName, bool _isEnabled)
        {
            FilterName = _filterName;
            IsEnabled = _isEnabled;
        }

        public FilterItem(string _filterName, string _searchName, bool _isEnabled)
        {
            FilterName = _filterName;
            SearchName = _searchName;
            IsEnabled = _isEnabled;
        }

        public FilterItem(string _filterName, bool _isEnabled, bool _isSortable)
        {
            FilterName = _filterName;
            IsEnabled = _isEnabled;
            IsSortable = _isSortable;
        }

        public FilterItem() { }

        public List<Product> Sort(List<Product> inputList)
        {
            return new List<Product>();
        }
    }
}