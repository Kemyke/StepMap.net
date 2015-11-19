using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StepMap.Common
{
    public static class CryptoHelper
    {
        public static string CreatePasswordHash(string password)
        {
            SHA256 sha256 = SHA256Managed.Create(); 
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            string result = Convert.ToBase64String(bytes);
            return result;
        }
    }
}
