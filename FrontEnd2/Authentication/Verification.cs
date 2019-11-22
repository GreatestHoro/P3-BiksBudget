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
                return Tuple.Create(false, "Størrelsen på dit password er ikke accepteret.");
            }
            else if (password.Contains(" "))
            {
                return Tuple.Create(false, "Undgå mellemrum i dit password.");
            }
            else if (!password.Any(char.IsUpper))
            {
                return Tuple.Create(false, "Pynt dit password med nogle store bogstaver.");
            }
            else if (!password.Any(char.IsLower))
            {
                return Tuple.Create(false, "Brug nogle små bogstaver, småt er godt.");
            }
            else if (new Regex("^[a-zA-Z0-9 ]*$").IsMatch(password))
            {
                return Tuple.Create(false, "Et symbol i passwordet, holder tyven for døren.");
            }
            else if (password != repPassword)
            {
                return Tuple.Create(false, "Der skal to ens password's til at danse tango.");
            }
            else
            {
                return Tuple.Create(true, "Perfekt!");
            }
        }

        public Tuple<bool, string> VerifyUsername(string username)
        {
            if (username.Length < 6 || username.Length > 50)
            {
                return Tuple.Create(false, "Størrelsen på dit username er ikke accepteret.");
            }
            else if (username.Contains(" "))
            {
                return Tuple.Create(false, "Undgå mellemrum i dit username.");
            }
            else if (!new Regex("^[a-zA-Z0-9 ]*$").IsMatch(username))
            {
                return Tuple.Create(false, "Lad os nu ikke være aaaalt for kreative, INGEN symboler.");
            }
            else
            {
                return Tuple.Create(true, "Perfekt!");
            }
        }
    }
}
