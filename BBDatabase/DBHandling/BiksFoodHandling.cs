using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BBCollection.BBObjects;
using BBCollection.DBConncetion;
using Newtonsoft.Json;

namespace BBCollection.DBHandling
{
    public class BiksFoodHandling
    {
        readonly ConnectionSettings connectionSettings = new ConnectionSettings();
        HttpResponseMessage response = new HttpResponseMessage();
        HttpClient Http = new HttpClient();

        public async Task<List<WeightedRecipies>> InitializeBiksFoodProcces(List<Product> storage) 
        {
            await SendProductList(ConvertProductsToString(storage));
            return null;
        }

        private string ConvertProductsToString(List<Product> storage) 
        {
            return JsonConvert.SerializeObject(storage);
        }

        async Task<HttpResponseMessage> SendProductList(string productString)
        {
            var content = new StringContent(productString, Encoding.UTF8, "application/json");
            response = await Http.PostAsync(connectionSettings.GetApiLink() + "api/biksfood" + "/", content);

            return response;
        }

        async Task<WeightedRecipies> RecieveRecipes()
        {
            

            return null;
        }

    }
}
