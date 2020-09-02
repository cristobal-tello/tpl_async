using System.Threading;
using System.Web;

namespace _01.SlowWeb
{
    /// <summary>
    /// Summary description for Handler1
    /// </summary>
    public class SlowHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            Thread.Sleep(5000);
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
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