using System.Collections.Generic;

namespace BBCollection.BBObjects
{
    public class SortoptionsList
    {
        public List<FilterItem> optionsList = new List<FilterItem>()
        {
            new FilterItem("Relevance", true, true),
            new FilterItem("Lowest price", false, true),
            new FilterItem("Heighest price", false, true),
            new FilterItem("Product (A-Z)", false, true),
            new FilterItem("Product (Z-A)", false, true),
            new FilterItem("Store (A - Z)", false, true),
            new FilterItem("Store (Z - A", false, true)
        };

        public List<FilterItem> GetSortList()
        {
            return optionsList;
        }
    }
    public class Options
    {
        public Options(string _filterName, bool _isEnabled)
        {
            FilterName = _filterName;
            IsEnabled = _isEnabled;
        }

        public string FilterName { get; set; }
        public bool IsEnabled { get; set; }

    }
}
