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
    public partial class NewTopics : System.Web.UI.UserControl
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

        public event EventHandler ViewAllTopicsClick;

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

            loadNewTopics();
        }

        private void loadNewTopics()
        {
            DataTable dtGroupTopics = new DataTable("SearchResults");

            dtGroupTopics.Columns.Add("Icon");
            dtGroupTopics.Columns.Add("GroupTopicID");
            dtGroupTopics.Columns.Add("GroupTopicName");
            dtGroupTopics.Columns.Add("Posts");
            dtGroupTopics.Columns.Add("Views");
            dtGroupTopics.Columns.Add("DateCreated");
            dtGroupTopics.Columns.Add("Username");
            dtGroupTopics.Columns.Add("ImageID", typeof(int));

            GroupTopic[] groupTopics = GroupTopic.FetchActiveTopics(GroupID, Config.Groups.MaxTopicsOnGroupHomePage);
            
            if (groupTopics.Length > 0)
            {
                int topics = GroupTopic.Count(GroupID);

                if (topics > Config.Groups.MaxTopicsOnGroupHomePage)
                {
                    pnlViewAllTopics.Visible = true;
                }

                string icon = String.Empty;

                foreach (GroupTopic groupTopic in groupTopics)
                {
                    icon = GroupTopic.GetIconString(groupTopic);

                    int imageID = 0;
                    try
                    {
                        imageID = Photo.GetPrimary(groupTopic.Username).Id;
                    }
                    catch (NotFoundException)
                    {
                        try
                        {
                            User user = User.Load(groupTopic.Username);
                            imageID = ImageHandler.GetPhotoIdByGender(user.Gender);
                        }
                        catch (NotFoundException)
                        {
                        }
                    }

                    dtGroupTopics.Rows.Add(new object[]
                                               {
                                                   icon,
                                                   groupTopic.ID,
                                                   Parsers.ProcessGroupTopicName(groupTopic.Name),
                                                   groupTopic.Posts,
                                                   groupTopic.Views,
                                                   groupTopic.DateUpdated.Add(Config.Misc.TimeOffset),
                                                   groupTopic.Username,
                                                   imageID
                                                });
                }

                dgGroupTopics.DataSource = dtGroupTopics;
                dgGroupTopics.DataBind();    
            }
            else
            {
                lblError.Text = Lang.Trans("There are no topics.");
                dgGroupTopics.Visible = false;
                pnlSearchTopic.Visible = false;
            }
            
        }

        private void loadStrings()
        {
            LargeBoxStart1.Title = Lang.Trans("Latest Topics");
            lnkViewAllTopics.Text = Lang.Trans("View all topics");
            btnSearch.Text = Lang.Trans("Search");

            dgGroupTopics.Columns[1].HeaderText = Lang.Trans("Poster");
            dgGroupTopics.Columns[2].HeaderText = Lang.Trans("Topic");
            dgGroupTopics.Columns[3].HeaderText = Lang.Trans("Posts");
            dgGroupTopics.Columns[4].HeaderText = Lang.Trans("Views");
            dgGroupTopics.Columns[5].HeaderText = Lang.Trans("Last Posted");
        }

        protected void lnkViewAllTopics_Click(object sender, EventArgs e)
        {
            OnViewAllTopicsCilck(e);
        }

        protected virtual void OnViewAllTopicsCilck(EventArgs e)
        {
            if (ViewAllTopicsClick != null)
            {
                ViewAllTopicsClick(this, e);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Session["SearchNewTopics"] = txtTopicToSearch.Text;
            Session["SearchNewTopicsInPosts"] = cbSearchInPosts.Checked;

            Response.Redirect(UrlRewrite.CreateShowGroupTopicsUrl(GroupID.ToString()));
        }
    }
}