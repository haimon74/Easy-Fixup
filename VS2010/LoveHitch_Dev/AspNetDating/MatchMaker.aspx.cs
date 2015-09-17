using System;
using AspNetDating.Classes;

namespace AspNetDating
{
    public partial class MatchMaker : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            MatchmakerHelper.ToggleState(CurrentUserSession.Username);
            Response.Redirect((string)Session["CurrentHomePage"]);
        }
    }
}