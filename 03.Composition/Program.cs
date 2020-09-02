using System;
using System.Net;
using System.Threading.Tasks;

namespace _03.Composition
{
    class Program
    {
        static void Main(string[] args)
        {
            // First example, we try to get 2 pages with error and use WhenAll
            // We will get an exception and we need to check IsFaulted
            Console.WriteLine("First example...");

            WebClient web1 = new WebClient();
            WebClient web2 = new WebClient();
            
            Task<string> getTask1 = web1.DownloadStringTaskAsync("http://localhost:50323/SlowMissing.ashx");    // 404 error
            Task<string> getTask2 = web2.DownloadStringTaskAsync("http://localhost:50323/adafagasdf.ashx");     // Not existing page

            var gesTasksDone = Task.WhenAll(getTask1, getTask2);

            Console.WriteLine("Results (example 1):");

            gesTasksDone.ContinueWith(t =>
                {
                 if (t.IsFaulted)
                    {
                        Console.WriteLine(t.Exception);     // Note, count is 2
                    }
                    else
                    {
                        Console.WriteLine(t.Result);        // Result is an array of string 
                    }
                }
            );


            // Second example, we try to get 2 pages. First one works, but second fails. Also we use WhenAll
            // We will get an exception and we need to check IsFaulted because second page fail
            // Problem. We cannot get t.Result of first page because t is faulted. Then this way is not best approach
            Console.WriteLine("Second example...");

            WebClient web11 = new WebClient();
            WebClient web21 = new WebClient();

            Task<string> getTask11 = web11.DownloadStringTaskAsync("http://localhost:50323/Slow.ashx");    // It works
            Task<string> getTask21 = web21.DownloadStringTaskAsync("http://localhost:50323/adafagasdf.ashx");     // Not existing page

            var gesTasksDone2 = Task.WhenAll(getTask11, getTask21);

            Console.WriteLine("Results (example 2):");

            gesTasksDone2.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    Console.WriteLine(t.Exception);     // Note, count now is only 1
                    //Console.WriteLine(t.Result);        // This line will crash. I just place this line here to note that we cannot use t.Result
                }
                else
                {
                    Console.WriteLine(t.Result);        // We cannot use t.Result because t.IsFaulted is True 
                }
            }
            );

            // Third example, we need to use ContinueWhenAll, to get a "Partial" success and solve previous problem (on example 2)

            WebClient web13 = new WebClient();
            WebClient web23 = new WebClient();

            Task<string> getTask13 = web13.DownloadStringTaskAsync("http://localhost:50323/Slow.ashx");    // It works
            Task<string> getTask23 = web23.DownloadStringTaskAsync("http://localhost:50323/adafagasdf.ashx");     // Not existing page

            Console.WriteLine("Results (example 3):");

            Task.Factory.ContinueWhenAll(new[] { getTask13, getTask23 },
                (Task<string>[] tasks) =>
                    {
                        foreach (var t in tasks)
                        {
                            if (t.IsFaulted)
                            {
                                Console.WriteLine(t.Exception);
                            }
                            else
                            {
                                Console.WriteLine(t.Result);        // Now, we can use t.Result successfully 
                            }
                        }
                });

            Console.WriteLine("\r\n\r\nMain finish here!!");
            Console.ReadKey();
        }
    }
}
