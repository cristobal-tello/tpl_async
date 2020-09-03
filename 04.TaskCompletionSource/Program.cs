using System;
using System.Linq;

namespace _04.TaskCompletionSource
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "apache.log";

            // First example
            //Console.WriteLine("Using LogProcessor...logic is inside a class. This is not good.");
            //var processor = new LogProcessor(path);

            // Second example
            var bestProcessor = new LogProcessorWithTaskCompletionSource(path);

            foreach (var pair in bestProcessor.IpHits.Result.OrderByDescending(p => p.Value))       // Result forces to wait. So "Main finish Message" will be show to the end
            {
                Console.WriteLine("{0}: {1}", pair.Key, pair.Value);
            }

            Console.WriteLine("Main finishes here!!");
            Console.ReadKey();
        }
    }
}
