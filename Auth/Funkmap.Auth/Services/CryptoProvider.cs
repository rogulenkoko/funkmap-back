using System.Security.Cryptography;
using System.Text;

namespace Funkmap.Auth.Services
{
    public static class CryptoProvider
    {
        public static string ComputeHash(string input)
        {

            var cryptoService = new SHA1CryptoServiceProvider();

            byte[] data = cryptoService.ComputeHash(Encoding.UTF8.GetBytes(input));
            
            StringBuilder sBuilder = new StringBuilder();
            
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }
}
