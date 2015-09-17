using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;

namespace ezFixUp
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "https://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class FlashResource : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string resourceName = context.Request.Params["resname"];
            //if (resourceName != "DetectWebcam")
            //    resourceName = resourceName + "_" +
            //        (ConfigurationManager.AppSettings["FlashServerType"] ?? "fms");

            using (Stream swfFile = Assembly.GetExecutingAssembly().GetManifestResourceStream("ezFixUp." + resourceName + ".swf"))
            {
                context.Response.ContentType = "application/x-shockwave-flash";
                context.Response.AddHeader("content-length", swfFile.Length.ToString());

                byte[] buffer = new byte[swfFile.Length];

                while (swfFile.Read(buffer, 0, buffer.Length) > 0)
                {
                    context.Response.BinaryWrite(buffer);
                }

                context.Response.Flush();
                context.Response.End();
            }

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
