using System;
using System.Data;
using System.Web.UI;
using AspNetDating.Classes;

namespace AspNetDating.Components.Groups
{
    public partial class ViewTopics : UserControl
    {
        private bool paginatorEnabled = true;

        public int GroupID
        {
            get
            {
                if (ViewState["CurrentGroupId"] != null)
                {
                    return (int)ViewState["CurrentGroupId"];
                }
                throw new Exception("The field groupID is not set!");
            }
            set { ViewState["CurrentGroupId"] = value; }
        }

        /// <summary>
        /// Gets or sets the number of topics which will be showed.
        /// If it is set with NULL this user control will show all topics for the group.
        /// </summary>
        /// <value>The topics count.</value>
        public int? TopicsCount
        {
            get { return (int?)ViewState["TopicsCount"]; }
            set { ViewState["TopicsCount"] = value; }
        }

        public string TxtTopicToSearch
        {
            get { return txtTopicToSearch.Text; }
            set { txtTopicToSearch.Text = value; }
        }

        public bool CbSearchInPosts
        {
            get { return cbSearchInPosts.Checked; }
            set { cbSearchInPosts.Checked = value; }
        }

        public GroupTopicSearchResults Results
        {
            set
            {
                ViewState["SearchResults"] = value;
                CurrentPage = 1;
            }
            get
            {
                return (GroupTopicSearchResults)
                       ViewState["SearchResults"];
            }
        }

        public int CurrentPage
        {
            set
            {
                ViewState["CurrentPage"] = value;
                PreparePaginator();
            }
            get
            {
                if (ViewState["CurrentPage"] == null
                    || (int)ViewState["CurrentPage"] == 0)
                    return 1;
                return (int)ViewState["CurrentPage"];
            }
        }

        public bool PaginatorEnabled
        {
            get { return paginatorEnabled; }
            set
            {
                pnlPaginator.Visible = value;
                paginatorEnabled = value;
            }
        }

        public event SearchTopicClickEventHandler SearchTopicClick;

        protected void Page_Load(object sender, EventArgs e)
        {
            loadStrings();

            if (!Visible)
            {
                Results = null;
            }
            else
            {
                // Load the results in advance unless paginator is used
                var eventTarget = Page.Request.Params["__EVENTTARGET"];
                if (eventTarget != null && !eventTarget.EndsWith("lnkNext")
                    && !eventTarget.EndsWith("lnkPrev") && !eventTarget.EndsWith("lnkNext")
                    && !eventTarget.EndsWith("lnkLast"))
                {
                    loadResultsPage();
                }
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            loadGroupTopics();
            loadResultsPage();
        }

        private void loadStrings()
        {
            LargeBoxStart1.Title = Lang.Trans("Topics");

            dgGroupTopics.Columns[1].HeaderText = Lang.Trans("Poster");
            dgGroupTopics.Columns[2].HeaderText = Lang.Trans("Topic");
            dgGroupTopics.Columns[3].HeaderText = Lang.Trans("Posts");
            dgGroupTopics.Columns[4].HeaderText = Lang.Trans("Views");
            dgGroupTopics.Columns[5].HeaderText = Lang.Trans("Last Posted");

            btnSearch.Text = Lang.Trans("Search");

            lnkFirst.Text = Lang.Trans("[ First ]");
            lnkPrev.Text = Lang.Trans("[ Prev ]");
            lnkNext.Text = Lang.Trans("[ Next ]");
            lnkLast.Text = Lang.Trans("[ Last ]");
        }

        private void PreparePaginator()
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
                int fromUser = (CurrentPage - 1) * Config.Groups.GroupTopicsPerPage + 1;
                int toUser = CurrentPage * Config.Groups.GroupTopicsPerPage;
                if (Results.GroupTopics.Length < toUser)
                    toUser = Results.GroupTopics.Length;

                lblPager.Text = String.Format(
                    Lang.Trans("Showing {0}-{1} from {2} total"),
                    fromUser, toUser, Results.GroupTopics.Length);
            }
            else
            {
                lblPager.Text = Lang.Trans("No Results");
            }
        }

        private void loadResultsPage()
        {
            if (dgGroupTopics.Items.Count > 0) return;

            PreparePaginator();

            var dtGroupTopics = new DataTable("SearchResults");
            dtGroupTopics.Columns.Add("Icon");
            dtGroupTopics.Columns.Add("GroupTopicID");
            dtGroupTopics.Columns.Add("GroupTopicName");
            dtGroupTopics.Columns.Add("Posts");
            dtGroupTopics.Columns.Add("Views");
            dtGroupTopics.Columns.Add("DateCreated");
            dtGroupTopics.Columns.Add("Username");
            dtGroupTopics.Columns.Add("ImageID", typeof(int));

            if (Results != null)
            {
                GroupTopic[] groupTopics = Results.GetPage(CurrentPage, Config.Groups.GroupTopicsPerPage);

                if (groupTopics != null && groupTopics.Length > 0)
                {
                    for (int i = 0; i < groupTopics.Length; i++)
                    {
                        GroupTopic groupTopic = groupTopics[i];

                        string icon = GroupTopic.GetIconString(groupTopic);

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
                }
            }

            Trace.Write("Binding...");

            dgGroupTopics.DataSource = dtGroupTopics;
            dgGroupTopics.DataBind();
        }

        private void loadGroupTopics()
        {
            if (Results == null)
            {
                Results = new GroupTopicSearchResults
                              {
                                  GroupTopics = GroupTopic.Search(GroupID, null, null, null, null, null, null, false)
                              };

                if (Results.GroupTopics.Length == 0)
                {
                    PaginatorEnabled = false;
                    dgGroupTopics.Visible = false;

                    lblError.Text = Lang.Trans("There are no topics.");
                    pnlFilterTopics.Visible = false;
                    return;
                }

                CurrentPage = 1;
            }
        }

        protected void lnkFirst_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 1)
                CurrentPage = 1;

            loadResultsPage();
        }

        protected void lnkPrev_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 1)
                CurrentPage--;

            loadResultsPage();
        }

        protected void lnkNext_Click(object sender, EventArgs e)
        {
            if (CurrentPage < Results.GetTotalPages(Config.Groups.GroupTopicsPerPage))
                CurrentPage++;

            loadResultsPage();
        }

        protected void lnkLast_Click(object sender, EventArgs e)
        {
            if (CurrentPage < Results.GetTotalPages(Config.Groups.GroupTopicsPerPage))
                CurrentPage = Results.GetTotalPages(Config.Groups.GroupTopicsPerPage);

            loadResultsPage();
        }

        public void ShowMessage(Misc.MessageType type, string message)
        {
            lblError.Text = message;
            switch (type)
            {
                case Misc.MessageType.Error:
                    lblError.CssClass = "error";
                    break;
                case Misc.MessageType.Success:
                    lblError.CssClass = "message";
                    break;
            }
        }

        public void btnSearch_Click(object sender, EventArgs e)
        {
            int[] groupTopicIDs = GroupTopic.Search(GroupID, null, null, null, null, null, txtTopicToSearch.Text, cbSearchInPosts.Checked);

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

    public delegate void SearchTopicClickEventHandler(object sender, SearchTopicEventArgs eventArgs);

    public class SearchTopicEventArgs : EventArgs
    {
        public SearchTopicEventArgs(int[] topicIDs)
        {
            TopicIDs = topicIDs;
        }

        public int[] TopicIDs { get; set; }
    }
}