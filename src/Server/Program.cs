using System;
using Microsoft.Owin.Hosting;

namespace Server
{
    class Program
    {
        static void Main()
        {
            using (WebApp.Start<Startup>("http://localhost:12345"))
            {
                Console.WriteLine("Started. Press any key for exit...");
                Console.ReadKey(true);
            }
        }
    }
}
