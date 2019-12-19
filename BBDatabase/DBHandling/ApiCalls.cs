using BBCollection.DBConncetion;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BBCollection.DBHandling
{
    class ApiCalls
    {
        public ApiCalls(string _dest, string _username)
        {
            dest = _dest;
            username = _username;
        }

        HttpResponseMessage response = new HttpResponseMessage();
        readonly ConnectionSettings connectionSettings = new ConnectionSettings();
        ControllerFuncionality features = new ControllerFuncionality();
        HttpClient Http = new HttpClient();

        string dest;
        string username;

        public async Task<string> Get()
        {
            return await Http.GetStringAsync(connectionSettings.GetApiLink() + dest + "/" + username);
        }

        public async Task<HttpResponseMessage> Post(string productString)
        {
            var content = new StringContent(productString, Encoding.UTF8, "application/json");
            response = await Http.PostAsync(connectionSettings.GetApiLink() + dest + "/" + username, content);

            return response;
        }

        public async Task<HttpResponseMessage> Put(string productString)
        {
            var content = new StringContent(productString, Encoding.UTF8, "application/json");
            response = await Http.PutAsync(connectionSettings.GetApiLink() + dest + "/" + username, content);

            return response;
        }
    }
}
