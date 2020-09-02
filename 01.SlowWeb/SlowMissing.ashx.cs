using System.Threading;
using System.Web;

namespace _01.SlowWeb
{
    /// <summary>
    /// Summary description for Handler1
    /// </summary>
    public class SlowMissingHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            Thread.Sleep(5000);
            context.Response.StatusCode = 404;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}