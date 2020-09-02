using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace _01.UsingTaskBasedApi
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("No tpl used. Sync method...\r\n\r\n");

            // This is the starting point. No tpl is used. DownloadString method is a sync method
            var web1 = new WebClient();
            string result = web1.DownloadString("http://localhost:50323/Slow.ashx");
            Console.WriteLine("Content:");
            Console.WriteLine(result);
            Console.WriteLine("\r\nWe will finish here!!");

            Console.WriteLine("\r\n\r\nUsing tpl, example 1...\r\n\r\n");

            // Using tpl. Program is not block when getTask is executed.It will block on getTask.Result
            WebClient web2 = new WebClient();
            Task<string> getTask = Task<string>.Factory.StartNew(() =>
                {
                    string result2 = web2.DownloadString("http://localhost:50323/Slow.ashx");
                    return result2;
                }
            );

            Console.WriteLine("Content:");  // You will note now, "Content" is show instantly
            Console.WriteLine(getTask.Result);
            Console.WriteLine("\r\n\r\nWe will finish here!!");

            /// Using tpl, but not blocking main thread, tpl will let you know when task is done
            //Console.WriteLine("\r\n\r\nUsing tpl, example 2...\r\n\r\n");

            //WebClient web3 = new WebClient();
            //Task<string> getTask3 = Task<string>.Factory.StartNew(() =>
            //    {
            //        string result3 = web3.DownloadString("http://localhost:50323/Slow.ashx");
            //        return result3;
            //    }
            //);

            //Console.WriteLine("Content:");  // You will note now, "Content" is show instantly

            //getTask3.ContinueWith(t =>
            //    {
            //        Console.WriteLine(t.Result);
            //    });

            //Console.WriteLine("\r\n\r\nWe will finish here!!"); // Note that this line also will show before t.Result (line 55)

            /// Using tpl and downloadasync in webclient. This is the best
            Console.WriteLine("\r\n\r\nUsing tpl and DownloadStringTaskAsync , example 3...\r\n\r\n");

            WebClient web4 = new WebClient();
            Task<string> getTask4 = web4.DownloadStringTaskAsync("http://localhost:50323/Slow.ashx");

            Console.WriteLine("Content:");  // You will note now, "Content" is show instantly

            getTask4.ContinueWith(t =>
            {
                Console.WriteLine(t.Result);
            });

            Console.WriteLine("\r\n\r\nMain finish here!!"); // Note that this line also will show before t.Result (line 68)
            Console.ReadKey();

        }
    }
}
