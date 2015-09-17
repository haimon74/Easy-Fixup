using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using AspNetDating.Classes;

namespace AspNetDating.Components.Groups
{
    public partial class GroupBans : System.Web.UI.UserControl
    {
        public int GroupID
        {
            get
            {
                return (int)ViewState["CurrentGroupId"];
            }
            set
            {
                ViewState["CurrentGroupId"] = value;
            }
        }

        protected Group CurrentGroup
        {
            get
            {
                if (Page is ShowGroup)
                {
                    return ((ShowGroup)Page).Group;
                }
                else
                {
                    return Group.Fetch(GroupID);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadStrings();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            loadGroupBans();
        }

        private void loadStrings()
        {
            dgGroupBans.Columns[0].HeaderText = Lang.Trans("Username");
            dgGroupBans.Columns[1].HeaderText = Lang.Trans("Expiration date");
            dgGroupBans.Columns[2].HeaderText = "";

            LargeBoxStart1.Title = Lang.Trans("Banned Group Members");
        }

        private void loadGroupBans()
        {
            DataTable dtGroupBans = new DataTable("GroupBans");

            dtGroupBans.Columns.Add("ID");
            dtGroupBans.Columns.Add("Username");
            dtGroupBans.Columns.Add("ExpirationDate");

            GroupBan[] groupBans = GroupBan.FetchByGroupID(CurrentGroup.ID);

            foreach (GroupBan groupBan in groupBans)
            {
                if (groupBan.Expires >= DateTime.Now)
                {
                    dtGroupBans.Rows.Add(new object[]
                                         {
                                             groupBan.ID,
                                             groupBan.Username,
                                             groupBan.Expires.Add(Config.Misc.TimeOffset).ToShortDateString()
                                         });
                }
            }

            dgGroupBans.DataSource = dtGroupBans;
            dgGroupBans.DataBind();

            if (dtGroupBans.Rows.Count == 0)
            {
                lblError.Text = Lang.Trans("There are no banned members in this group.");
                dgGroupBans.Visible = false;
            }
        }

        protected void dgGroupBans_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Unban")
            {
                GroupBan.Delete(Convert.ToInt32(e.CommandArgument));
            }
        }
    }
}