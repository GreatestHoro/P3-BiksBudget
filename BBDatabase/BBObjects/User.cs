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
        public List<Shoppinglist> _shoppinglists;
        public List<Product> _storage;

        public User(string userName, string password, List<Shoppinglist> shoppinglists)
        {
            
        }

        public User(string userName, string passWord, List<Product> storage, List<string> filters)
        {
            _userName = userName;
            _password = passWord;
            _storage = storage;
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
