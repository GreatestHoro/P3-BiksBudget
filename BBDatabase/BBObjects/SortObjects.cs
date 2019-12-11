using System;
using System.Collections.Generic;
using System.Text;

namespace BBCollection.BBObjects
{
    public class SortObjects
    {
        public SortObjects(FilterItem _filterItem)
        {
            filterItem = _filterItem;
        }

        public FilterItem filterItem;

        public List<Product> Sort()
        {
            return new List<Product>();
        }
    }
}
