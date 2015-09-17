using System;
using AspNetDating.Classes;

namespace AspNetDating.Components
{
    public partial class MutualFriendModalPopup : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //LoadStrings();
            }
        }

        private void LoadStrings()
        {
            lbModalBlock.ToolTip = Lang.Trans("Block user");
            lbModalFavorite.ToolTip = Lang.Trans("Add to favorite list");
            lbModalRemoveFavorite.ToolTip = Lang.Trans("Remove from favorite list");
            lbModalInviteFriend.ToolTip = Lang.Trans("Friendship request");
            lbModalAcceptFriend.ToolTip = Lang.Trans("Accept friendship request");
            lbModalDenyFriend.ToolTip = Lang.Trans("Reject friendship request");
            lbModalRemoveFriend.ToolTip = Lang.Trans("Remove from friends list");
            lbModalSendMsg.ToolTip = Lang.Trans("Send message");
            lbModalSendGift.ToolTip = Lang.Trans("Send gift");
            lbModalWink.ToolTip = Lang.Trans("Send wink");
            lbModalMF1_AskAbout.ToolTip = "Ask about".Translate();
            lbModalMF2_AskAbout.ToolTip = "Ask about".Translate();
            lbModalMF3_AskAbout.ToolTip = "Ask about".Translate();
            lbModalMF1_MatchReq.ToolTip = "Match request".Translate();
            lbModalMF2_MatchReq.ToolTip = "Match request".Translate();
            lbModalMF3_MatchReq.ToolTip = "Match request".Translate();
            if (MatchmakerHelper.IsMatchmakerState)
            {
                if (MatchmakerHelper.IsMatchToFriendAlredySelected)
                {
                    lbModalMatchRequest.ToolTip = "Match with".Translate();
                }
                else
                {
                    //lbModalMatchRequest.ToolTip = "Redirect to select friend to match at the matchmaker main page".Translate();
                    lbModalMatchRequest.ToolTip = "Find match to him/her".Translate();
                }
            }
            else
            {
                lbModalMatchRequest.ToolTip = "Enter Matchmaker state".Translate();
            }
            bool isFriendsPage = (Page is Friends3);
            lbModalAcceptFriend.Visible = isFriendsPage;
            lbModalDenyFriend.Visible = isFriendsPage;
            //lbModalDenyFriend.Visible = isFriendsPage;
        }
    }
}