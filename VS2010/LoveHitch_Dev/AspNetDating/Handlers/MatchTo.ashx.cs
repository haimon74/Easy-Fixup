﻿using System.Web;
using System.Web.Services;
using AspNetDating.Classes;

namespace AspNetDating.Handlers
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "https://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class MatchTo : LoveHitchBaseHandler
    {
        public override void ProcessRequest(HttpContext context)
        {
            base.ProcessRequest(context);
            string imgIdStr = context.Request["imgId"];
            string username = context.Request["username"];
            var currentUser = PageBase.GetCurrentUserSession();
            if (!imgIdStr.IsNullOrEmpty() && !username.Trim().IsNullOrEmpty() && currentUser != null)
            {
                int imgId;
                if (int.TryParse(imgIdStr, out imgId))
                {
                    MatchmakerHelper.MatchToFriendImageId = imgId;
                    MatchmakerHelper.MatchToUsername = username;
                    context.Session["ToUsername"] = username;
                    if (!MatchmakerHelper.IsMatchmakerState)
                    {
                        MatchmakerHelper.ToggleState(currentUser.Username);
                    }
                    return;
                }
            }
            else
            {
                MatchmakerHelper.MatchToUsername = null;
                //context.Session.Remove("MatchToUsername");
                context.Session.Remove("ToUsername");
                context.Session.Remove("MatchToFriendImageId");
            }
            
            //context.Response.Write(String.Format(
            //    "<script type='text/javascript'>window.location.href='{0}'</script>",
            //    context.Request.UrlReferrer.ToString()));
            //context.Response.Write("<script type='text/javascript'>window.location.reload();</script>");
            //context.Response.Flush();
            //context.Response.Redirect(context.Request.UrlReferrer.ToString());
            //context.Server.Transfer(context.Request.UrlReferrer.LocalPath);
        }
        /*
    public class MatchTo : LoveHitchBaseAsyncHandler
    {
        private HttpContext _context;
        public override void EndProcessRequest(IAsyncResult result)
        {
            var context = ItsAsynchOperation.Context;
            base.EndProcessRequest(result);
            string imgIdStr = context.Request["imgId"];
            string username = context.Request["username"];
            var currentUser = PageBase.GetCurrentUserSession();
            if (imgIdStr.IsNullOrEmpty() || username.IsNullOrEmpty() || currentUser == null) return;
            int imgId;
            if (int.TryParse(imgIdStr, out imgId))
            {
                MatchmakerHelper.MatchToFriendImageId = imgId;
                MatchmakerHelper.MatchToUsername = username;
                context.Session["ToUsername"] = username;
                if (!MatchmakerHelper.IsMatchmakerState)
                    MatchmakerHelper. ToggleState(currentUser.Username);
            }
            return;
        }
*/
        public override bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
