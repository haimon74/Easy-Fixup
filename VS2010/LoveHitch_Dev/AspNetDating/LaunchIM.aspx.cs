using System;
using AspNetDating.Classes;

namespace AspNetDating
{
    public partial class LaunchIM : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string targetUsername = Request.Params["targetUsername"];
            //string targetAge = Request.Params["targetAge"];
            //string targetSex = Request.Params["targetSex"];
            //string targetLocation = Request.Params["targetLocation"];

            if (targetUsername == null || CurrentUserSession == null)
            {
                RedirectToLoginPage();
                return;
            }

            try
            {
                string url =
                    String.Format("~/AjaxChat/MessengerWindow.aspx?init=1&target={0}",ViewedUser.Username);

                bool isSectionUnlocked =
                    UnlockedSection.IsSectionUnlocked(CurrentUserSession.Username, ViewedUser.Username, UnlockedSection.SectionType.IM, null);

                var permissionCheckResult = CurrentUserSession.CanIM();

                if (permissionCheckResult == PermissionCheckResult.No)
                {
                    RedirectToLoginPage();
                    return;
                }

                if (permissionCheckResult == PermissionCheckResult.Yes || isSectionUnlocked)
                {
                    Response.Redirect(url);
                    return;
                }

                if (permissionCheckResult == PermissionCheckResult.YesButPlanUpgradeNeeded ||
                    permissionCheckResult == PermissionCheckResult.YesButMoreCreditsNeeded)
                {
                    Session["BillingPlanOption"] = CurrentUserSession.BillingPlanOptions.CanIM;
                    Response.Clear();

                    Response.Write("<script type=\"text/javascript\">");
                    Response.Write(String.Format("window.opener.window.location.href = '{0}/ManageProfile.aspx?sel=payment';", Config.Urls.Home));
                    Response.Write("window.close();");
                    Response.Write("</script>");

                    Response.End();
                    return;
                }

                if (permissionCheckResult == PermissionCheckResult.YesWithCredits)
                {
                    int credits = CurrentUserSession.BillingPlanOptions.CanIM.Credits;

                    CurrentUserSession.Credits -= credits;
                    CurrentUserSession.Update(true);

                    CreditsHistory creditsHistory = new CreditsHistory(CurrentUserSession.Username);
                    creditsHistory.Amount = credits;
                    creditsHistory.Service = CreditsHistory.eService.UseIM;
                    creditsHistory.Save();

                    UnlockedSection.UnlockSection(CurrentUserSession.Username, ViewedUser.Username, UnlockedSection.SectionType.IM,
                                                  null, DateTime.Now.AddDays(Config.Credits.IMUnlockPeriod));

                    Response.Redirect(url);
                    return;
                }

                //if (IMLocked)
                //{
                //    int credits = CurrentUserSession.BillingPlanOptions.CanIM.Credits;

                //    if (CurrentUserSession.Credits - credits < 0)
                //    {
                //        //lblError.Text = Lang.Trans("You don't have enough credits to start new chat session");
                //        Response.Clear();

                //        Response.Write("<script type=\"text/javascript\">");
                //        Response.Write(String.Format("window.opener.window.location.href = '{0}/ManageProfile.aspx?sel=payment';", Config.Urls.Home));
                //        Response.Write("window.close();");
                //        Response.Write("</script>");

                //        Response.End();
                //        return;
                //    }

                //    CurrentUserSession.Credits -= credits;
                //    CurrentUserSession.Update(true);

                //    CreditsHistory creditsHistory = new CreditsHistory(CurrentUserSession.Username);
                //    creditsHistory.Amount = credits;
                //    creditsHistory.Service = CreditsHistory.eService.UseIM;
                //    creditsHistory.Save();

                //    UnlockedSection.UnlockSection(CurrentUserSession.Username, ViewedUser.Username, UnlockedSection.SectionType.IM,
                //                                  null, DateTime.Now.AddDays(Config.Credits.IMUnlockPeriod));

                //    Response.Redirect(url);
                //}
                //else
                //{
                //    Response.Redirect(url);
                //}
            }
            catch (NotFoundException) {
                return; }
        }

        private void RedirectToLoginPage()
        {
            Response.Clear();

            Response.Write("<script type=\"text/javascript\">");
            Response.Write(String.Format("window.opener.window.location.href = '{0}/Login.aspx';", Config.Urls.Home));
            Response.Write("window.close();");
            Response.Write("</script>");

            Response.End();
            return;
        }

        private User viewedUser;
        private User ViewedUser
        {
            get
            {
                if (viewedUser == null
                    && !string.IsNullOrEmpty(Request.Params["targetUsername"]))
                    viewedUser = Classes.User.Load(Request.Params["targetUsername"]);
                return viewedUser;
            }
        }

        //private bool IMLocked
        //{
        //    get
        //    {
        //        return Config.Credits.Required && Config.Credits.CreditsForIM > 0 &&
        //            (CurrentUserSession == null ||
        //            (CurrentUserSession.Username != ViewedUser.Username &&
        //             !(Config.Users.FreeForFemales && CurrentUserSession.Gender == Classes.User.eGender.Female) &&
        //             !UnlockedSection.IsSectionUnlocked(CurrentUserSession.Username, ViewedUser.Username, UnlockedSection.SectionType.IM, null)));
        //    }
        //}
    }
}
