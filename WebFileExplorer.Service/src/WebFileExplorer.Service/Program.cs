using System;

namespace WebFileExplorer.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var app = new ApplicationBootstrapper();
            app.Run();

            Console.WriteLine("Press [ENTER] to stop WebFileExplorer service\n");
            Console.Read();

            Console.WriteLine("Shutting down...");
            app.Stop();
            Console.Read();
        }
    }
}
