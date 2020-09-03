using System;
using System.Linq;

namespace _05.TaskCompletionReportingError
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // To force a exception reading the file, set a breakpoint in LogProcessorWithTaskCompletionSource.ProcessLine, line 64
                // Then open Computer Management (Admin tools) and inside Open Files folder, force to close it
                // Note: log file must in another machine in a shared folder

                string path = "\\\\VBOXSVR\\Shared\\apache.log";

                var bestProcessor = new LogProcessorWithTaskCompletionSource(path);

                foreach (var pair in bestProcessor.IpHits.Result.OrderByDescending(p => p.Value))       // Result forces to wait. So "Main finish Message" will be show to the end
                {
                    Console.WriteLine("{0}: {1}", pair.Key, pair.Value);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception:");
                Console.WriteLine(ex);
            }

            Console.WriteLine("Main finishes here!!");
            Console.ReadKey();
        }
    }
}
