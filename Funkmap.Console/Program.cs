using System.Configuration;
using Funkmap.Middleware;
using Microsoft.Owin.Hosting;

namespace Funkmap.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseAddress = ConfigurationManager.AppSettings["serverAddress"];

            using (WebApp.Start<Startup>(baseAddress))
            {
                System.Console.WriteLine($"Server has been started on {baseAddress}.");
                System.Console.ReadLine();
            }
        }
    }
}