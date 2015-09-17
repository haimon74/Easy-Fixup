using System;
using System.Web;
using System.Web.Services;
using AspNetDating.Classes;

namespace AspNetDating
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class EcardContent : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            int ecardTypeID = 0;

            try
            {
                ecardTypeID = Convert.ToInt32(context.Request.Params["ect"]);
            }
            catch (Exception)
            {
                return;
            }

            EcardType ecardType = EcardType.Fetch(ecardTypeID);

            if (ecardType != null && ecardType.Content != null && ecardType.Type != EcardType.eType.Wink)
            {
                string contentType = String.Empty;

                contentType = ecardType.Type == EcardType.eType.Flash 
                                ? "application/x-shockwave-flash" : "image/jpeg";

                context.Response.Clear();
                context.Response.ContentType = contentType;
                context.Response.BinaryWrite(ecardType.Content);
                context.Response.Flush();
                //context.Response.Close();
            }
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
