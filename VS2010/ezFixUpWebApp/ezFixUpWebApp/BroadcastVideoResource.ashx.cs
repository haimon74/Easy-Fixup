using System;
using System.Reflection;
using System.Web;
using AspNetDating.Plugins.Interfaces;

namespace ezFixUp
{
    public class BroadcastVideoResource : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            IPlugin videobroadcast = null;
            foreach (IPlugin plugin in ezFixUp.Classes.Plugins.Instances)
            {
                if (plugin.Name == "Video Broadcast")
                {
                    videobroadcast = plugin;
                    break;
                }
            }
            if (videobroadcast == null) return;

            string resourceName = String.Format("{0}_{1}", context.Request.Params["res"] ?? "receive",
                context.Request.Params["serv"] ?? "fms");
            Type faceFinderType = videobroadcast.GetType();
            byte[] result = (byte[]) faceFinderType.InvokeMember("GetResource", BindingFlags.Default | BindingFlags.InvokeMethod,
                                                                 null, videobroadcast, new object[] { resourceName });

            context.Response.ContentType = "application/x-shockwave-flash";
            context.Response.AddHeader("content-length", result.Length.ToString());

            context.Response.BinaryWrite(result);

            context.Response.Flush();
            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}
