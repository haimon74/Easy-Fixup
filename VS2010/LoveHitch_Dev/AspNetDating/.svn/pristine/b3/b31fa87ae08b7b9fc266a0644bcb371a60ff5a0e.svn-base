using System;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.SessionState;
using AspNetDating.Classes;

namespace AspNetDating.Handlers
{
    /// <summary>
    /// TipsHandler manages Tips State per User
    /// </summary>
    [WebService(Namespace = "https://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class TipsHandler : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            //base.ProcessRequest(context);
            var referer = context.Request.UrlReferrer;
            string page = referer != null ? referer.Segments[1] : context.Request.Params["page"];
            page = page.ToLower();
            string cmd = context.Request.Params["cmd"];
            UserSession currentUser = PageBase.GetCurrentUserSession();
            
            if (currentUser == null)
                return;

            if (!currentUser.TipsToPageViewDic.Keys.Contains(page))
                return;

            var tip = currentUser.TipsToPageViewDic[page][0];

            if (cmd.IsNullOrEmpty() || tip == null || !tip.ID.HasValue)
                return;

            var tipStatus = currentUser.ProfileTipsStatus.First(c => c.TipId == tip.ID.Value);

            tipStatus.IsBlocked = tipStatus.IsBlocked || (cmd == "disable");
            tipStatus.LastViewedDate = DateTime.Now;
            tipStatus.ViewedTimes++;
            tipStatus.Username = currentUser.Username;
            tipStatus.Save();

            currentUser.TipsToPageViewDic[page].RemoveAt(0);
            if (currentUser.TipsToPageViewDic[page].Count == 0)
                currentUser.TipsToPageViewDic.Remove(page);
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
