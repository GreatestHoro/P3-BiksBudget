using System;
using System.Collections.Generic;
using System.Text;

namespace BBCollection.BBObjects
{
    public class SortoptionsList
    {
        public List<Options> optionsList = new List<Options>()
        {
            new Options("Most relevant", true),
            new Options("Price (Low to High)", false),
            new Options("Price (High to Low)", false),
            new Options("Alphabetical order (A - Z)", false),
            new Options("Alphabetical order (Z - A)", false),
            new Options("Store (A - Z)", false),
            new Options("Store (Z - A", false)
        };

        public List<Options> GetList()
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
