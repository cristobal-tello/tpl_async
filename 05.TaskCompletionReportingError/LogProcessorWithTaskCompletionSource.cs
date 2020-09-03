using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace _05.TaskCompletionReportingError
{
    class LogProcessorWithTaskCompletionSource
    {
        private StreamReader _reader;

        private ConcurrentDictionary<string, int> _matches = new ConcurrentDictionary<string, int>();
        private Regex _reg = new Regex("\\b\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\b");   // Very simple way to get ip (not the best, but it will work)

        private TaskCompletionSource<IDictionary<string, int>> _ipCompletionSource = new TaskCompletionSource<IDictionary<string, int>>();

        public Task<IDictionary<string, int>> IpHits
        {
            get
            {
                return _ipCompletionSource.Task;
            }
        }

        public LogProcessorWithTaskCompletionSource(string path)
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
            //if (t.IsFaulted)          // First test, with this code commented. Later, uncomnent and check again (Note. You must force an error, getting the file)
            //{
            //    _reader.Close();
            //    _ipCompletionSource.SetException(t.Exception.InnerExceptions);
            //}
            //else
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
                    _ipCompletionSource.SetResult(_matches);
                }
            }
        }
    }
}
