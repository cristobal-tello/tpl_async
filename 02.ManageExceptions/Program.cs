using System;
using System.Net;
using System.Threading.Tasks;

namespace _02.ManageExceptions
{
    class Program
    {
        static void Main(string[] args)
        {
            WebClient web = new WebClient();
            Task<string> getTask = web.DownloadStringTaskAsync("http://localhost:50323/SlowMissing.ashx");

            Console.WriteLine("Content:");  // You will note now, "Content" is show instantly

            getTask.ContinueWith(t =>
            {
                // First problem
                //Console.WriteLine(t.Result);        // You will get an exception here due a 404
                // The exception will be AggregateException. You need to check the "Count" member, and Innerexception member of AggregateException

                // How to solve it
                // To check if there is a problem when we get the result, we need to check if IsFaulted.So commnent line 24 and run this code
                if (t.IsFaulted)
                {
                    Console.WriteLine("Problem getting data:");
                    Console.WriteLine(t.Exception.Message);
                }
                else
                {
                    Console.WriteLine(t.Result);
                }

                // Extra
                // This is another possible problem. You only will note there is a faulted is you check t.IsFaulted
                // But imagine, that you only show a message. Comment lines from 24 to 36 and just put a messsage
                //Console.WriteLine("Completed!!!");  // The app, not crash due we not use 't'.

                // There is a way to get an exception if we modify the app.config file using ThrowUnobservedTaskExceptions 
                // You need to run it in Release and without debuging Ctrl+F5 
                // Sample here: https://stackoverflow.com/questions/22266495/throwunobservedtaskexceptions-not-working
            });

            Console.WriteLine("\r\n\r\nMain finish here!!");
            Console.ReadKey();

        }
    }
}
