using System;
using System.Collections.Generic;
using System.Text;

namespace BiksAPI
{
    class BearerAccessToken : IBearerToken
    {
        string _bearerToken { get; set; }

        public BearerAccessToken(string bearerToken)
        {
            _bearerToken = bearerToken;
        }

        public string GetBearerToken()
        {
            return _bearerToken;
        }
    }
}
