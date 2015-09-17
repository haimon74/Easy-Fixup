using System;
using System.Web;
using System.Web.Services;
using ezFixUp.Classes;

namespace ezFixUp.Handlers
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "https://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

    public class MatchWith : ezFixUpBaseHandler
    {
        public override void ProcessRequest(HttpContext context)
        {
            base.ProcessRequest(context);
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
        /*
    public class MatchWith : ezFixUpBaseAsyncHandler
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
