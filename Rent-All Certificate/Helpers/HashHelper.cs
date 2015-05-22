using System;
using System.Security.Cryptography;
using System.Text;

namespace Helpers
{
    public static class HashHelper
    {
        /// <summary>
        /// Hash an Char-array with SHA512 and concat it with salt. 
        /// </summary>
        public static string Hash(char[] toChar, string salt) {
            var hasher = SHA512.Create();
            var result = hasher.ComputeHash(Encoding.UTF8.GetBytes(string.Concat(new string(toChar), salt)));
            return Convert.ToBase64String(result);
        }

        public static string CreateSalt()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[32];
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);
        }
    }

}
