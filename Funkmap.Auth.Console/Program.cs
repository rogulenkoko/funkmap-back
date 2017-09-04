using System.Configuration;
using Microsoft.Owin.Hosting;

namespace Funkmap.Auth.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseAddress = ConfigurationManager.AppSettings["serverAddress"];

            using (WebApp.Start<Startup>(baseAddress))
            {
                System.Console.WriteLine($"Сервер авторизации запущен по адерсу {baseAddress}");
                System.Console.ReadLine();
            }
        }
    }
}
