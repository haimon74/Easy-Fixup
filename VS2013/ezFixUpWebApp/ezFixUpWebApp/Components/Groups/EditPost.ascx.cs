using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using ezFixUp.Classes;
using Group=ezFixUp.Classes.Group;

namespace ezFixUp.Components.Groups
{
    public partial class EditPost : System.Web.UI.UserControl
    {
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
            set
            {
                ViewState["CurrentGroupId"] = value;
            }
        }

        public int TopicID
        {
            get
            {
                if (ViewState["CurrentTopicId"] != null)
                {
                    return (int)ViewState["CurrentTopicId"];
                }
                throw new Exception("The topic ID is not set!");
            }
            set
            {
                ViewState["CurrentTopicId"] = value;
            }
        }

        public int PostID
        {
            get { return Convert.ToInt32(ViewState["CurrentPost"]); }
            set { ViewState["CurrentPost"] = value;}
        }

        protected string MessageBodyClientId
        {
            get { return txtPost.ClientID; }
        }

        public GroupMember CurrentGroupMember
        {
            get
            {
                if (Page is ShowGroupTopics)
                {
                    return ((ShowGroupTopics)Page).CurrentGroupMember;
                }
                if (CurrentUserSession != null)
                {
                    return GroupMember.Fetch(GroupID, CurrentUserSession.Username);
                }
                return null;
            }
        }

        /// <summary>
        /// Specifies the type of this user control.
        /// Based on the type the user control will be rendered in different way.
        /// </summary>
        public enum eType
        {
            /// <summary>
            /// User control will be used for adding a new topic.
            /// </summary>
            AddTopic,

            EditTopic,

            /// <summary>
            /// User control will be used for adding a new post.
            /// </summary>
            AddPost,

            /// <summary>
            /// User control will be used for editing a post.
            /// </summary>
            EditPost
        }

        private eType type = eType.AddTopic;
        /// <summary>
        /// Gets or sets the type. The default value is 'AddTopic'.
        /// </summary>
        /// <value>The type.</value>
        public eType Type
        {
            get { return (eType) (ViewState["EditPost_Type"] ?? type); }
            set
            {
                type = value;

                if (type == eType.AddTopic || type == eType.AddPost)
                {
                    txtTopicName.Text = "";
                    txtPost.Text = "";
                }

                if (type == eType.EditPost)
                {
                    txtEditReason.Text = "";
                }

                ViewState["EditPost_Type"] = value;
            }
        }

        private string post;
        public string Post
        {
            get { return post; }
            set { post = value; }
        }

        private UserSession CurrentUserSession
        {
            get { return ((PageBase) Page).CurrentUserSession; }
        }

        private bool loadTopic = false;
        public bool LoadTopic
        {
            get { return loadTopic; }
            set { loadTopic = value;}
        }

        private int pageNumber;
        public int PageNumber
        {
            get
            {
                pageNumber = (int) ViewState["EditPost_PageNumber"];

                return pageNumber;
            }
            set
            {
                ViewState["EditPost_PageNumber"] = value;
                pageNumber = value;
            }
        }

        public int CurrentSmiliesPage
        {
            set
            {
                ViewState["CurrentSmiliesPage"] = value;
            }
            get
            {
                return (int)(ViewState["CurrentSmiliesPage"] ?? 0);
            }
        }

        public event EventHandler ViewPosts;
        public event UpdatePostEventHandler UpdatePost;
        public event UpdateTopicEventHandler UpdateTopic;
        public event EventHandler CancelUpdateTopic;
        public event EventHandler CancelStartNewTopicClick;
        public event EventHandler CancelNewPostClick;
        public event EventHandler CancelUpdatePostClick;

        //Invoke delegates registered with the ViewPostsClickEvent event.
        protected virtual void OnViewPostsClick(EventArgs e)
        {
            if (ViewPosts != null)
            {
                ViewPosts(this, e);
            }
        }

        //Invoke delegates registered with the ViewPostsClickEvent event.
        protected virtual void OnUpdatePostClick(UpdatePostEventArgs e)
        {
            if (UpdatePost != null)
            {
                UpdatePost(this, e);
            }
        }

