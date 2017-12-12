using System;
using System.Configuration;
using Funkmap.Middleware;
using Microsoft.Owin.Hosting;
using NLog;
using NLog.Config;

namespace Funkmap.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseAddress = ConfigurationManager.AppSettings["serverAddress"];

            using (WebApp.Start<Startup>(baseAddress))
            {
                System.Console.WriteLine($"Сервер запущен по адерсу {baseAddress}");
                System.Console.ReadLine();
            }
        }
    }
}