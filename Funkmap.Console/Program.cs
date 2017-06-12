using Microsoft.Owin.Hosting;

namespace Funkmap.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseAddress = "http://localhost:9000/";
            //string baseAddress = "http://office.scout-gps.ru:11212/";

            using (WebApp.Start<Startup>(baseAddress))
            {
                System.Console.WriteLine($"Сервер запущен по адерсу {baseAddress}");
                System.Console.ReadLine();
            }
        }
    }
}
