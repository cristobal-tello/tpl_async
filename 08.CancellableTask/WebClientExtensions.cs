using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace _08.CancellableTask
{
    public static class WebClientExtensions
    {
        public static Task<string> DownloadStringTaskAsync(this WebClient web, string address, CancellationToken ct)
        {
            var tcs = new TaskCompletionSource<string>();
            CancellationTokenRegistration ctr = default(CancellationTokenRegistration);
            
            DownloadStringCompletedEventHandler h = null;

            h = (s, e) =>
            {
                web.DownloadStringCompleted -= h;
                ctr.Dispose();

                if (e.Cancelled)
                {
                    tcs.SetCanceled();
                }
                else if (e.Error != null)
                {
                    tcs.SetException(e.Error);
                }
                else
                {
                    tcs.SetResult(e.Result);
                }
            };

            if (ct.IsCancellationRequested)
            {
                tcs.SetCanceled();
            }
            else
            {
                web.DownloadStringCompleted += h;
                ctr = ct.Register(() => web.CancelAsync());
                web.DownloadStringAsync(new Uri(address));
                if (ct.IsCancellationRequested)
                {
                    web.CancelAsync();
                }
            }
            return tcs.Task;
        }
    }
}
