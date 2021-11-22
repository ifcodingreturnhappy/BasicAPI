using DataLayer.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace API.Services
{
    public class HMACHashingService : IPasswordHasherService
    {
        public async Task<HashedPassword> HashPassword(string password, byte[] salt = null)
        {
            return await Task.Run(() =>
            {
                if(salt == null)
                {
                    // generate a 128-bit salt using a cryptographically strong random sequence of nonzero values
                    salt = new byte[128 / 8];
                    using (var rngCsp = new RNGCryptoServiceProvider())
                    {
                        rngCsp.GetNonZeroBytes(salt);
                    }
                }

                // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8));

                return new HashedPassword
                {
                    Hash = hashed,
                    Salt = HashedPassword.SaltToString(salt)
                };
            });
        }
    }
}
