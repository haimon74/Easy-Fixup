using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using AspNetDating.Classes;
using AspNetDating.Components.Buttons;

namespace AspNetDating.Components.WebParts
{
    [Themeable(true), Editable]
    public partial class FemaleFriendsWebPart : WebPartUserControl
    {
        public User.eGenderSearch Gender { get { return User.eGenderSearch.Male; } }
        public int MinAge { get { return Config.Users.MinAge; } }
        public int MaxAge { get { return Config.Users.MaxAge; } }
        public int RepeatColumns
        {
            get { return dlFemaleFriends.RepeatColumns; }
            set { dlFemaleFriends.RepeatColumns = value; }
        }

        [Personalizable(PersonalizationScope.User), WebBrowsable(true), CustomWebDisplayNameAttribute("Number of rows")]
        public int LimitRows
        {
            get { return (int)(ViewState["FemaleFriends_LimitRows"] ?? Config.Photos.MaxRowsPhotosOnMainPage); }
            set { ViewState["FemaleFriends_LimitRows"] = value < 1 ? value : 1; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = Lang.Translate("My Single Female Friends");
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            btnContinueFemaleSingles.Visible = dlFemaleFriends.Results != null &&
                            dlFemaleFriends.Results.GetTotalPages(LimitRows*RepeatColumns) > 1;
        }

        protected void dlNewMembers_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            string username = (string)DataBinder.Eval(e.Item.DataItem, "Username");
            ((SendWinkIcon)e.Item.FindControl("btnSendWinkIcon")).Username = username;
            ((SendMsgIcon)e.Item.FindControl("btnSendMsgIcon")).Username = username;
            ((SendGiftIcon)e.Item.FindControl("btnSendGiftIcon")).Username = username;
            ((AddFriendIcon)e.Item.FindControl("btnAddFriendIcon")).Username = username;
            ((AddFavoriteIcon)e.Item.FindControl("btnAddFavoriteIcon")).Username = username;
        }

        protected void dlNewMembers_ItemCreated(object sender, DataListItemEventArgs e)
        {
            if (Config.Users.DisableAgeInformation && Config.Users.DisableGenderInformation)
            {
                HtmlGenericControl pnlGenderAge = (HtmlGenericControl)e.Item.FindControl("pnlGenderAge");
                pnlGenderAge.Visible = false;
            }
            else if (Config.Users.DisableAgeInformation || Config.Users.DisableGenderInformation)
            {
                HtmlGenericControl pnlDelimiter = (HtmlGenericControl)e.Item.FindControl("pnlDelimiter");
                pnlDelimiter.Visible = false;
            }
        }
        protected void btnContinueFemaleSinglesClicked(object sender, EventArgs e)
        {
            Server.Transfer("~/Friends3.aspx?loc=singlefemales");
        }
        protected override void OnUnload(EventArgs e)
        {
            //HttpContext.Current.Response.Flush();
            base.OnUnload(e);
        }
    }
}
