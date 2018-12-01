using System.Configuration;

namespace Funkmap.Auth.Web
{
    public  static class FunkmapJwtOptions
    {
        public static string Issuer { get; } = ConfigurationManager.AppSettings["issuer"];
        public static string Audience { get; } = ConfigurationManager.AppSettings["audience"];

        public static string Key { get; } = ConfigurationManager.AppSettings["key"];
    }
}
