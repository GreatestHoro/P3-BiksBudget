using System.Collections.Generic;

namespace BBCollection.BBObjects
{
    public class SortoptionsList
    {
        public SortNames name = new SortNames();
        public List<FilterItem> optionsList = new List<FilterItem>();

        public SortoptionsList()
        {
            optionsList.Add( new FilterItem(name.relevance, true, true));
            optionsList.Add(new FilterItem(name.lowToHight, false, true));
            optionsList.Add(new FilterItem(name.heigestPrice, false, true));
            optionsList.Add(new FilterItem(name.productAZ, false, true));
            optionsList.Add(new FilterItem(name.productZA, false, true));
            optionsList.Add(new FilterItem(name.storeAZ, false, true));
            optionsList.Add(new FilterItem(name.storeZA, false, true));
        }


        public List<FilterItem> GetSortList()
        {
            return optionsList;
        }
    }

    public class SortNames
    {
        public string relevance = "Relevance";
        public string lowToHight = "Lowest price";
        public string heigestPrice = "Heighest price";
        public string productAZ = "Product (A-Z)";
        public string productZA = "Product (Z-A)";
        public string storeAZ = "Store (A-Z)";
        public string storeZA = "Store (Z-A)";
    }
}
