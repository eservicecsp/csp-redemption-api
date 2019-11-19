using Konscious.Security.Cryptography;
using System;
using System.Security.Cryptography;
using System.Text;

namespace CSP_Redemption_WebApi.Helpers
{
    public class Argon2Helper
    {
        public static string HashPassword(string username, string password)
        {
            var argon2 = new Argon2i(Encoding.UTF8.GetBytes(password));

            argon2.Salt = Encoding.UTF8.GetBytes(username);
            argon2.DegreeOfParallelism = 1;
            argon2.Iterations = 2;
            argon2.MemorySize = 16; //kbytes

            var hash = argon2.GetBytes(16);
            return Convert.ToBase64String(hash);
        }

        private static byte[] CreateSalt()
        {
            var buffer = new byte[16];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(buffer);
            return buffer;
        }
    }
}
