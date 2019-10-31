using System.Collections.Generic;

namespace BBGatherer.StoreApi.CoopApi
{
    class CoopDoStuff
    {
        enum Store
        {
            // StoreId for the different stores
            DagliBrugsen = 2096,
            fakta = 24073
        }

        public string _token { get; set; }

        CoopAPILinks linkMaker = new CoopAPILinks();


        public CoopDoStuff(string token)
        {
            _token = token;
        }

        public object CoopCloseStore()
        {
            //BearerAccessToken bearerAccessToken = new BearerAccessToken("f0cabde6bb8d4bd78c28270ee203253f"); /* Code #1 */
            //BearerAccessToken bearerAccessToken = new BearerAccessToken("d0b9a5266a2749cda99d4468319b6d9f"); /* Code #2 */

            BearerAccessToken bearerAccessToken = new BearerAccessToken(_token);

            // Create link to a store with radius as input
            string url = linkMaker.GetRadiusLink(10000);

            // Open the link using the url and token
            OpenHttp<CoopStoreApi> openHttp = new OpenHttp<CoopStoreApi>(url, bearerAccessToken.GetBearerToken());

            // Parse 
            CoopStoreApi stuff = openHttp.ReadAndParseAPISingle();

            // Create list for StoreId in the different stores found inside the radius
            List<int> storId = new List<int>();

            // Fill the StoreId list with Kardex (storeid) for the different stores
            foreach (var product in stuff.Data)
            {
                storId.Add(product.Kardex);
            }

            //foreach (var store in stuff.Data)
            //{
            //    Console.WriteLine(store.RetailGroupName + " - " + store.Kardex + " - " + store.Address + " - " + store.Zipcode);
            //}

            return stuff;
        }

        public List<CoopProduct> CoopFindEverythingInStore(string storeId)
        {
            BearerAccessToken bearerAccessToken = new BearerAccessToken(_token);
            OpenHttp<CoopProduct> openHttpStore = new OpenHttp<CoopProduct>(bearerAccessToken.GetBearerToken());

            string url = linkMaker.GetProductLinke(storeId);
            openHttpStore.ChangeUrl(url);

            List<CoopProduct> coopProducts = openHttpStore.ReadAndParseAPI();

            //foreach(var item in coopProducts)
            //{
            //    Console.WriteLine(item.Navn + " - " + item.Pris);
            //}

            return coopProducts;
        }

        public List<CoopProduct> CoopFindProduct(string storeId, string SearchWord)
        {
            List<CoopProduct> product = CoopFindEverythingInStore(storeId);
            List<CoopProduct> result = new List<CoopProduct>();

            foreach (var item in product)
            {
                if (item.Navn.Contains(SearchWord.ToUpper()))
                {
                    result.Add(item);
                }
            }

            //foreach(var item in result)
            //{
            //    Console.WriteLine(item.Navn + " - " + item.Pris);
            //}

            return result;
        }
    }
}
