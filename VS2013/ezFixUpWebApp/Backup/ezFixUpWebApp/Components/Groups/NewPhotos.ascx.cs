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
using ezFixUp.Classes;

namespace ezFixUp.Components.Groups
{
    public partial class NewPhotos : System.Web.UI.UserControl
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

        public event EventHandler MoreClick;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadStrings();
            }
        }

        private void loadStrings()
        {
            LargeBoxStart1.Title = Lang.Trans("Latest Photos");
            lnkMore.Text = Lang.Trans("View all group photos");
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            loadPhotos();
        }

        private void loadPhotos()
        {
            DataTable dtGroupPosts = new DataTable("SearchResults");

            dtGroupPosts.Columns.Add("GroupPhotoID");
            dtGroupPosts.Columns.Add("GroupID");
            dtGroupPosts.Columns.Add("Username");
            dtGroupPosts.Columns.Add("Name");
            dtGroupPosts.Columns.Add("Description");
            dtGroupPosts.Columns.Add("Date");

            GroupPhoto[] groupPhotos =
                GroupPhoto.Fetch(GroupID, Config.Groups.MaxGroupPhotosOnGroupHomePage,
                                 GroupPhoto.eSortColumn.DateUploaded);

            if (groupPhotos.Length > 0)
            {
                int photos = GroupPhoto.Count(GroupID);

                if (photos > Config.Groups.MaxGroupPhotosOnGroupHomePage)
                {
                    pnlMore.Visible = true;
                }

                foreach (GroupPhoto groupPhoto in groupPhotos)
                {
                    dtGroupPosts.Rows.Add(new object[]
                                               {
                                                   groupPhoto.ID,
                                                   groupPhoto.GroupID,
                                                   groupPhoto.Username,
                                                   groupPhoto.Name.Replace('\'', '’'),
                                                   groupPhoto.Description.Replace('\'', '’'),
                                                   groupPhoto.Date.ToShortDateString()
                                                });
                }
            }
            else
            {
                lblError.Text = Lang.Trans("There are no group photos.");
            }

            dlGroupPhotos.DataSource = dtGroupPosts;
            dlGroupPhotos.DataBind();
            dlGroupPhotos.Visible = dtGroupPosts.Rows.Count > 0;

        }

        protected void lnkMore_Click(object sender, EventArgs e)
        {
            OnMoreClick(e);
        }

        protected virtual void OnMoreClick(EventArgs e)
        {
            if (MoreClick != null)
            {
                MoreClick(this, e);
            }
        }
    }
}