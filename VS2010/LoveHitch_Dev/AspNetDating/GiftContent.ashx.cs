using System;
using System.Web;
using System.Web.Services;
using AspNetDating.Classes;
using AspNetDating.Handlers;


namespace AspNetDating
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "https://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class GiftContent : LoveHitchBaseAsyncHandler
    {

        public override IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, Object extraData)
        {
            ItsAsynchOperation = new AsynchOperation(cb, context, extraData);
            return base.BeginProcessRequest(context, cb, extraData);
        }
        private HttpContext _context;
        public override void EndProcessRequest(IAsyncResult result)
        {
            var context = ItsAsynchOperation.Context;
            base.EndProcessRequest(result);
            int ecardTypeID = 0;

            try
            {
                ecardTypeID = Convert.ToInt32(context.Request.Params["ect"]);
            }
            catch (Exception)
            {
                return;
            }

            GiftType ecardType = GiftType.Fetch(ecardTypeID);

            if (ecardType != null && ecardType.Content != null)// && ecardType.Type != GiftType.eType.Wink)
            {
                string contentType = "image/jpeg";
                //contentType = ecardType.Type == EcardType.eType.Flash 
                //                ? "application/x-shockwave-flash" : "image/jpeg";
                context.Response.Clear();
                context.Response.ContentType = contentType;
                context.Response.BinaryWrite(ecardType.Content);
                context.Response.Flush();
                //context.Response.Close();
            }
        }

        public override bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}
