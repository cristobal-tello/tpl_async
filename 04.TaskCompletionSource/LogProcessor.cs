using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace _04.TaskCompletionSource
{
    class LogProcessor
    {
        private StreamReader _reader;

        private ConcurrentDictionary<string, int> _matches = new ConcurrentDictionary<string, int>();
        private Regex _reg = new Regex("\\b\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\b");   // Very simple way to get ip (not the best, but it will work)
        
    
    public LogProcessor(string path)
        {
            if (path == null)
            {
                throw new ArgumentException("Invalid path");
            }
            _reader = new StreamReader(path);
            FetchNextLine();

        }

        private void FetchNextLine()
        {
            _reader.ReadLineAsync().ContinueWith(ProcessLine);
        }

        private void ProcessLine(Task<string> t)
        {
            string line = t.Result;
            if (line != null)
            {
                Match match = _reg.Match(line);
                if (match.Success)
                {
                    string ip = match.Groups[0].Value;
                    _matches.AddOrUpdate(ip, 1, (k, count) => count + 1);
                }
                FetchNextLine();
            }
            else
            {
                _reader.Close();
                foreach (var pair in _matches.OrderByDescending(p => p.Value))
                {
                    Console.WriteLine("{0}: {1}", pair.Key, pair.Value);
                }
            }
        }
    }
}
