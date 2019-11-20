using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BBCollection.StoreApi.ApiNeeds
{
    public class Filter<T>
    {
        public List<T> GetFiltered(List<T> inputList, Predicate<T> predicate)
        {
            List<T> resList = new List<T>();
            foreach(T item in inputList)
            {
                if (predicate(item))
                {
                    resList.Add(item);
                }
            }
            return resList;
        }
    }
}
