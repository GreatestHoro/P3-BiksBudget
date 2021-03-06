﻿namespace BBCollection.StoreApi
{
    public class BearerAccessToken : IBearerToken
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
