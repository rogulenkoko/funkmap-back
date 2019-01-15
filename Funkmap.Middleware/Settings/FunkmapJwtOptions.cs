using System.Configuration;

namespace Funkmap.Middleware.Settings
{
    public  static class FunkmapJwtOptions
    {
        public static string Issuer => ConfigurationManager.AppSettings["issuer"];
        public static string Audience => ConfigurationManager.AppSettings["audience"];

        public static string Key => ConfigurationManager.AppSettings["key"];
    }
}
