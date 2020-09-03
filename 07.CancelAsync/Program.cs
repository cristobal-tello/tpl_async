using System;
using System.IO;
using System.Threading;

namespace _07.CancelAsync
{
    class Program
    {
        static void Main(string[] args)
        {
            string source = @"C:\Program Files (x86)\Infragistics\2020.1\WPF\Dictionaries\es-spanish-v2-whole.dict";            // Big file
            string target = @"OUTPUT.IN";

            Console.WriteLine("Target:" + target);

            using (var inStream = File.OpenRead(source))
            {
                using (var outStream = File.OpenWrite(target))
                {
                    using (var cts = new CancellationTokenSource())
                    {
                        var task = inStream.CopyToAsync(outStream, 4096, cts.Token);
                        Console.WriteLine("Copying. Press 'c' to Cancel. Another key to continue...");
                        char key = Console.ReadKey().KeyChar;
                        if ('c' == key)
                        {
                            Console.WriteLine("Cancelling...");
                            cts.Cancel();
                        }
                        Console.WriteLine("Waiting...");

                        task.ContinueWith(t =>
                        {
                            // If you Cancel to late maybe the copy is already done.
                            // Then you will get a RanToCompletion status even you typed 'c'
                            Console.WriteLine("Status (on ContinueWith):" + task.Status);
                        });

                        Console.WriteLine("Status:" + task.Status);

                        Console.WriteLine("\r\n\r\nMain finish here!!");
                        Console.ReadKey();

                    }
                }
            }
        }
    }
}
