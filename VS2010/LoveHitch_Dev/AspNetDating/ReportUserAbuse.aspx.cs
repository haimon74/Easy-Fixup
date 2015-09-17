using System;
using AspNetDating.Classes;

namespace AspNetDating
{
    public partial class ReportUserAbuse : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ReportAbuseCtrl.CancelClickEvent += new EventHandler(ReportAbuseCtrl_CancelClickEvent);
            LargeBoxStart1.Title = Lang.Trans("Report Abuse");
        }

        void ReportAbuseCtrl_CancelClickEvent(object sender, EventArgs e)
        {
            Response.Redirect(UrlRewrite.CreateShowUserUrl(((ShowUser)Master).ViewedUser.Username));
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            ReportAbuseCtrl.ReportType = AbuseReport.ReportType.Profile;
            ReportAbuseCtrl.ReportedUser = ((ShowUser)Master).ViewedUser.Username;
            base.OnLoadComplete(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }
    }
}