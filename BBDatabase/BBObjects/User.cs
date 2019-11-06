using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BBCollection.BBObjects
{
    public class User
    {
        public string _userName { get; }
        public List<Product> _storage { get; }
        public List<string> _filters { get; }

        public User(string userName, List<Product> storage, List<string> filters)
        {
            _userName = userName;
            _storage = storage;
            _filters = filters;
        }
    }
}
