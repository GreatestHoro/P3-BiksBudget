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
        string WeightedRecipiesList;
        List<WeightedRecipies> WeightedList = new List<WeightedRecipies>();
        HttpClient Http = new HttpClient();

        public async Task<List<WeightedRecipies>> GetWeihtedRecipes(string id)
        {
            WeightedRecipiesList = await Http.GetStringAsync(connectionSettings.GetApiLink() + "api/biksfood" + "/"+id);
            WeightedList = JsonConvert.DeserializeObject<List<WeightedRecipies>>(WeightedRecipiesList);

            return WeightedList;
        }



    }
}
