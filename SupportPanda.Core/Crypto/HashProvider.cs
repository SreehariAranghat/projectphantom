using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SupportPanda.Core
{
    public class HashProvider
    {
        public static string CreateHash(string input)
        {
            SHA512CryptoServiceProvider c = new SHA512CryptoServiceProvider();
            c.ComputeHash(Encoding.UTF8.GetBytes(input));

            string hashStr = Convert.ToBase64String(c.Hash);
            return hashStr;
        }
    }
}
