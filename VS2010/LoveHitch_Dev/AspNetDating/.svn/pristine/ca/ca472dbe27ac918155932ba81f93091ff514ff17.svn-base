using System;
using AspNetDating.Classes;


namespace AspNetDating.Components.Matchmaker
{
    public partial class MatchToFriendContent : System.Web.UI.UserControl
    {
        public bool ContentVisible{set { pnlContent.Visible = value; }}

        protected void Page_Load(object sender, EventArgs e)
        {
            string toUsername = (string)Session["ToUsername"] ?? String.Empty;
            btnSendWinkIcon.Username = toUsername;
            btnSendGiftIcon.Username = toUsername;
            btnSendMsgIcon.Username = toUsername;
            ibFromFriends.Text = PageBase.MatchmakingFromFriendsTranslated;
            lbFromSearch.Text = PageBase.MatchmakingBySearchTranslated;
            //if (!string.IsNullOrEmpty(toUsername))
            //{
            //    User u = Classes.User.Load(toUsername);
            //    if (u != null && (u.InterestedIn == User.eGender.Female || u.InterestedIn == User.eGender.Male))
            //        ibFromFriends.PostBackUrl = String.Format("~/Single{0}FriendsList.aspx", u.InterestedIn.ToString());
            //    else ibFromFriends.PostBackUrl = "~/FriendsList.aspx";
            //}
            //else ibFromFriends.PostBackUrl = "~/FriendsList.aspx";
        }
    }
}