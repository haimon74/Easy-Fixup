using System;
using System.Web.UI.HtmlControls;
using AspNetDating.Classes;

namespace AspNetDating
{
    public partial class ShowGroupPhotos : PageBase
    {
        public ShowGroupPhotos()
        {
            RequiresAuthorization = false;
        }

        /// <summary>
        /// Gets the 'CurrentGroupId' from View State.
        /// It returns -1 if it is null.
        /// </summary>
        /// <value>The group ID.</value>
        public int GroupID
        {
            get
            {
                if (ViewState["CurrentGroupId"] != null)
                {
                    return (int) ViewState["CurrentGroupId"];
                }
                else
                {
                    return -1;
                }
            }
            set
            {
                ViewState["CurrentGroupId"] = value;
            }
        }

        /// <summary>
        /// Gets the group from DB and saves it in 'ViewState'.
        /// If the group doesn't exist it returns NULL.
        /// </summary>
        /// <value>The group.</value>
        public Group Group
        {
            get
            {
                if (ViewState["CurrentGroup"] == null)
                {
                    ViewState["CurrentGroup"] = Group.Fetch(GroupID);
                }

                return ViewState["CurrentGroup"] as Group;
            }
        }

        public GroupMember CurrentGroupMember
        {
            get
            {
                if (ViewState["CurrentGroupMember_IsNull"] != null &&
                    (bool)ViewState["CurrentGroupMember_IsNull"])
                {
                    return null;
                }
                else if (ViewState["CurrentGroupMember"] == null)
                {
                    if (CurrentUserSession != null)
                    {
                        GroupMember groupMember = GroupMember.Fetch(GroupID, CurrentUserSession.Username);
                        if (groupMember == null)
                        {
                            ViewState["CurrentGroupMember_IsNull"] = true;
                        }
                        else
                        {
                            ViewState["CurrentGroupMember"] = groupMember;
                            ViewState["CurrentGroupMember_IsNull"] = false;
                        }
                    }
                }

                return ViewState["CurrentGroupMember"] as GroupMember;
            }

            set
            {
                if (value == null)
                {
                    ViewState["CurrentGroupMember_IsNull"] = true;
                }
                else
                {
                    ViewState["CurrentGroupMember_IsNull"] = false;
                }

                ViewState["CurrentGroupMember"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            #region Show AddThis link

            if (Config.ThirdPartyServices.UseAddThis)
            {
                divAddThis.Visible = true;
                divAddThis.InnerHtml = Config.ThirdPartyServices.AddThisCode;
            }

            #endregion
            
            ViewGroupPhotos1.UploadPhotoClick += new EventHandler(ViewGroupPhotos1_UploadPhotoClick);
            if (!IsPostBack)
            {
                if (CurrentUserSession == null && Config.Groups.OnlyRegisteredUsersCanBrowseGroups)
                    Response.Redirect("~/LoginThroughFacebook.aspx?back_url=" + Server.UrlEncode(Request.Url.AbsoluteUri));

                if (CurrentUserSession != null)
                {
                    var permissionCheckResult = CurrentUserSession.CanBrowseGroups();

                    if (permissionCheckResult == PermissionCheckResult.YesButMoreCreditsNeeded ||
                        permissionCheckResult == PermissionCheckResult.YesButPlanUpgradeNeeded)
                    {
                        Session["BillingPlanOption"] = CurrentUserSession.BillingPlanOptions.CanBrowseGroups;
                        Response.Redirect("~/ManageProfile.aspx?sel=payment");
                        return;
                    }

                    if (permissionCheckResult == PermissionCheckResult.No)
                    {
                        StatusPageMessage = Lang.Trans("You are not allowed to browse groups!");
                        Response.Redirect("ShowStatus.aspx");
                        return;
                    }
                }          
                
                int groupID;
                if (Int32.TryParse((Request.Params["gid"]), out groupID))
                {
                    GroupID = groupID;
                    ViewGroupPhotos1.GroupID = groupID;
                    UploadGroupPhoto1.GroupID = groupID;
                    GroupMembers1.GroupID = groupID;
                    lnkOpenAjaxChat.NavigateUrl = String.Format("{0}/AjaxChat/ChatWindow.aspx?id={1}",
                                                                Config.Urls.Home, groupID);
                    lnkOpenAjaxChat.Target = String.Format("ajaxchat{0}", groupID);

                    if (!Group.Approved && (CurrentUserSession == null || (CurrentUserSession.Username != Group.Owner && !CurrentUserSession.IsAdmin())))
                    {
                        StatusPageMessage = Lang.Trans("This group is not approved yet.");

                        Response.Redirect("~/ShowStatus.aspx");
                        return;
                    }
                }
                else
                {
                    ViewGroupPhotos1.GroupID = -1;
                    UploadGroupPhoto1.GroupID = -1;
                    GroupMembers1.GroupID = -1;

                    return;
                }

                if (Group == null) return;

                if (!GroupMember.HasPermission(CurrentUserSession, CurrentGroupMember, Group, eGroupPermissionType.ViewGallery))
                {
                    StatusPageMessage = Lang.Trans("You are not authorized to view this gallery.");

                    Response.Redirect("~/ShowStatus.aspx");
                    return;
                }

                loadStrings();
                mvGroup.SetActiveView(viewGroupPhotos);
                lnkGroupGallery.Enabled = false;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            setMenu();

            #region Prepare CoolIris link

            if (Config.Misc.EnableCoolIris)
            {
                var rssNews = new HtmlLink();
                rssNews.Attributes.Add("rel", "alternate");
                rssNews.Attributes.Add("type", "application/rss+xml");
                rssNews.Attributes.Add("title", "");
                rssNews.Attributes.Add("id", "gallery");
                var feedUrl = String.Format("CoolIris.ashx?feed=groupphotos&groupid={0}",
                    GroupID);
                rssNews.Attributes.Add("href", feedUrl);
                Page.Header.Controls.Add(rssNews);
            }

            #endregion
        }

        private void setMenu()
        {
            Group group = Group;

            if (CurrentUserSession == null)
            {
                pnlGroupMembers.Visible = group.IsPermissionEnabled(eGroupPermissions.ViewMembersNonMembers);
                pnlMessageBoard.Visible = group.IsPermissionEnabled(eGroupPermissions.ViewMessageBoardNonMembers);
                pnlGroupEvents.Visible = group.IsPermissionEnabled(eGroupPermissions.ViewEventsNonMembers);
                pnlUploadPhoto.Visible = group.IsPermissionEnabled(eGroupPermissions.UploadPhotoNonMembers);
                pnlAjaxChat.Visible = false;
            }
            else if (CurrentGroupMember == null) // is not member for this group
            {
                pnlGroupMembers.Visible = group.IsPermissionEnabled(eGroupPermissions.ViewMembersNonMembers);
                pnlMessageBoard.Visible = group.IsPermissionEnabled(eGroupPermissions.ViewMessageBoardNonMembers);
                pnlGroupEvents.Visible = group.IsPermissionEnabled(eGroupPermissions.ViewEventsNonMembers);
                pnlUploadPhoto.Visible = group.IsPermissionEnabled(eGroupPermissions.UploadPhotoNonMembers);
                pnlAjaxChat.Visible = (Config.Groups.EnableAjaxChat &&
                                           (CurrentUserSession.CanUseChat() == PermissionCheckResult.Yes ||
                                           CurrentUserSession.CanUseChat() == PermissionCheckResult.YesWithCredits ||
                                           (CurrentUserSession.Level != null &&
                                            CurrentUserSession.Level.Restrictions.UserCanUseChat)) &&
                                      group.IsPermissionEnabled(eGroupPermissions.UseChatNonMembers));
            }
            else if (!CurrentGroupMember.Active)
            {
                pnlGroupMembers.Visible = group.IsPermissionEnabled(eGroupPermissions.ViewMembersNonMembers);
                pnlMessageBoard.Visible = group.IsPermissionEnabled(eGroupPermissions.ViewMessageBoardNonMembers);
                pnlGroupEvents.Visible = group.IsPermissionEnabled(eGroupPermissions.ViewEventsNonMembers);
                pnlUploadPhoto.Visible = group.IsPermissionEnabled(eGroupPermissions.UploadPhotoNonMembers);
                pnlAjaxChat.Visible = (Config.Groups.EnableAjaxChat &&
                                           (CurrentUserSession.CanUseChat() == PermissionCheckResult.Yes ||
                                           CurrentUserSession.CanUseChat() == PermissionCheckResult.YesWithCredits ||
                                           (CurrentUserSession.Level != null &&
                                            CurrentUserSession.Level.Restrictions.UserCanUseChat)) &&
                                      group.IsPermissionEnabled(eGroupPermissions.UseChatNonMembers));
            }
            else if (!Group.Approved) // can upload photos
            {
                pnlAjaxChat.Visible = false;
            }
            else if (CurrentUserSession.IsAdmin())
            {
                pnlAjaxChat.Visible = Config.Groups.EnableAjaxChat &&
                                          (CurrentUserSession.CanUseChat() == PermissionCheckResult.Yes ||
                                           CurrentUserSession.CanUseChat() == PermissionCheckResult.YesWithCredits ||
                                          CurrentUserSession.Level != null &&
                                          CurrentUserSession.Level.Restrictions.UserCanUseChat);
            }
            else // is member
            {
                if (CurrentGroupMember.Type == GroupMember.eType.Member)
                {
                    pnlGroupMembers.Visible = group.IsPermissionEnabled(eGroupPermissions.ViewMembersMembers);
                    pnlMessageBoard.Visible = group.IsPermissionEnabled(eGroupPermissions.ViewMessageBoardMembers);
                    pnlGroupEvents.Visible = group.IsPermissionEnabled(eGroupPermissions.ViewEventsMembers);
                    pnlAjaxChat.Visible = (Config.Groups.EnableAjaxChat &&
                                           (CurrentUserSession.CanUseChat() == PermissionCheckResult.Yes ||
                                           CurrentUserSession.CanUseChat() == PermissionCheckResult.YesWithCredits ||
                                           (CurrentUserSession.Level != null &&
                                            CurrentUserSession.Level.Restrictions.UserCanUseChat)) &&
                                          group.IsPermissionEnabled(eGroupPermissions.UseChatMembers));

                    pnlUploadPhoto.Visible = group.IsPermissionEnabled(eGroupPermissions.UploadPhotoMembers);
                }
                else if (CurrentGroupMember.Type == GroupMember.eType.VIP)
                {
                    pnlGroupMembers.Visible = group.IsPermissionEnabled(eGroupPermissions.ViewMembersVip);
                    pnlMessageBoard.Visible = group.IsPermissionEnabled(eGroupPermissions.ViewMessageBoardVip);
                    pnlGroupEvents.Visible = group.IsPermissionEnabled(eGroupPermissions.ViewEventsVip);
                    pnlAjaxChat.Visible = (Config.Groups.EnableAjaxChat &&
                                           (CurrentUserSession.CanUseChat() == PermissionCheckResult.Yes ||
                                           CurrentUserSession.CanUseChat() == PermissionCheckResult.YesWithCredits ||
                                           (CurrentUserSession.Level != null &&
                                            CurrentUserSession.Level.Restrictions.UserCanUseChat)) &&
                                          group.IsPermissionEnabled(eGroupPermissions.UseChatVip));

                    pnlUploadPhoto.Visible = group.IsPermissionEnabled(eGroupPermissions.UploadPhotoVip);
                }
                else if (CurrentGroupMember.Type == GroupMember.eType.Admin || CurrentGroupMember.Type == GroupMember.eType.Moderator)
                {
                    pnlAjaxChat.Visible = Config.Groups.EnableAjaxChat &&
                                          (CurrentUserSession.CanUseChat() == PermissionCheckResult.Yes ||
                                           CurrentUserSession.CanUseChat() == PermissionCheckResult.YesWithCredits ||
                                          (CurrentUserSession.Level != null &&
                                           CurrentUserSession.Level.Restrictions.UserCanUseChat));
                }
            }

            pnlNavLine.Visible = pnlUploadPhoto.Visible;
            ViewGroupPhotos1.BtnUploadPhotoVisible = pnlUploadPhoto.Visible;
        }

        private void loadStrings()
        {
            SmallBoxStart1.Title = Lang.Trans("Actions");
            lnkGroupHome.Text = Lang.Trans("Group Home");
            lnkGroupGallery.Text = Lang.Trans("Group Gallery");
            lnkGroupMembers.Text = Lang.Trans("Group Members");
            lnkMessageBoard.Text = Lang.Trans("Message Board");
            lnkGroupEvents.Text = Lang.Trans("Group Events");
            lnkOpenAjaxChat.Text = Lang.Trans("Start Group Chat");
            lnkUploadPhoto.Text = Lang.Trans("Upload Photo");
            lnkBrowseGroups.Text = Lang.Trans("Back to Groups");
        }

        private void enableMenuLinks()
        {
            lnkGroupHome.Enabled = true;
            lnkGroupGallery.Enabled = true;
            lnkGroupMembers.Enabled = true;
            lnkUploadPhoto.Enabled = true;
            lnkBrowseGroups.Enabled = true;
            lnkOpenAjaxChat.Enabled = true;
        }

        protected void lnkUploadPhoto_Click(object sender, EventArgs e)
        {
            mvGroup.SetActiveView(viewUploadPhoto);
            enableMenuLinks();
            lnkUploadPhoto.Enabled = false;
        }

        protected void lnkGroupGallery_Click(object sender, EventArgs e)
        {
            ViewGroupPhotos1.Results = null; // fetch all data from DB

            mvGroup.SetActiveView(viewGroupPhotos);
            enableMenuLinks();
            lnkGroupGallery.Enabled = false;
        }

        protected void lnkGroupHome_Click(object sender, EventArgs e)
        {
            Response.Redirect(UrlRewrite.CreateShowGroupUrl(GroupID.ToString()));
        }

        protected void lnkGroupMembers_Click(object sender, EventArgs e)
        {
            Response.Redirect(UrlRewrite.CreateShowGroupUrl(GroupID.ToString(), "gmembers"));
        }

        protected void lnkBrowseGroups_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Groups.aspx");
        }

        protected void lnkMyGroups_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Groups.aspx?show=mg");
        }

        protected void ViewGroupPhotos1_UploadPhotoClick(object sender, EventArgs e)
        {
            lnkUploadPhoto_Click(null, null);
        }

        protected void lnkMessageBoard_Click(object sender, EventArgs e)
        {
            Response.Redirect(UrlRewrite.CreateShowGroupTopicsUrl(GroupID.ToString()));
        }

        protected void lnkGroupEvents_Click(object sender, EventArgs e)
        {
            Response.Redirect(UrlRewrite.CreateShowGroupEventsUrl(GroupID.ToString()));
        }
    }
}
