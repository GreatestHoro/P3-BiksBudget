using BBCollection.BBObjects;
using BBCollection.DBConncetion;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BBCollection.DBHandling
{
    public class LoginRegister
    {
        string userString;
        HttpClient http = new HttpClient();
        ConnectionSettings connectionSettings = new ConnectionSettings();
        HttpResponseMessage response = new HttpResponseMessage();

        public async Task<HttpResponseMessage> Login(User user)
        {
            userString = JsonConvert.SerializeObject(user);

            var content = new StringContent(userString, Encoding.UTF8, "application/json");

            response = await http.PostAsync(connectionSettings.GetApiLink() + "api/Login", content);

            return response;
        }

        public async Task<HttpResponseMessage> Register(User user)
        {
            userString = JsonConvert.SerializeObject(user);

            var content = new StringContent(userString, Encoding.UTF8, "application/json");

            response = await http.PostAsync(connectionSettings.GetApiLink() + "User/Register", content);

            return response;
        }
    }
}
