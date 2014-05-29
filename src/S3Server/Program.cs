using System;
using Microsoft.Owin.Hosting;

namespace S3Server
{
    class Program
    {
        static void Main()
        {
            using (WebApp.Start<Startup>("http://*:12345"))
            {
                Console.WriteLine("Started. Press any key for exit...");
                Console.ReadKey(true);
            }
        }
    }
}
