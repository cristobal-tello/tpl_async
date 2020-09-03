using System;
using System.Net;
using System.Threading.Tasks;

namespace _06.AwaitAndAsync
{
    class Program
    {
        static void Main(string[] args)
        {
            // In these lines we don't use async/await
            //WebClient web = new WebClient();
            //Task<string> getTask = web.DownloadStringTaskAsync("https://www.google.com");

            //Console.WriteLine("Content:");  // You will note now, "Content" is show instantly

            //getTask.ContinueWith(t =>
            //{
            //    Console.WriteLine(t.Result);
            //});


            DoTheSameUsingAwaitAndAsncAsync().Wait();

            Console.WriteLine("\r\n\r\nMain finish here!!");
            Console.ReadKey();
        }

        private static async Task DoTheSameUsingAwaitAndAsncAsync()
        {
            //// Using await is like ContinueWith
            //WebClient web = new WebClient();
            //var s = await web.DownloadStringTaskAsync("https://www.google.com");
            //Console.WriteLine("On DoTheSameUsingAwaitAndAsncAsync:");
            //Console.WriteLine("Result:");
            //Console.WriteLine(s);

            // Another way
            WebClient web = new WebClient();
            var t = web.DownloadStringTaskAsync("https://www.google.com");
            Console.WriteLine("On DoTheSameUsingAwaitAndAsncAsync:");
            Console.WriteLine("Result:");
            var s = await t;
            Console.WriteLine(s);

        }
    }
}
