using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels.Authentication
{
    public class HashedPassword
    {
        public string Hash { get; set; }
        public string Salt { get; set; }

        public static string SaltToString(byte[] salt) => Convert.ToBase64String(salt);
        public static byte[] SaltToByteArray(string salt) => Convert.FromBase64String(salt);
    }
}