        protected virtual void OnUpdateTopicClick(int topicID)
        {
            if (UpdateTopic != null)
            {
                UpdateTopic(this, new UpdateTopicEventArgs(topicID));
            }
        }

        protected virtual void OnCancelUpdateTopicClick(EventArgs e)
        {
            if (CancelUpdateTopic != null)
            {
                CancelUpdateTopic(this, e);
            }
        }

        protected virtual void OnCancelStartNewTopicClick(EventArgs e)
        {
            if (CancelStartNewTopicClick != null)
            {
                CancelStartNewTopicClick(this, e);
            }
        }

        protected virtual void OnCancelNewPostClick(EventArgs e)
        {
            if (CancelNewPostClick != null)
            {
                CancelNewPostClick(this, e);
            }
        }

        protected virtual void OnCancelUpdatePostClick(EventArgs e)
        {
            if (CancelUpdatePostClick != null)
            {
                CancelUpdatePostClick(this, e);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                createPollsChoices();
                DatePicker1.MinYear = DateTime.Now.Year;
                DatePicker1.MaxYear = DateTime.Now.AddYears(10).Year;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            loadStrings();
            loadSmileys();
            setControl();

            if (LoadTopic)
            {
                populateTopic();
            }
        }

        private void populateTopic()
        {
            GroupTopic groupTopic = GroupTopic.Fetch(TopicID);

            if (groupTopic != null)
            {
                txtTopicName.Text = groupTopic.Name;
                cbLocked.Checked = groupTopic.Locked;

                if (groupTopic.StickyUntil != null)
                {
                    DatePicker1.SelectedDate = groupTopic.StickyUntil.Value;
                    cbSticky.Checked = true;
                }
            }
        }

        private void setControl()
        {
            switch (Type)
            {
                case eType.AddTopic :
                    LargeBoxStart1.Title = Lang.Trans("Add Topic");
                    pnlTopic.Visible = true;
                    pnlPost.Visible = true;
                    pnlPoll.Visible = true;
                    pnlLocked.Visible = Visible();
                    cbLocked.Checked = false;
                    pnlSticky.Visible = Visible();
                    cbSticky.Checked = false;
                    DatePicker1.Visible = Visible();
                    DatePicker1.Reset();
                    btnStartNewTopic.Visible = true;
                    btnCancelStartNewTopic.Visible = true;
                    btnUpdateTopic.Visible = false;
                    btnCancelUpdateTopic.Visible = false;
                    pnlEditReason.Visible = false;
                    btnNewPost.Visible = false;
                    btnCancelNewPost.Visible = false;
                    btnUpdatePost.Visible = false;
                    btnCancelUpdatePost.Visible = false;
                    break;

                case eType.EditTopic:
                    LargeBoxStart1.Title = Lang.Trans("Edit Topic");
                    pnlTopic.Visible = true;
                    pnlPost.Visible = false;
                    pnlPoll.Visible = false;
                    pnlLocked.Visible = Visible();
                    pnlSticky.Visible = Visible();
                    pnlMoveTopic.Visible = Visible();
                    DatePicker1.Visible = Visible();
                    btnStartNewTopic.Visible = false;
                    btnCancelStartNewTopic.Visible = false;
                    btnUpdateTopic.Visible = true;
                    btnCancelUpdateTopic.Visible = true;
                    pnlEditReason.Visible = false;
                    btnNewPost.Visible = false;
                    btnCancelNewPost.Visible = false;
                    btnUpdatePost.Visible = false;
                    btnCancelUpdatePost.Visible = false;

                    if (ddMoveToGroups.Items.Count <= 1)
                    {
                        foreach (GroupMember member in GroupMember.Fetch(CurrentUserSession.Username))
                        {
                            if (member.Type == GroupMember.eType.Moderator 
                                || member.Type == GroupMember.eType.Admin 
                                || CurrentUserSession.Username == "admin")
                            {
                                if (member.GroupID == GroupID) continue;
                                Group group = Group.Fetch(member.GroupID);
                                ddMoveToGroups.Items.Add(new ListItem(group.Name, group.ID.ToString()));
                            }
                        }
                        if (ddMoveToGroups.Items.Count == 1)
                            pnlMoveTopic.Visible = false;
                    }
                    break;

                case eType.AddPost :
                    LargeBoxStart1.Title = Lang.Trans("Add Post");
                    pnlTopic.Visible = false;
                    pnlPost.Visible = true;
                    pnlPoll.Visible = false;
                    txtPost.Text = post;
                    pnlLocked.Visible = false;
                    pnlSticky.Visible = false;
                    DatePicker1.Visible = false;
                    btnStartNewTopic.Visible = false;
                    btnCancelStartNewTopic.Visible = false;
                    btnUpdateTopic.Visible = false;
                    btnCancelUpdateTopic.Visible = false;
                    btnUpdatePost.Visible = false;
                    btnCancelUpdatePost.Visible = false;
                    pnlEditReason.Visible = false;
                    btnNewPost.Visible = true;
                    btnCancelNewPost.Visible = true;
                    break;

                case eType.EditPost :
                    LargeBoxStart1.Title = Lang.Trans("Edit Post");
                    pnlTopic.Visible = false;
                    pnlPost.Visible = true;
                    pnlPoll.Visible = false;
                    txtPost.Text = post;
                    pnlLocked.Visible = false;
                    pnlSticky.Visible = false;
                    DatePicker1.Visible = false;
                    btnStartNewTopic.Visible = false;
                    btnCancelStartNewTopic.Visible = false;
                    btnUpdateTopic.Visible = false;
                    btnCancelUpdateTopic.Visible = false;
                    btnNewPost.Visible = false;
                    btnCancelNewPost.Visible = false;
                    pnlEditReason.Visible = true;
                    btnUpdatePost.Visible = true;
                    btnCancelUpdatePost.Visible = true;
                    break;
            }
        }

        private void loadStrings()
        {
            btnStartNewTopic.Text = Lang.Trans("Post");
            btnCancelStartNewTopic.Text = Lang.Trans("Cancel");
            btnUpdateTopic.Text = Lang.Trans("Update");
            btnCancelUpdateTopic.Text = Lang.Trans("Cancel");
            btnNewPost.Text = Lang.Trans("Add new Post");
            btnCancelNewPost.Text = Lang.Trans("Cancel");
            btnUpdatePost.Text = Lang.Trans("Update");
            btnCancelUpdatePost.Text = Lang.Trans("Cancel");

            if (ddMoveToGroups.Items.Count == 0)
                ddMoveToGroups.Items.Add(new ListItem(Lang.Trans("-- don't move --"), ""));
        }

        private void loadSmileys()
        {
            var lUsedSmilies = new List<string>();
            var dtSmilies = new DataTable();
            dtSmilies.Columns.Add("Key");
            dtSmilies.Columns.Add("Image");
            dtSmilies.Columns.Add("Description");
            dtSmilies.Columns.Add("IsSecondary");

            foreach (string key in Smilies.dSmileys.Keys)
            {
                Smiley smiley = Smilies.dSmileys[key];
                if (lUsedSmilies.IndexOf(smiley.Image) >= 0) continue;
                lUsedSmilies.Add(smiley.Image);

                var row = dtSmilies.NewRow();
                row.ItemArray = new object[]
                                 {
                                     smiley.Key, Config.Urls.Home + "/Smilies/" + smiley.Image,
                                     smiley.Description, smiley.Secondary
                                 };

                dtSmilies.Rows.Add(row);
            }

            dtSmilies.DefaultView.Sort = "IsSecondary";

            var pagedSource = new PagedDataSource
            {
                AllowPaging = true,
                PageSize = 41,
                DataSource = dtSmilies.DefaultView,
            };
            if (CurrentSmiliesPage < 0) CurrentSmiliesPage = pagedSource.PageCount - 1;
            if (CurrentSmiliesPage >= pagedSource.PageCount) CurrentSmiliesPage = 0;
            pagedSource.CurrentPageIndex = CurrentSmiliesPage;

            dlSmilies.DataSource = pagedSource;
            dlSmilies.DataBind();
        }

        protected void btnStartNewTopic_Click(object sender, EventArgs e)
        {
            string topicName = Config.Misc.EnableBadWordsFilterGroups ? Parsers.ProcessBadWords(txtTopicName.Text.Trim()) : txtTopicName.Text.Trim();
            string post = Config.Misc.EnableBadWordsFilterGroups ? Parsers.ProcessBadWords(txtPost.Text.Trim()) : txtPost.Text.Trim();

            if (topicName.Length == 0)
            {
                type = eType.AddTopic;

                lblError.Text = Lang.Trans("Please enter topic name.");
                return;
            }

            if (post.Length == 0)
            {
                type = eType.AddTopic;

                lblError.Text = Lang.Trans("Please enter post text.");
                return;
            }

            #region Find unclosed [quote] tags

            int openQuotesCount = Regex.Matches(post, @"\[quote", RegexOptions.IgnoreCase).Count;
            int closedQuotesCount = Regex.Matches(post, @"\[/quote", RegexOptions.IgnoreCase).Count;
            if (openQuotesCount != closedQuotesCount)
            {
                type = eType.AddTopic;
                this.post = post;

                lblError.Text = Lang.Trans("Please close all [quote] tags with a [/quote] tag!");
                return;
            }

            #endregion

            if (CurrentUserSession != null)
            {
                var groupTopic = new GroupTopic(GroupID, CurrentUserSession.Username)
                                     {
                                         Name = topicName,
                                         Posts = 1,
                                         Locked = cbLocked.Checked
                                     };

                if (cbCreatePoll.Checked)
                {
                    if (validatePollsChoices())
                    {
                        groupTopic.IsPoll = true;
                    }
                    else
                    {
                        lblError.Text = Lang.Trans("Please enter at least one choice!");
                        return;
                    }
                }

                if (cbSticky.Checked)
                {
                    if (!DatePicker1.ValidDateEntered)
                    {
                        lblError.Text = Lang.Trans("Please select date!");
                        return;
                    }
                    groupTopic.StickyUntil = DatePicker1.SelectedDate;
                }

                groupTopic.Save();

                User.AddScore(CurrentUserSession.Username, Config.UserScores.NewTopic, "NewTopic");

                var groupPost = new GroupPost(groupTopic.ID, CurrentUserSession.Username) {Post = post};
                groupPost.Save();

                #region Subscribe automatically for this topic

                GroupTopicSubscription groupTopicSubscription = new GroupTopicSubscription(CurrentUserSession.Username, groupTopic.ID, GroupID);
                groupTopicSubscription.DateUpdated = groupTopic.DateUpdated;
                groupTopicSubscription.Save();

                #endregion

                #region create a poll

                if (cbCreatePoll.Checked)
                {
                    foreach (RepeaterItem item in rptChoices.Items)
                    {
                        TextBox txtChoice = item.FindControl("txtChoice") as TextBox;

                        if (txtChoice != null && txtChoice.Text.Trim() != String.Empty)
                        {
                            GroupPollsChoice choice = new GroupPollsChoice(groupTopic.ID);
                            choice.Answer = txtChoice.Text.Trim();
                            choice.Save();
                        }
                    }
                }

                #endregion

                #region Add NewGroupTopic Event

                Event newEvent = new Event(CurrentUserSession.Username);

                newEvent.FromGroup = GroupID;
                newEvent.Type = Event.eType.NewGroupTopic;
                NewGroupTopic newGroupTopic = new NewGroupTopic();
                newGroupTopic.GroupTopicID = groupTopic.ID;
                newEvent.DetailsXML = Misc.ToXml(newGroupTopic);

                newEvent.Save();

                Group group = Group.Fetch(groupTopic.GroupID);

                string[] usernames = User.FetchMutuallyFriends(CurrentUserSession.Username);

                foreach (string friendUsername in usernames)
                {
                    if (Config.Users.NewEventNotification)
                    {
                        if (group != null)
                        {
                            string text =
                                String.Format("Your friend {0} has posted a new topic {1} in the {2} group".Translate(),
                                              "<b>" + CurrentUserSession.Username + "</b>",
                                              Server.HtmlEncode(groupTopic.Name), Server.HtmlEncode(group.Name));
                            int imageID = 0;
                            try
                            {
                                imageID = Photo.GetPrimary(CurrentUserSession.Username).Id;
                            }
                            catch (NotFoundException)
                            {
                                imageID = ImageHandler.GetPhotoIdByGender(CurrentUserSession.Gender);
                            }
                            string thumbnailUrl = ImageHandler.CreateImageUrl(imageID, 50, 50, false, true, true);
                            User.SendOnlineEventNotification(CurrentUserSession.Username, friendUsername, text,
                                                             thumbnailUrl,
                                                             UrlRewrite.CreateShowGroupTopicsUrl(
                                                                 groupTopic.ID.ToString()));
                        }
                    }
                }

                GroupMember[] groupMembers = GroupMember.Fetch(GroupID, true);

                foreach (GroupMember groupMember in groupMembers)
                {
                    // A user should not receive events for their topics
                    if (groupMember.Username == CurrentUserSession.Username) continue;

                    if (Config.Users.NewEventNotification)
                    {
                        if (group != null)
                        {
                            string text =
                                String.Format("There is a new topic {0} in the {1} group".Translate(),
                                              "<b>" + Server.HtmlEncode(groupTopic.Name) + "</b>",
                                              Server.HtmlEncode(group.Name));
                            string thumbnailUrl = GroupIcon.CreateImageUrl(group.ID, 50, 50, true);
                            User.SendOnlineEventNotification(CurrentUserSession.Username, groupMember.Username, text,
                                                             thumbnailUrl, UrlRewrite.CreateShowGroupUrl(group.ID.ToString()));
                        }
                    }
                }

                #endregion
            }

            Response.Redirect(UrlRewrite.CreateShowGroupTopicsUrl(GroupID.ToString()));
            
        }

        protected void btnCancelStartNewTopic_Click(object sender, EventArgs e)
        {
            OnCancelStartNewTopicClick(e);
        }

        protected void btnNewPost_Click(object sender, EventArgs e)
        {
            GroupTopic groupTopic = GroupTopic.Fetch(TopicID);

            if (CurrentUserSession != null && groupTopic != null)
            {
                string post = Config.Misc.EnableBadWordsFilterGroups
                                  ? Parsers.ProcessBadWords(txtPost.Text.Trim())
                                  : txtPost.Text.Trim();

                if (post.Length == 0)
                {
                    type = eType.AddPost;

                    lblError.Text = Lang.Trans("Please enter post text.");
                    return;
                }

                #region Find unclosed [quote] tags

                int openQuotesCount = Regex.Matches(post, @"\[quote", RegexOptions.IgnoreCase).Count;
                int closedQuotesCount = Regex.Matches(post, @"\[/quote", RegexOptions.IgnoreCase).Count;
                if (openQuotesCount != closedQuotesCount)
                {
                    type = eType.AddPost;
                    this.post = post;

                    lblError.Text = Lang.Trans("Please close all [quote] tags with a [/quote] tag!");
                    return;
                }

                #endregion

                if (!GroupPost.IsDuplicate(TopicID, CurrentUserSession.Username, post))
                {
                    GroupPost groupPost = new GroupPost(TopicID, CurrentUserSession.Username);

                    groupPost.Post = post;
                    groupPost.Save();

                    User.AddScore(CurrentUserSession.Username, Config.UserScores.NewPost, "NewPost");
                    User.AddScore(groupTopic.Username, Config.UserScores.NewPostsOnUserTopic, "NewPostsOnUserTopic");

                    groupTopic.Posts++;
                    groupTopic.DateUpdated = DateTime.Now;
                    groupTopic.Save();

                    GroupTopicSubscription groupTopicSubscription =
                        GroupTopicSubscription.Fetch(CurrentUserSession.Username, TopicID);

                    if (groupTopicSubscription != null) // is subscribed
                    {
                        groupTopicSubscription.DateUpdated = groupTopic.DateUpdated;
                        groupTopicSubscription.Save();
                    }

                    OnViewPostsClick(new EventArgs());
                }
                else
                {
                    type = eType.AddPost;
                    this.post = post;

                    lblError.Text = Lang.Trans("Duplicate post!");
                    return;
                }
            }
        }

        protected void btnUpdatePost_Click(object sender, EventArgs e)
        {
            GroupPost groupPost = GroupPost.Fetch(PostID);

            if (groupPost.Username != CurrentUserSession.Username
                && CurrentGroupMember.Type != GroupMember.eType.Admin
                && CurrentGroupMember.Type != GroupMember.eType.Moderator
                && CurrentUserSession.Username != "admin")
            {
                Global.Logger.LogError("Unauthorized attempt to update a post by member " +
                                       CurrentUserSession.Username + " with IP " + Request.UserHostAddress + 
                                       " (post " + groupPost.ID + " by " + groupPost.Username + ")");
                return;
            }

            if (txtPost.Text.Trim().Length == 0)
            {
                type = eType.EditPost;
                lblError.Text = Lang.Trans("Please enter post text.");
                return;
            }

            groupPost.Post = Config.Misc.EnableBadWordsFilterGroups ? Parsers.ProcessBadWords(txtPost.Text.Trim()) : txtPost.Text.Trim();

            #region Find unclosed [quote] tags

            int openQuotesCount = Regex.Matches(groupPost.Post, @"\[quote", RegexOptions.IgnoreCase).Count;
            int closedQuotesCount = Regex.Matches(groupPost.Post, @"\[/quote", RegexOptions.IgnoreCase).Count;
            if (openQuotesCount != closedQuotesCount)
            {
                type = eType.EditPost;
                post = txtPost.Text.Trim();

                lblError.Text = Lang.Trans("Please close all [quote] tags with a [/quote] tag!");
                return;
            }

            #endregion
            
            groupPost.DateEdited = DateTime.Now;
            groupPost.EditNotes = txtEditReason.Text.Trim().Length == 0 ? null : Config.Misc.EnableBadWordsFilterGroups ? Parsers.ProcessBadWords(txtEditReason.Text.Trim()) : txtEditReason.Text.Trim();
            groupPost.Save();

            OnUpdatePostClick(new UpdatePostEventArgs(groupPost.ID, PageNumber));
        }

        protected void btnCancelNewPost_Click(object sender, EventArgs e)
        {
            OnCancelNewPostClick(e);
        }

        protected void btnCancelUpdatePost_Click(object sender, EventArgs e)
        {
            OnCancelUpdatePostClick(e);
        }

        public delegate void UpdateTopicEventHandler(object sender, UpdateTopicEventArgs e);

        protected void btnUpdateTopic_Click(object sender, EventArgs e)
        {
            string name = Config.Misc.EnableBadWordsFilterGroups ? Parsers.ProcessBadWords(txtTopicName.Text.Trim()) : txtTopicName.Text.Trim();

            if (name.Length == 0)
            {
                type = eType.EditTopic;

                lblError.Text = Lang.Trans("Please enter topic name.");
                return;
            }

            GroupTopic groupTopic = GroupTopic.Fetch(TopicID);

            if (groupTopic != null)
            {
                if (groupTopic.Username != CurrentUserSession.Username
                    && CurrentGroupMember.Type != GroupMember.eType.Admin
                    && CurrentGroupMember.Type != GroupMember.eType.Moderator
                    && CurrentUserSession.Username != "admin")
                {
                    Global.Logger.LogError("Unauthorized attempt to update a topic by member " +
                                           CurrentUserSession.Username + " with IP " + Request.UserHostAddress +
                                           " (topic " + groupTopic.ID + " by " + groupTopic.Username + ")");
                    return;
                }

                groupTopic.Name = name;
                groupTopic.Locked = cbLocked.Checked;
                groupTopic.StickyUntil = null;

                if (cbSticky.Checked)
                {
                    if (!DatePicker1.ValidDateEntered)
                    {
                        type = eType.EditTopic;

                        lblError.Text = Lang.Trans("Please select date!");
                        return;
                    }
                    else
                    {
                        groupTopic.StickyUntil = DatePicker1.SelectedDate.AddHours(23).AddMinutes(59).AddSeconds(59);
                    }
                }

                if (ddMoveToGroups.SelectedIndex > 0)
                {
                    int groupId = Convert.ToInt32(ddMoveToGroups.SelectedValue);
                    GroupMember member = GroupMember.Fetch(groupId, CurrentUserSession.Username);
                    if (CurrentUserSession.Username == "admin"
                        || member.Type == GroupMember.eType.Admin || member.Type == GroupMember.eType.Moderator)
                    {
                        groupTopic.GroupID = groupId;
                        GroupTopicSubscription[] subscriptions = GroupTopicSubscription.Fetch(null, null, groupTopic.ID, null, null);
                        foreach (var subscription in subscriptions)
                        {
                            GroupTopicSubscription.Delete(subscription.ID);
                        }
                    }
                }

                groupTopic.Save();
            }

            OnUpdateTopicClick(TopicID);
        }

        protected void btnCancelUpdateTopic_Click(object sender, EventArgs e)
        {
            OnCancelUpdateTopicClick(e);
        }

        /// <summary>
        /// Determines when to show server control.
        /// Returns true if the Admin is logged in or if the group member is Admin/Moderator, otherwise returns false.
        /// </summary>
        /// <returns></returns>
        private new bool Visible()
        {
            if ((CurrentUserSession != null && CurrentUserSession.IsAdmin())
                || (CurrentGroupMember != null 
                 && (CurrentGroupMember.Type == GroupMember.eType.Admin || CurrentGroupMember.Type == GroupMember.eType.Moderator)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected void cbCreatePoll_CheckedChanged(object sender, EventArgs e)
        {
            if (cbCreatePoll.Checked)
            {
                pnlChoices.Visible = true;
            }
            else
            {
                pnlChoices.Visible = false;
            }
        }

        private void createPollsChoices()
        {
            var dtPollsChoices = new DataTable();

            dtPollsChoices.Columns.Add("NumberOfChoice");

            for (int i = 0; i < Config.Groups.NumberOfGroupPollsChoices; i++ )
            {
                dtPollsChoices.Rows.Add(new object[] {i + 1});
            }

            rptChoices.DataSource = dtPollsChoices;
            rptChoices.DataBind();
        }

        private bool validatePollsChoices()
        {
            bool result = false;

            foreach (RepeaterItem item in rptChoices.Items)
            {
                TextBox txtChoice = item.FindControl("txtChoice") as TextBox;

                if (txtChoice != null && txtChoice.Text.Trim() != String.Empty)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        protected void ibtnPrevSmilies_Click(object sender, ImageClickEventArgs e)
        {
            CurrentSmiliesPage--;
            loadSmileys();
        }

        protected void ibtnNextSmilies_Click(object sender, ImageClickEventArgs e)
        {
            CurrentSmiliesPage++;
            loadSmileys();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class UpdateTopicEventArgs : EventArgs
    {
        private int topicID;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateTopicEventArgs"/> class.
        /// </summary>
        /// <param name="topicID">The topic ID.</param>
        public UpdateTopicEventArgs(int topicID)
        {
            this.topicID = topicID;
        }

        /// <summary>
        /// Gets or sets the topic ID.
        /// </summary>
        /// <value>The topic ID.</value>
        public int TopicID
        {
            get { return topicID; }
            set { topicID = value; }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public delegate void UpdatePostEventHandler(object sender, UpdatePostEventArgs e);
    /// <summary>
    /// 
    /// </summary>
    public class UpdatePostEventArgs : EventArgs
    {
        private int postID;
        private int currentPage;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdatePostEventArgs"/> class.
        /// </summary>
        /// <param name="postID">The post ID.</param>
        /// <param name="currentPage">The current page.</param>
        public UpdatePostEventArgs(int postID, int currentPage)
        {
            this.postID = postID;
            this.currentPage = currentPage;
        }

        /// <summary>
        /// Gets or sets the post ID.
        /// </summary>
        /// <value>The post ID.</value>
        public int PostID
        {
            get { return postID; }
            set { postID = value; }
        }

        /// <summary>
        /// Gets or sets the current page.
        /// </summary>
        /// <value>The current page.</value>
        public int CurrentPage
        {
            get { return currentPage; }
            set { currentPage = value; }
        }
    }
}