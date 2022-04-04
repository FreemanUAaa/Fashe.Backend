using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Fashe.Users.Core.Helpers.Passwords
{
    public static class PasswordHasher
    {
        public static int SaltSize => 128 / 8;

        public static int IterationCount => 10;

        public static int NumBytesRequested => 256 / 8;

        public static string Hash(string password, byte[] salt)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password, salt: salt, prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: IterationCount, numBytesRequested: NumBytesRequested));

            return hashed;
        }

        public static byte[] GetNewSalt()
        {
            byte[] salt = new byte[SaltSize];
            using RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();
            rngCsp.GetNonZeroBytes(salt);
            return salt;
        }
    }
}
