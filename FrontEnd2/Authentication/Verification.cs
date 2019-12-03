using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Reflection;

namespace FrontEnd2.Authentication
{
    public class Verification
    {
        public Tuple<bool, string> VerifyPassword(string password, string repPassword)
        {
            if (password.Length < 6 || password.Length > 50)
            {
                return Tuple.Create(false, "Please use between 6 and 50 characters.");
            }
            else if (password.Contains(" "))
            {
                return Tuple.Create(false, "Please refrain from using a 'space'.");
            }
            else if (!password.Any(char.IsUpper))
            {
                return Tuple.Create(false, "Please use an upper case letter.");
            }
            else if (!password.Any(char.IsLower))
            {
                return Tuple.Create(false, "Please use an lower case letter.");
            }
            else if (new Regex("^[a-zA-Z0-9 ]*$").IsMatch(password))
            {
                return Tuple.Create(false, "Please use at least one symbol.");
            }
            else if (password != repPassword)
            {
                return Tuple.Create(false, "The passwords are not the same.");
            }
            else
            {
                return Tuple.Create(true, "Perfect!");
            }
        }

        public Tuple<bool, string> VerifyUsername(string username)
        {
            if (username.Length < 6 || username.Length > 50)
            {
                return Tuple.Create(false, "Please use between 6 and 50 characters.");
            }
            else if (username.Contains(" "))
            {
                return Tuple.Create(false, "Please refrain from using a 'space'.");
            }
            else if (!new Regex("^[a-zA-Z0-9 ]*$").IsMatch(username))
            {
                return Tuple.Create(false, "Please refrain from using symbols.");
            }
            else
            {
                return Tuple.Create(true, "Perfekt!");
            }
        }
    }
}
