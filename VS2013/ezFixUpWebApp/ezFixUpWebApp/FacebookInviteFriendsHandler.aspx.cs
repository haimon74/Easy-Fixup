using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ezFixUp.Classes;

namespace ezFixUp
{
    public partial class FacebookInviteFriendsHandler : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if ids exist then redirect to status page, else go to home.aspx
            //ids are comma separated e.g. 43453,34343
            if (Request.Form["ids[]"] != null)
            {
                ((PageBase) Page).StatusPageMessage =
                    "Invitations to your Facebook friends have been sent successfully!".Translate();
                Response.Redirect("ShowStatus.aspx");
            }
            else
            {
                Response.Redirect(MatchmakerHelper.CurrentHomePage);
            }
        }
    }
}
