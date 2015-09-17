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
    public partial class SearchTopicResults : System.Web.UI.UserControl
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

        public event SearchTopicClickEventHandler SearchTopicClick;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                loadStrings();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            loadResultsPage();

            if (PaginatorEnabled)
            {
                preparePaginator();
            }

            base.OnPreRender(e);
        }

        private void loadStrings()
        {
            LargeBoxStart1.Title = Lang.Trans("Results");
            btnSearch.Text = Lang.Trans("Search");

            dgGroupTopics.Columns[1].HeaderText = Lang.Trans("Poster");
            dgGroupTopics.Columns[2].HeaderText = Lang.Trans("Topic");
            dgGroupTopics.Columns[3].HeaderText = Lang.Trans("Posts");
            dgGroupTopics.Columns[4].HeaderText = Lang.Trans("Last Posted");

            lnkFirst.Text = Lang.Trans("[ First ]");
            lnkPrev.Text = Lang.Trans("[ Prev ]");
            lnkNext.Text = Lang.Trans("[ Next ]");
            lnkLast.Text = Lang.Trans("[ Last ]");
        }

        public GroupTopicSearchResults Results
        {
            set
            {
                if (ViewState["SearchResults_guid"] == null)
                {
                    ViewState["SearchResults_guid"] = Guid.NewGuid().ToString();
                }

                if (value != null && value.GroupTopics.Length == 0)
                    value = null;

                Session["SearchResults" + ViewState["SearchResults_guid"]] = value;

                CurrentPage = 1;
            }
            get
            {
                if (ViewState["SearchResults_guid"] != null)
                {
                    return (GroupTopicSearchResults)
                           Session["SearchResults" + ViewState["SearchResults_guid"]];
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets or sets the current page.
        /// </summary>
        /// <value>The current page.</value>
        public int CurrentPage
        {
            set
            {
                Trace.Write("SearchResults.ascx.cs", "CurrentPage = " + value);
                ViewState["CurrentPage"] = value;
                preparePaginator();
            }
            get
            {
                if (ViewState["CurrentPage"] == null
                    || (int)ViewState["CurrentPage"] == 0)
                {
                    return 1;
                }
                else
                {
                    return (int)ViewState["CurrentPage"];
                }
            }
        }

        /// <summary>
        /// Sets a value indicating whether [paginator enabled].
        /// </summary>
        /// <value><c>true</c> if [paginator enabled]; otherwise, <c>false</c>.</value>
        private bool paginatorVisible = true;

        public bool PaginatorEnabled
        {
            get { return paginatorVisible; }
            set
            {
                paginatorVisible = value;
                pnlPaginator.Visible = value;
            }
        }

        /// <summary>
        /// Prepares the paginator.
        /// </summary>
        private void preparePaginator()
        {
            if (Results == null || CurrentPage <= 1)
            {
                lnkFirst.Enabled = false;
                lnkPrev.Enabled = false;
            }
            else
            {
                lnkFirst.Enabled = true;
                lnkPrev.Enabled = true;
            }
            if (Results == null || CurrentPage >= Results.GetTotalPages(Config.Groups.GroupTopicsPerPage))
            {
                lnkLast.Enabled = false;
                lnkNext.Enabled = false;
            }
            else
            {
                lnkLast.Enabled = true;
                lnkNext.Enabled = true;
            }
            if (Results != null && Results.GroupTopics.Length > 0)
            {
                int fromGroup = (CurrentPage - 1) * Config.Groups.GroupTopicsPerPage + 1;
                int toGroup = CurrentPage * Config.Groups.GroupTopicsPerPage;
                if (Results.GroupTopics.Length < toGroup)
                    toGroup = Results.GroupTopics.Length;

                lblPager.Text = String.Format(
                    Lang.Trans("Showing {0}-{1} from {2} total"),
                    fromGroup, toGroup, Results.GroupTopics.Length);
            }
            else
            {
                lblPager.Text = Lang.Trans("No Results");
            }
        }

        private void loadResultsPage()
        {
            DataTable dtGroupTopics = new DataTable("SearchResults");
            dtGroupTopics.Columns.Add("Icon");
            dtGroupTopics.Columns.Add("GroupTopicID");
            dtGroupTopics.Columns.Add("GroupTopicName");
            dtGroupTopics.Columns.Add("Posts");
            dtGroupTopics.Columns.Add("DateCreated");
            dtGroupTopics.Columns.Add("Username");
            dtGroupTopics.Columns.Add("ImageID", typeof(int));

            if (Results != null)
            {
                string icon;
                GroupTopic[] groupTopics = null;

                groupTopics = Results.GetPage(CurrentPage, Config.Groups.GroupTopicsPerPage);

                if (groupTopics != null && groupTopics.Length > 0)
                {
                    for (int i = 0; i < groupTopics.Length; i++)
                    {
                        GroupTopic groupTopic = groupTopics[i];

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
                                                   groupTopic.DateUpdated.Add(Config.Misc.TimeOffset),
                                                   groupTopic.Username,
                                                   imageID
                                                });
                    }
                }
            }

            dgGroupTopics.Visible = dtGroupTopics.Rows.Count > 0;
            dgGroupTopics.DataSource = dtGroupTopics;
            dgGroupTopics.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            int[] groupTopicIDs;

            if (cbSearchInPosts.Checked)
            {
                groupTopicIDs = GroupTopic.Search(GroupID, null, null, null, null, null, txtTopicToSearch.Text, true);
            }
            else
            {
                groupTopicIDs = GroupTopic.Search(GroupID, null, null, null, null, null, txtTopicToSearch.Text, false);
            }

            OnSearchTopicClick(groupTopicIDs);
        }

        protected void OnSearchTopicClick(int[] topicIDs)
        {
            if (SearchTopicClick != null)
            {
                SearchTopicClick(this, new SearchTopicEventArgs(topicIDs));
            }
        }
    }
}