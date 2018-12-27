using System.Security.Cryptography;
using System.Text;

namespace Funkmap.Auth.Services
{
    /// <summary>
    /// Hash-provider
    /// </summary>
    public static class CryptoProvider
    {
        /// <summary>
        /// Compute password hash
        /// </summary>
        /// <param name="input">String to hash</param>
        public static string ComputeHash(string input)
        {
            var cryptoService = new SHA1CryptoServiceProvider();

            byte[] data = cryptoService.ComputeHash(Encoding.UTF8.GetBytes(input));
            
            var sBuilder = new StringBuilder();
            
            foreach (var @byte in data)
            {
                sBuilder.Append(@byte.ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }
}
