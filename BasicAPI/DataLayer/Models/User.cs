using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DataLayer.Models
{
    public class User
    {
        [BsonId]
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string ZipCode { get; set; }
        public string Address { get; set; }
        public string Role { get; set; }

        public string Salt { get; set; }

        public bool HasValidData()
        {
            var output = !string.IsNullOrEmpty(Email) && IsValidEmail() &&
                         !string.IsNullOrEmpty(Password) &&
                         !string.IsNullOrEmpty(PhoneNumber) &&
                         !string.IsNullOrEmpty(ZipCode) &&
                         !string.IsNullOrEmpty(Address);

            return output;
        }

        private bool IsValidEmail()
        {
            const string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(Email);
        }
    }
}
