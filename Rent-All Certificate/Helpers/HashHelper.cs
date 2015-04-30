using System;
using System.Security.Cryptography;
using System.Text;

namespace Helpers
{
    public class HashHelper
    {
        /// <summary>
        /// Hash an Char-array with SHA512 and concat it with salt. 
        /// </summary>
        public string Hash(char[] toChar, string salt) {
            var hasher = SHA512.Create();
            var result = hasher.ComputeHash(Encoding.UTF8.GetBytes(string.Concat(new string(toChar), salt)));
            return Convert.ToBase64String(result);
        }
    }
}
