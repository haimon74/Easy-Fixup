using System;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.SessionState;
using AspNetDating.Classes;

namespace AspNetDating
{
    public class CaptchaHandler : IHttpHandler, IRequiresSessionState
    {
        private Random random = new Random();

        #region IHttpHandler Members

        ///<summary>
        ///Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"></see> interface.
        ///</summary>
        ///
        ///<param name="context">An <see cref="T:System.Web.HttpContext"></see> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests. </param>
        public void ProcessRequest(HttpContext context)
        {
            string randomCode = (string)context.Session["Captcha_RandomCode"];
            if (randomCode.IsNullOrEmpty())
            {
                for (int i = 0; i < 6; i++)
                    randomCode = String.Concat(randomCode, random.Next(10).ToString());
                context.Session["Captcha_RandomCode"] = randomCode;
            }
            CaptchaImage image = new CaptchaImage(randomCode, 160, 40, "Century Schoolbook");
            MemoryStream ms = new MemoryStream();
            image.Image.Save(ms, ImageFormat.Jpeg);

            context.Response.Clear();
            context.Response.ContentType = "image/jpeg";
            ms.WriteTo(context.Response.OutputStream);
        }

        ///<summary>
        ///Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"></see> instance.
        ///</summary>
        ///
        ///<returns>
        ///true if the <see cref="T:System.Web.IHttpHandler"></see> instance is reusable; otherwise, false.
        ///</returns>
        ///
        public bool IsReusable
        {
            get { return false; }
        }

        #endregion
    }
}