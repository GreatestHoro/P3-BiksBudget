using System;
using System.Collections.Generic;
using System.Text;

namespace BiksAPI
{
    class CoopStoreApi
    {
        public int CurrentPage { get; set; }
        public int NextPage { get; set; }
        public int PageCount { get; set; }
        public int TotalPagedItemsCount { get; set; }
        public List<CoopStoreData> Data { get; set; }
        public bool ApiObsolete { get; set; }
        public string ApiVersion { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
        
    }
}
