using System.Collections.Generic;

namespace BBCollection.BBObjects
{
    public class SortoptionsList
    {
        public List<FilterItem> optionsList = new List<FilterItem>();

        public SortoptionsList()
        {
            optionsList.Add(new FilterItem("Relevance", (int)SortNames.Relevance, true, true));
            optionsList.Add(new FilterItem("Lowest Price", (int)SortNames.LowestPrice, false, true));
            optionsList.Add(new FilterItem("Heighest Price", (int)SortNames.HeigestPrice, false, true));
            optionsList.Add(new FilterItem("Producst (A - Z)", (int)SortNames.ProductAZ, false, true));
            optionsList.Add(new FilterItem("Products (Z - A)", (int)SortNames.ProductZA, false, true));
            optionsList.Add(new FilterItem("Store (A - Z)", (int)SortNames.StoreAZ, false, true));
            optionsList.Add(new FilterItem("Store (Z - A)", (int)SortNames.StoreZA, false, true));
        }


        public List<FilterItem> GetSortList()
        {
            return optionsList;
        }
    }

    public enum SortNames
    {
        Relevance,
        LowestPrice,
        HeigestPrice,
        ProductAZ,
        ProductZA,
        StoreAZ,
        StoreZA,
    }
}
