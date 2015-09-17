using System;
using System.Web;
using System.Web.Services;
using AspNetDating.Classes;
using System.Collections.Generic;

namespace AspNetDating.Handlers
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "https://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class PostActionInvitaionCheck : LoveHitchBaseHandler
    {
        public override void ProcessRequest(HttpContext context)
        {
            base.ProcessRequest(context);
            string username1 = context.Request.Params["un1"];
            string username2 = context.Request.Params["un2"];
            List<string> lstInviteUsernames = new List<string>();

            if (username1.IsNotNullOrEmpty())
            {
                var result = InviteIfUserNotRegistered(context, username1);
                if (result.IsNotNullOrEmpty()) lstInviteUsernames.Add(result);
            }

            if (username2.IsNotNullOrEmpty())
            {
                var result = InviteIfUserNotRegistered(context, username2);
                if (result.IsNotNullOrEmpty()) lstInviteUsernames.Add(result);
            }
            context.Response.Write(string.Join(",", lstInviteUsernames.ToArray()));
            context.Response.Flush();
            context.Response.End();
        }
        private string InviteIfUserNotRegistered(HttpContext context, string username)
        {
            User u = User.Load(username);
            if (u != null && u.LoginCount == 0)
            {
                PageBase.SetSessionStatusPageMessage("Some of Your Friends Are Not Registered. <br/>Use the Following  Page to Invite Them.".Translate());
                PageBase.SetIsToRedirectToLastViewedUsername(false);
                return username;
            }
            else return String.Empty;
        }
        /*
    public class MatchWith : LoveHitchBaseAsyncHandler
    {
        private HttpContext _context;
        public override void EndProcessRequest(IAsyncResult result)
        {
            var context = ItsAsynchOperation.Context;
            base.EndProcessRequest(result);
            string username = context.Request.Params["with_user"];
            if (!MatchmakerHelper.IsMatchmakerState)
            {
                MatchmakerHelper.ToggleState();
            }
            if (username.IsNullOrEmpty())
            {
                //context.Response.Redirect("~/Home_mm.aspx");
                context.Response.End();
            }
            if (MatchmakerHelper.IsMatchToFriendAlredySelected)
            {
                string toUsername = MatchmakerHelper.MatchToUsername;
                MatchmakerHelper.CreateMatchmakingWithUsername(username);
                //context.Response.Redirect("~/Home_mm.aspx");
                context.Response.Write(toUsername);
            }
            else
            {
                MatchmakerHelper.MatchToFriendImageId = User.Load(username).GetPrimaryPhoto().Id;
                MatchmakerHelper.MatchToUsername = username;
                //context.Response.Redirect("~/Search2.aspx");
                context.Response.Write("false");
            }
            
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
