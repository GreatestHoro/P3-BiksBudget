using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace FrontEnd2
{
    class OpenHttp<T>
    {
        HttpWebRequest _httpWebRequest { get; set; }
        string _apiLink { get; set; }
        string _accessToken { get; set; }
        string _jsonData { get; set; }

        public OpenHttp(string apiLink, string accessToken)
        {
            _apiLink = apiLink;
            _accessToken = accessToken;
        }

        public OpenHttp(string accessToken)
        {
            _accessToken = accessToken;
        }

        public void ChangeUrl(string newUrl)
        {
            _apiLink = newUrl;
        }

        #region Public Methods
        public List<T> ReadAndParseAPI()
        {

            HttpWebRequest httpWebRequest = APIHttpWebReqeust();

            HttpWebResponse response = httpWebRequest.GetResponse() as HttpWebResponse;

            StreamReader streamReader = new StreamReader(response.GetResponseStream());

            JsonTextReader reader = new JsonTextReader(streamReader);

            reader.SupportMultipleContent = true;
            List<T> resList = new List<T>();

            var serializer = new JsonSerializer();
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.StartObject)
                {
                    resList.Add(serializer.Deserialize<T>(reader));
                }
            }

            return resList;
        }

        public async Task<List<T>> ReadAndParseAPI(string searchword)
        {

            HttpWebRequest httpWebRequest = APIHttpWebReqeust();

            HttpWebResponse response = await httpWebRequest.GetResponseAsync() as HttpWebResponse;

            StreamReader streamReader = new StreamReader(response.GetResponseStream());

            JsonTextReader reader = new JsonTextReader(streamReader);

            reader.SupportMultipleContent = true;
            List<T> resList = new List<T>();

            var serializer = new JsonSerializer();
            while (await reader.ReadAsync())
            {
                if (reader.TokenType == JsonToken.StartObject /*&& reader.DateFormatString.Contains(searchword)*/)
                {

                    resList.Add(serializer.Deserialize<T>(reader));
                }
            }

            return resList;
        }

        public T ReadAndParseAPISingle()
        {
            HttpWebRequest httpWebRequest = APIHttpWebReqeust();

            HttpWebResponse response = httpWebRequest.GetResponse() as HttpWebResponse;

            StreamReader streamReader = new StreamReader(response.GetResponseStream());

            JsonTextReader reader = new JsonTextReader(streamReader);

            reader.SupportMultipleContent = true;
            T resObject = default(T);

            var serializer = new JsonSerializer();

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.StartObject)
                {
                    resObject = serializer.Deserialize<T>(reader);
                }
            }

            return resObject;
        }
        #endregion

        #region Private Methods
        private HttpWebRequest APIHttpWebReqeust()
        {

            _httpWebRequest = (HttpWebRequest)WebRequest.Create(_apiLink);

            IWebProxy theProxy = _httpWebRequest.Proxy;
            if (theProxy != null)
            {
                theProxy.Credentials = CredentialCache.DefaultCredentials;
            }
            CookieContainer cookies = new CookieContainer();
            _httpWebRequest.UseDefaultCredentials = true;
            _httpWebRequest.CookieContainer = cookies;
            _httpWebRequest.ContentType = "application/json";
            _httpWebRequest.CookieContainer = cookies;

            // write the "Authorization" header
            _httpWebRequest.Headers.Add("Authorization", "Bearer " + _accessToken);
            //_httpWebRequest.Headers.Add("Ocp-Apim-Subscription-Key", _accessToken);

            _httpWebRequest.Method = "GET";

            return _httpWebRequest;

        }

        #endregion
    }
}
