using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Web.SessionState;
using ezFixUp.Classes;

namespace ezFixUp
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    public class AffiliateBannerImage : IHttpHandler, IReadOnlySessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            int id = 0;
            try
            {
                id = Convert.ToInt32(context.Request.Params["id"]);
            }
            catch (Exception) // invalid id parameter
            {
                return;
            }

            Image image = null;

            AffiliateBanner affiliateBanner = AffiliateBanner.Fetch(id);

            if (affiliateBanner != null)
            {
                image = affiliateBanner.Image;
            }

            if (image != null)
            {
                image.Save(context.Response.OutputStream, image.RawFormat);
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
