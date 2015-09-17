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
    public class MatchTo : ezFixUpBaseHandler
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
                    Global.GetSessionState(context.Session.SessionID)["ToUsername"] = username;
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
                Global.GetSessionState(context.Session.SessionID).Remove("ToUsername");
                Global.GetSessionState(context.Session.SessionID).Remove("MatchToFriendImageId");
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
    public class MatchTo : ezFixUpBaseAsyncHandler
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
                Global.GetSessionState()["ToUsername"] = username;
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
