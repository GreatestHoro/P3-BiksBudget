using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BBCollection.StoreApi.ApiNeeds
{
    public class Filter<T>
    {
        /// <summary>
        /// Class which allows a generic list to be filtered by a generic predicate
        /// </summary>
        /// <param name="inputList">the input list of objects to be filtered</param>
        /// <param name="predicate">the predicate to filter the list by</param>
        /// <returns>returns a filtered list of the generic objects</returns>
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
