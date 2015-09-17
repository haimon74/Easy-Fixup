using System;
using AspNetDating.Classes;

namespace AspNetDating.Components.Matchmaker
{
    public partial class MatchToFriend : System.Web.UI.UserControl
    {
        private string _findMatchTranslated = "Find Match to".Translate();

        public bool IsVisible
        {
            set
            {
                SmallBoxStart1.Visible = value;
                MatchToFriendContent1.ContentVisible = value;
                SmallBoxEnd1.Visible = value;
            }
        }

        public string ToUsername
        {
            set
            {
                MatchmakerHelper.MatchToUsername = value;
                SmallBoxStart1.Title = _findMatchTranslated + " " + MatchmakerHelper.MatchToDisplayName;
            }
            //get { return " " + MatchmakerHelper.MatchToUsername ?? "user".Translate(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SmallBoxStart1.Title = _findMatchTranslated + " " +
                (MatchmakerHelper.MatchToDisplayName.Length > 15
                    ? String.Format("{0}..", MatchmakerHelper.MatchToDisplayName.Substring(0, 12))
                    : MatchmakerHelper.MatchToDisplayName);
        }
    }
}