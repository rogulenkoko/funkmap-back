using System;
using Microsoft.Owin.Hosting;

namespace Funkmap
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseAddress = "http://localhost:9000/";
            //string baseAddress = "http://office.scout-gps.ru:11212/";

            using (WebApp.Start<Startup>(baseAddress))
            {
                Console.WriteLine($"Сервер запущен по адерсу {baseAddress}");
                Console.ReadLine();
            }
        }
    }
}
