using System;
using ezFixUp.Classes;

namespace ezFixUp
{
    public partial class MatchMaker : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            MatchmakerHelper.ToggleState(CurrentUserSession.Username);
            Response.Redirect((string)Global.GetSessionState()["CurrentHomePage"]);
        }
    }
}