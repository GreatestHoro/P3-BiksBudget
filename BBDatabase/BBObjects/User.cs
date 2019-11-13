using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BBCollection.BBObjects
{
    public class User
    {
        public string _userName;
        public string _password;
        public List<Product> _storage;
        public List<string> _filters;
        

        public User(string userName, string passWord, List<Product> storage, List<string> filters)
        {
            _userName = userName;
            _password = passWord;
            _storage = storage;
            _filters = filters;
        }

        public User(string userName, string password)
        {
            _userName = userName;
            _password = password;
        }

        public User()
        {

        }
    }
}
