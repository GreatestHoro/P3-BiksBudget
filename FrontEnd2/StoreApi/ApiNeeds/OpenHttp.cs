using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace FrontEnd2
{
    /// <summary>
    /// used for opening a http connection to APIS and for reading from it
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class OpenHttp<T>
    {
        HttpWebRequest _httpWebRequest { get; set; }
        string _apiLink { get; set; }
        string _accessToken { get; set; }
        string _jsonData { get; set; }
        #region Constructors
        public OpenHttp(string apiLink, string accessToken)
        {
            _apiLink = apiLink;
            _accessToken = accessToken;
        }

        public OpenHttp(string accessToken)
        {
            _accessToken = accessToken;
        }
        #endregion
        /// <summary>
        /// Function to update the URL of openhttp class
        /// </summary>
        /// <param name="newUrl">new url</param>
        public void ChangeUrl(string newUrl)
        {
            _apiLink = newUrl;
        }

        #region Public Methods
        /// <summary>
        /// Step 1: Opens a connection to the _apiLink and reads the information
        /// Step 2: Read the information with a streamreader
        /// Step 3: parse the information to objects
        /// </summary>
        /// <returns>a list of the object parsed from the api</returns>
        public List<T> ReadAndParseAPI()
        {
            // create HTTP header tuned for an API call
            HttpWebRequest httpWebRequest = APIHttpWebReqeust();

            HttpWebResponse response = httpWebRequest.GetResponse() as HttpWebResponse;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                // create a streamreader to read from the response
                StreamReader streamReader = new StreamReader(response.GetResponseStream());

                // create a json reader from the streamreader
                JsonTextReader reader = new JsonTextReader(streamReader);

                reader.SupportMultipleContent = true;
                List<T> resList = new List<T>();

                var serializer = new JsonSerializer();
                try
                {
                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonToken.StartObject)
                        {
                            resList.Add(serializer.Deserialize<T>(reader));
                        }
                    }
                }
                catch (Newtonsoft.Json.JsonReaderException e)
                {
                    throw new ArgumentNullException(e.Message);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

                return resList;
            }

            return new List<T>();
        }

        /// <summary>
        /// Same as above, just asynchronus instead
        /// </summary>
        /// <param name="searchword"></param>
        /// <returns></returns>
        public async Task<List<T>> ReadAndParseAPI(string searchword)
        {

            HttpWebRequest httpWebRequest = APIHttpWebReqeust();

            HttpWebResponse response = await httpWebRequest.GetResponseAsync() as HttpWebResponse;

            StreamReader streamReader = new StreamReader(response.GetResponseStream());

            JsonTextReader reader = new JsonTextReader(streamReader);

            reader.SupportMultipleContent = true;
            List<T> resList = new List<T>();

            var serializer = new JsonSerializer();
            try
            {
                while (await reader.ReadAsync())
                {
                    if (reader.TokenType == JsonToken.StartObject /*&& reader.DateFormatString.Contains(searchword)*/)
                    {

                        resList.Add(serializer.Deserialize<T>(reader));
                    }
                }

            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                throw new ArgumentNullException(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return resList;
        }

        /// <summary>
        /// Function to open http connection and read from an API, but expects to read a single object rather a collection. 
        /// The rest is as ReadAndParseAPI()
        /// </summary>
        /// <returns>An object read and parsed from the api</returns>
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
        /// <summary>
        /// Sets up the HTTP header to access the API through the _apiLink
        /// </summary>
        /// <returns>a httpWebRequest to access an API by bearer token</returns>
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
            if (_accessToken.Contains("-"))
            {
                _httpWebRequest.Headers.Add("Authorization", "Bearer " + _accessToken);
            }
            else
            {
                _httpWebRequest.Headers.Add("Ocp-Apim-Subscription-Key", _accessToken);

            }

            _httpWebRequest.Method = "GET";

            return _httpWebRequest;

        }

        #endregion
    }
}
