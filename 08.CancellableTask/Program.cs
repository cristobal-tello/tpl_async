using System;
using System.Net;
using System.Threading;

namespace _08.CancellableTask
{
    class Program
    {
        static void Main(string[] args)
        {
            string page = "https://www.20minutos.es";

            using (var webClient = new WebClient())
            {
                using (var cts = new CancellationTokenSource())
                {
                    var task = webClient.DownloadStringTaskAsync(page, cts.Token)
                        .ContinueWith(t =>
                    {
                        if (!t.IsCanceled)
                        {
                            Console.WriteLine("Size get it:" + t.Result.Length);
                        }
                        Console.WriteLine("Status: " + t.Status);
                    });

                    //cts.Cancel();       // Uncomnent this line to simulate a Cancel
                }
            }

            Console.WriteLine("\r\n\r\nMain finish here!!");
            Console.ReadKey();

        }
    }
}
