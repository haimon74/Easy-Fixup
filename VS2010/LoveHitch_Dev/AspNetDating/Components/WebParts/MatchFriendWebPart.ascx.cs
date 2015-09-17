using System;
using AspNetDating.Classes;

namespace AspNetDating.Components.WebParts
{
    public partial class MatchFriendWebPart : WebPartUserControl
    {
        public User FriendForMatching 
        { 
            get { return (User)Session["FriendForMatching"]; } 
            set { Session["FriendForMatching"] = value; }
        }
        public int FriendImageId
        {
            get { return (FriendForMatching != null) ? FriendForMatching.GetPrimaryPhoto().Id : -1; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = Lang.Translate("Match for Friend");
            
        }
    }
}