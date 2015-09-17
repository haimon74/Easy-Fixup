
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
    public partial class NewMembers : System.Web.UI.UserControl
    {
        public int GroupID
        {
            get
            {
                if (ViewState["CurrentGroupId"] != null)
                {
                    return (int)ViewState["CurrentGroupId"];
                }
                else
                {
                    throw new Exception("The field groupID is not set!");
                }
            }
            set
            {
                ViewState["CurrentGroupId"] = value;
            }
        }

        public event EventHandler MoreClickEvent;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadStrings();
            }
        }

        //Invoke delegates registered with the MoreClickEvent event.
        protected virtual void OnMoreClick(EventArgs e)
        {
            if (MoreClickEvent != null)
            {
                MoreClickEvent(this, e);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            loadMembers();
        }

        private void loadMembers()
        {
            DataTable dtMembers = new DataTable("Members");

            dtMembers.Columns.Add("GroupID");
            dtMembers.Columns.Add("Username");
            dtMembers.Columns.Add("ImageID", typeof (int));
            dtMembers.Columns.Add("JoinDate", typeof (DateTime));

            GroupMember[] groupMembers = GroupMember.Fetch(GroupID, true, Config.Groups.MaxGroupMembersOnGroupHomePage, GroupMember.eSortColumn.JoinDate);
            
            if (groupMembers.Length > 0)
            {
                int members = GroupMember.Count(GroupID, true);

                if (members > Config.Groups.MaxGroupMembersOnGroupHomePage)
                {
                    pnlMore.Visible = true;
                }

                foreach (GroupMember groupMember in groupMembers)
                {
                    int imageID = 0;
                    try
                    {
                        imageID = Photo.GetPrimary(groupMember.Username).Id;
                    }
                    catch (NotFoundException)
                    {
                        try
                        {
                            User user = User.Load(groupMember.Username);
                            imageID = ImageHandler.GetPhotoIdByGender(user.Gender);
                        }
                        catch (NotFoundException)
                        {
                        }
                    }

                    dtMembers.Rows.Add(new object[]
                                   {
                                       groupMember.GroupID,
                                       groupMember.Username,
                                       imageID,
                                       groupMember.JoinDate
                                   });
                }

                dlGroupMembers.DataSource = dtMembers;
                dlGroupMembers.DataBind();
            }
            else
            {
                lblError.Text = Lang.Trans("There are no members.");
            }
        }

        private void loadStrings()
        {
            LargeBoxStart1.Title = Lang.Trans("Latest Group Members");
            lnkMore.Text = Lang.Trans("View all members");
        }

        protected void lnkMore_Click(object sender, EventArgs e)
        {
            OnMoreClick(e);
        }
    }
}