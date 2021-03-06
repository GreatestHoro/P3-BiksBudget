﻿using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace FrontEnd2.Authentication
{
    public class Verification
    {
        public Tuple<bool, string> VerifyPassword(string password)
        {
            if (String.IsNullOrEmpty(password) || String.IsNullOrWhiteSpace(password))
            {
                return Tuple.Create(false, "Please use between 6 and 50 characters.");
            }
            else if (password.Length < 6 || password.Length > 50)
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
            else
            {
                return Tuple.Create(true, "Perfect!");
            }
        }

        public Tuple<bool, string> VerifyRepPassword(string password, string repPassword)
        {
            Tuple<bool, string> checkFirstPass = VerifyPassword(password);

            if (checkFirstPass.Item1 == false)
            {
                return Tuple.Create(false, "The first password is invalid");
            }
            else if (String.IsNullOrEmpty(repPassword) || String.IsNullOrWhiteSpace(repPassword))
            {
                return Tuple.Create(false, "Please use between 6 and 50 characters.");
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

        public Tuple<bool, string> VerifyUsername(string username, string verifiedUsername)
        {
            if (String.IsNullOrEmpty(username) || String.IsNullOrWhiteSpace(username))
            {
                return Tuple.Create(false, "Please write a username");
            }
            else if (username.Length < 6 || username.Length > 50)
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
            else if (username.Equals(verifiedUsername))
            {
                return Tuple.Create(false, "This Username is already being used");
            }
            else
            {
                return Tuple.Create(true, "Perfekt!");
            }
        }
    }
}
