using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI.WebControls;
using ezFixUp.Classes;

namespace ezFixUp.Components.Groups
{
    public partial class EditGroup : System.Web.UI.UserControl
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
                return Group.Fetch(GroupID);
            }
        }

        protected GroupMember CurrentGroupMember
        {
            get
            {
                if (Page is ShowGroup)
                {
                    return ((ShowGroup)Page).CurrentGroupMember;
                }
                return GroupMember.Fetch(GroupID, ((PageBase)Page).CurrentUserSession.Username);
            }

            set
            {
                if (Page is ShowGroup)
                {
                    ((ShowGroup)Page).CurrentGroupMember = value;
                }
            }
        }

        protected UserSession CurrentUserSession
        {
            get { return ((PageBase) Page).CurrentUserSession; }
        }

        protected string CurrentUsername
        {
            get
            {
                if (((PageBase)Page).CurrentUserSession != null)
                {
                     return ((PageBase) Page).CurrentUserSession.Username;
                }
                return null;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (CurrentUserSession == null) return;

                loadStrings();
                mvEditGroup.SetActiveView(viewMain);
            }
        }

        private void loadStrings()
        {
            CascadingDropDownCity.LoadingText = "[Loading cities...]".Translate();
            LargeBoxStart1.Title = Lang.Trans("Manage Group");
            lblEditGroup.Text = Lang.Trans(" I want to change my group setting and/or information.");
            lblTransferOwnership.Text =
                Lang.Trans(
                    " I don't want to be the owner anymore and want to transfer the group ownership to someone else.");
            lblDeleteGroup.Text =
                    String.Format(Lang.Trans("Your group can only be deleted if it has up to {0} members. <br> Otherwise, please transfer the ownership to another member if you no longer wish to manage this group."),
                                    Config.Groups.MaxGroupMembersToDeleteGroup);
            lblTerms.Text = Lang.Trans("Terms & Conditions");
            btnEditGroup.Text = Lang.Trans("Edit Group");
            btnTransferOwnership.Text = Lang.Trans("Transfer Ownership");
            btnDeleteGroup.Text = Lang.Trans("Delete Group");
            btnUpdate.Text = Lang.Trans("Update");
            btnCancelUpdate.Text = Lang.Trans("Cancel");
            btnTransfer.Text = Lang.Trans("Transfer");
            btnCancelTransfer.Text = Lang.Trans("Cancel");
            btnDelete.Text = Lang.Trans("Delete");
            btnCancelDelete.Text = Lang.Trans("Cancel");
            lblName.Text = Lang.Trans("Name");
            lblDescription.Text = Lang.Trans("Description");
            lblCategories.Text = Lang.Trans("Categories");
            lblExplanation.Text = Lang.Trans("Hold Ctrl for multiple selection");
            lblGroupImage.Text = Lang.Trans("Group Image");
            lblAccessLevel.Text = Lang.Trans("Access Level");
            lblQuestion.Text = Lang.Trans("Enter a question to ask the user when joining the group");

            ddAgeRestriction.Items.Add(new ListItem(Lang.Trans("No restriction"), "-1"));
            for (int i = Config.Users.MinAge; i <= Config.Users.MaxAge; i++)
            {
                ddAgeRestriction.Items.Add(new ListItem(i.ToString()));
            }

            if (CurrentUserSession.Username == Config.Users.SystemUsername)
            {
                pnlAutomaticallyJoin.Visible = true;
                CascadingDropDownCountry.PromptText = "Any country".Translate();
                CascadingDropDownState.PromptText = "Any region".Translate();
                CascadingDropDownCity.PromptText = "Any city".Translate();
            }

            #region Set group managment actions

            // the logged in user is owner
            if (CurrentGroup != null && CurrentGroup.Owner == CurrentUsername)
            {
                lblEditGroup.Visible = true;
                lblDeleteGroup.Visible = true;
                lblTransferOwnership.Visible = true;
                btnEditGroup.Visible = true;
                btnDeleteGroup.Visible = true;
                btnTransferOwnership.Visible = true;
            }
            else if (CurrentGroupMember != null && (CurrentGroupMember.Type == GroupMember.eType.Admin) && !CurrentUserSession.IsAdmin())
            {
                lblEditGroup.Visible = true;
                btnEditGroup.Visible = true;

                divLineTransferOwnership.Visible = false;
                divSeparatorTransferOwnership.Visible = false;
            }
            else if ((CurrentUserSession != null && CurrentUserSession.IsAdmin()) && (CurrentGroup != null && CurrentGroup.Owner != CurrentUsername))
            {
                lblEditGroup.Visible = true;
                lblDeleteGroup.Visible = true;
                btnEditGroup.Visible = true;
                btnDeleteGroup.Visible = true;

                divLineTransferOwnership.Visible = false;
                divSeparatorTransferOwnership.Visible = false;
            }

            #endregion
        }

        private void loadGroupData()
        {
            #region Populate Controls

            txtName.Text = CurrentGroup.Name;
            txtDescription.Text = CurrentGroup.Description;
            txtTerms.Text = CurrentGroup.JoinTerms;
            txtQuestion.Text = CurrentGroup.JoinQuestion;

            ddAgeRestriction.SelectedValue = CurrentGroup.MinAge.HasValue ? CurrentGroup.MinAge.Value.ToString() : "-1";

            if (CurrentGroup.Autojoin)
            {
                cbAutomaticallyJoin.Checked = true;

                if (Config.Users.LocationPanelVisible)
                {
                    if (CurrentUserSession.Username == Config.Users.SystemUsername)
                    {
                        pnlCountry.Visible = true;
                        pnlState.Visible = true;
                        pnlCity.Visible = true;
                    }

                    if (CurrentGroup.AutojoinCountry != null)
                        CascadingDropDownCountry.SelectedValue = CurrentGroup.AutojoinCountry;
                    if (CurrentGroup.AutojoinRegion != null)
                        CascadingDropDownState.SelectedValue = CurrentGroup.AutojoinRegion;
                    if (CurrentGroup.AutojoinCity != null)
                        CascadingDropDownCity.SelectedValue = CurrentGroup.AutojoinCity;
                }
            }

            Category[] categories = Category.Fetch();
            Category[] categoriesByGroup = Category.FetchCategoriesByGroup(CurrentGroup.ID);

            lbCategories.Items.Clear();

            if (categories.Length > 0)
            {
                for (int i = 0; i < categories.Length; i++)
                {
                    bool isCategoryInSelectedCategories =
                        Array.Exists(categoriesByGroup, delegate(Category cat) { return cat.ID == categories[i].ID; });
                    
                    if (CurrentUserSession.IsAdmin() || categories[i].UsersCanCreateGroups || isCategoryInSelectedCategories)
                        lbCategories.Items.Add(new ListItem(categories[i].Name, categories[i].ID.ToString()));
                    
                    //foreach (Category categoryByGroup in categoriesByGroup)
                    //{
                    //    if (categoryByGroup.ID == categories[i].ID)
                    //    {
                    //        lbCategories.Items[i].Selected = true;
                    //    }
                    //}

                    if (isCategoryInSelectedCategories)
                        lbCategories.Items.FindByValue(categories[i].ID.ToString()).Selected = true;
                }
            }
            else
            {
                lblError.Text = Lang.Trans("There are no categories.");
                lbCategories.Visible = false;
            }

            ddAccessLevel.Items.Clear();
            ddAccessLevel.Items.Add(new ListItem(Lang.Trans("Public Group"), 
                ((int)Group.eAccessLevel.Public).ToString()));
            ddAccessLevel.Items.Add(new ListItem(Lang.Trans("Moderated Group"), 
                ((int)Group.eAccessLevel.Moderated).ToString()));
            ddAccessLevel.Items.Add(new ListItem(Lang.Trans("Private Group"), 
                ((int)Group.eAccessLevel.Private).ToString()));

            ddAccessLevel.SelectedValue = ((int)CurrentGroup.AccessLevel).ToString();

            Group currentGroup = CurrentGroup;

            cbViewGroupNonMembers.Checked = currentGroup.IsPermissionEnabled(eGroupPermissions.ViewGroupNonMembers);
            cbViewGroupMembers.Checked = currentGroup.IsPermissionEnabled(eGroupPermissions.ViewGroupMembers);
            cbViewGroupVip.Checked = currentGroup.IsPermissionEnabled(eGroupPermissions.ViewGroupVip);

            cbViewMessageBoardNonMembers.Checked = currentGroup.IsPermissionEnabled(eGroupPermissions.ViewMessageBoardNonMembers);
            cbViewMessageBoardMembers.Checked = currentGroup.IsPermissionEnabled(eGroupPermissions.ViewMessageBoardMembers);
            cbViewMessageBoardVip.Checked = currentGroup.IsPermissionEnabled(eGroupPermissions.ViewMessageBoardVip);

            cbViewGalleryNonMembers.Checked = currentGroup.IsPermissionEnabled(eGroupPermissions.ViewGalleryNonMembers);
            cbViewGalleryMembers.Checked = currentGroup.IsPermissionEnabled(eGroupPermissions.ViewGalleryMembers);
            cbViewGalleryVip.Checked = currentGroup.IsPermissionEnabled(eGroupPermissions.ViewGalleryVip);

            cbViewMembersNonMembers.Checked = currentGroup.IsPermissionEnabled(eGroupPermissions.ViewMembersNonMembers);
            cbViewMembersMembers.Checked = currentGroup.IsPermissionEnabled(eGroupPermissions.ViewMembersMembers);
            cbViewMembersVip.Checked = currentGroup.IsPermissionEnabled(eGroupPermissions.ViewMembersVip);

            cbViewEventsNonMembers.Checked = currentGroup.IsPermissionEnabled(eGroupPermissions.ViewEventsNonMembers);
            cbViewEventsMembers.Checked = currentGroup.IsPermissionEnabled(eGroupPermissions.ViewEventsMembers);
            cbViewEventsVip.Checked = currentGroup.IsPermissionEnabled(eGroupPermissions.ViewEventsVip);

            cbUploadPhotoNonMembers.Checked = currentGroup.IsPermissionEnabled(eGroupPermissions.UploadPhotoNonMembers);
            cbUploadPhotoMembers.Checked = currentGroup.IsPermissionEnabled(eGroupPermissions.UploadPhotoMembers);
            cbUploadPhotoVip.Checked = currentGroup.IsPermissionEnabled(eGroupPermissions.UploadPhotoVip);

            cbUseChatNonMembers.Checked = currentGroup.IsPermissionEnabled(eGroupPermissions.UseChatNonMembers);
            cbUseChatMembers.Checked = currentGroup.IsPermissionEnabled(eGroupPermissions.UseChatMembers);
            cbUseChatVip.Checked = currentGroup.IsPermissionEnabled(eGroupPermissions.UseChatVip);

            cbAddTopicNonMembers.Checked = currentGroup.IsPermissionEnabled(eGroupPermissions.AddTopicNonMembers);
            cbAddTopicMembers.Checked = currentGroup.IsPermissionEnabled(eGroupPermissions.AddTopicMembers);
            cbAddTopicVip.Checked = currentGroup.IsPermissionEnabled(eGroupPermissions.AddTopicVip);

            cbAddPostNonMembers.Checked = currentGroup.IsPermissionEnabled(eGroupPermissions.AddPostNonMembers);
            cbAddPostMembers.Checked = currentGroup.IsPermissionEnabled(eGroupPermissions.AddPostMembers);
            cbAddPostVip.Checked = currentGroup.IsPermissionEnabled(eGroupPermissions.AddPostVip);

            cbAddEventNonMembers.Checked = currentGroup.IsPermissionEnabled(eGroupPermissions.AddEventNonMembers);
            cbAddEventMembers.Checked = currentGroup.IsPermissionEnabled(eGroupPermissions.AddEventMembers);
            cbAddEventVip.Checked = currentGroup.IsPermissionEnabled(eGroupPermissions.AddEventVip);
            
            if (ddAccessLevel.SelectedValue == ((int)Group.eAccessLevel.Moderated).ToString())
                pnlQuestion.Visible = true;
            else
                pnlQuestion.Visible = false;
                
            #endregion
        }

        protected void btnEditGroup_Click(object sender, EventArgs e)
        {
            loadGroupData();
            mvEditGroup.SetActiveView(viewEditGroup);
        }

        protected void btnTransferOwnership_Click(object sender, EventArgs e)
        {
            bool isAdminMember = false;
            GroupMember[] groupAdmins = GroupMember.Fetch(GroupID, GroupMember.eType.Admin);
            
            ddGroupMembers.Items.Clear();

            foreach (GroupMember groupAdmin in groupAdmins)
            {
                if (groupAdmin.Username == "admin") isAdminMember = true;

                if (groupAdmin.Username == CurrentUsername)
                {
                    continue;
                }

                ddGroupMembers.Items.Add(new ListItem(groupAdmin.Username));
            }

            if (!isAdminMember) ddGroupMembers.Items.Add(new ListItem("admin"));

            mvEditGroup.SetActiveView(viewTransferOwnership);
        }

        protected void btnDeleteGroup_Click(object sender, EventArgs e)
        {
            mvEditGroup.SetActiveView(viewDeleteGroup);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (CurrentUsername == null) return;
            if (CurrentUsername != "admin")
            {
                if (CurrentGroupMember == null)
                {
                    Global.Logger.LogError("Unauthorized attempt to delete a group by member " +
                                           CurrentUsername + " with IP " + Request.UserHostAddress + " (not a member)");
                    return;
                }
                if (!CurrentGroupMember.Active || CurrentGroupMember.Type != GroupMember.eType.Admin)
                {
                    Global.Logger.LogError("Unauthorized attempt to delete a group by member " +
                                           CurrentUsername + " with IP " + Request.UserHostAddress +
                                           " (not active or admin)");
                    return;
                }
            }

            if (CurrentGroup.ActiveMembers <= Config.Groups.MaxGroupMembersToDeleteGroup)
            {
                Group.Delete(GroupID);

                ((PageBase)Page).StatusPageMessage = Lang.Trans("Your group has been deleted successfully!");

                Response.Redirect("ShowStatus.aspx");    
            }
            else
            {
                lblError.Text =
                    Lang.Trans(
                        String.Format("You cannot delete this group because it has more than {0} members.",
                                      Config.Groups.MaxGroupMembersToDeleteGroup));
            }

            mvEditGroup.SetActiveView(viewMain);
        }

        protected void btnCancelDelete_Click(object sender, EventArgs e)
        {
            mvEditGroup.SetActiveView(viewMain);
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            Group group = CurrentGroup;

            if (CurrentUsername == null) return;
            if (CurrentUsername != "admin")
            {
                if (CurrentGroupMember == null)
                {
                    Global.Logger.LogError("Unauthorized attempt to update a group by member " +
                                           CurrentUsername + " with IP " + Request.UserHostAddress + " (not a member)");
                    return;
                }
                if (!CurrentGroupMember.Active || CurrentGroupMember.Type != GroupMember.eType.Admin)
                {
                    Global.Logger.LogError("Unauthorized attempt to update a group by member " +
                                           CurrentUsername + " with IP " + Request.UserHostAddress +
                                           " (not active or admin)");
                    return;
                }
            }

            #region Set fields

            string name = Config.Misc.EnableBadWordsFilterGroups ? Parsers.ProcessBadWords(txtName.Text.Trim()) : txtName.Text.Trim();
            string description = Config.Misc.EnableBadWordsFilterGroups ? Parsers.ProcessBadWords(txtDescription.Text.Trim()) : txtDescription.Text.Trim();
            string terms = txtTerms.Text.Trim();
            string question = txtQuestion.Text.Trim();
            var accessLevel = (Group.eAccessLevel)Convert.ToInt32(ddAccessLevel.SelectedValue);

            #endregion

            #region Validate fields

            #region Validate Group name

            if (name.Length == 0)
            {
                lblError.Text = Lang.Trans("Please enter group name.");
                return;
            }

            if (group.Name != name && Group.IsNameUsed(name))
            {
                lblError.Text = Lang.Trans("Group name already exists.");
                return;
            }

            #endregion

            #region Validate Categories

            List<int> lCategoriesIDs = new List<int>();

            foreach (ListItem item in lbCategories.Items)
            {
                if (item.Selected)
                {
                    lCategoriesIDs.Add(Convert.ToInt32(item.Value));
                }
            }

            if (lCategoriesIDs.Count == 0)
            {
                lblError.Text = Lang.Trans("Please select category.");
                return;
            }

            #endregion

            #region Validate Description

            if (description.Length == 0)
            {
                lblError.Text = Lang.Trans("Please enter group description.");
                return;
            }
            #endregion

            #endregion

            group.JoinTerms = terms;
            group.Name = name;
            group.Description = description;
            group.AccessLevel = accessLevel;

            if (cbAutomaticallyJoin.Checked)
            {
                if (CurrentUserSession.Username == Config.Users.SystemUsername)
                {
                    group.Autojoin = true;
                    group.AutojoinCountry = dropCountry.SelectedValue.Trim() != String.Empty ? dropCountry.SelectedValue.Trim() : null;
                    group.AutojoinRegion = dropRegion.SelectedValue.Trim() != String.Empty ? dropRegion.SelectedValue.Trim() : null;
                    group.AutojoinCity = dropCity.SelectedValue.Trim() != String.Empty ? dropCity.SelectedValue.Trim() : null;
                }
            }
            else
            {
                group.Autojoin = false;
                group.AutojoinCountry = null;
                group.AutojoinRegion = null;
                group.AutojoinCity = null;
            }

            if (ddAgeRestriction.SelectedValue != "-1") group.MinAge = Convert.ToInt32(ddAgeRestriction.SelectedValue);
            else group.MinAge = null;

            ulong permissions = 0;

            if (cbViewGroupNonMembers.Checked) permissions |= (ulong) eGroupPermissions.ViewGroupNonMembers;
            if (cbViewGroupMembers.Checked) permissions |= (ulong) eGroupPermissions.ViewGroupMembers;
            if (cbViewGroupVip.Checked) permissions |= (ulong)eGroupPermissions.ViewGroupVip;
            if (cbViewMessageBoardNonMembers.Checked) permissions |= (ulong)eGroupPermissions.ViewMessageBoardNonMembers;
            if (cbViewMessageBoardMembers.Checked) permissions |= (ulong)eGroupPermissions.ViewMessageBoardMembers;
            if (cbViewMessageBoardVip.Checked) permissions |= (ulong)eGroupPermissions.ViewMessageBoardVip;
            if (cbViewGalleryNonMembers.Checked) permissions |= (ulong)eGroupPermissions.ViewGalleryNonMembers;
            if (cbViewGalleryMembers.Checked) permissions |= (ulong)eGroupPermissions.ViewGalleryMembers;
            if (cbViewGalleryVip.Checked) permissions |= (ulong) eGroupPermissions.ViewGalleryVip;
            if (cbViewMembersNonMembers.Checked) permissions |= (ulong)eGroupPermissions.ViewMembersNonMembers;
            if (cbViewMembersMembers.Checked) permissions |= (ulong)eGroupPermissions.ViewMembersMembers;
            if (cbViewMembersVip.Checked) permissions |= (ulong)eGroupPermissions.ViewMembersVip;
            if (cbViewEventsNonMembers.Checked) permissions |= (ulong)eGroupPermissions.ViewEventsNonMembers;
            if (cbViewEventsMembers.Checked) permissions |= (ulong)eGroupPermissions.ViewEventsMembers;
            if (cbViewEventsVip.Checked) permissions |= (ulong)eGroupPermissions.ViewEventsVip;
            if (cbUploadPhotoNonMembers.Checked) permissions |= (ulong)eGroupPermissions.UploadPhotoNonMembers;
            if (cbUploadPhotoMembers.Checked) permissions |= (ulong)eGroupPermissions.UploadPhotoMembers;
            if (cbUploadPhotoVip.Checked) permissions |= (ulong)eGroupPermissions.UploadPhotoVip;
            if (cbUseChatNonMembers.Checked) permissions |= (ulong)eGroupPermissions.UseChatNonMembers;
            if (cbUseChatMembers.Checked) permissions |= (ulong)eGroupPermissions.UseChatMembers;
            if (cbUseChatVip.Checked) permissions |= (ulong)eGroupPermissions.UseChatVip;
            if (cbAddTopicNonMembers.Checked) permissions |= (ulong)eGroupPermissions.AddTopicNonMembers;
            if (cbAddTopicMembers.Checked) permissions |= (ulong)eGroupPermissions.AddTopicMembers;
            if (cbAddTopicVip.Checked) permissions |= (ulong)eGroupPermissions.AddTopicVip;
            if (cbAddPostNonMembers.Checked) permissions |= (ulong)eGroupPermissions.AddPostNonMembers;
            if (cbAddPostMembers.Checked) permissions |= (ulong)eGroupPermissions.AddPostMembers;
            if (cbAddPostVip.Checked) permissions |= (ulong)eGroupPermissions.AddPostVip;
            if (cbAddEventNonMembers.Checked) permissions |= (ulong)eGroupPermissions.AddEventNonMembers;
            if (cbAddEventMembers.Checked) permissions |= (ulong)eGroupPermissions.AddEventMembers;
            if (cbAddEventVip.Checked) permissions |= (ulong)eGroupPermissions.AddEventVip;

            group.Permissions = permissions;
            if (accessLevel == Group.eAccessLevel.Moderated)
                group.JoinQuestion = question;            

            group.Save();
            group.SetCategories(lCategoriesIDs.ToArray());

            #region Upload Group Icon

            if (fuGroupImage.PostedFile.FileName.Length > 0)
            {
                System.Drawing.Image image = null;
                try
                {
                    image = System.Drawing.Image.FromStream
                        (fuGroupImage.PostedFile.InputStream);
                }
                catch
                {
                    lblError.Text = Lang.Trans("Invalid image!");
                    return;
                }

                Group.SaveIcon(group.ID, image);

                string cacheFileDir = Config.Directories.ImagesCacheDirectory + "/" + group.ID % 10;
                string cacheFileMask = String.Format("groupID{0}_*.jpg", group.ID);
                foreach (string file in Directory.GetFiles(cacheFileDir, cacheFileMask))
                {
                    File.Delete(file);
                }
            }

            #endregion

            mvEditGroup.SetActiveView(viewMain);

        }

        protected void btnCancelUpdate_Click(object sender, EventArgs e)
        {
            mvEditGroup.SetActiveView(viewMain);
        }

        protected void btnTransfer_Click(object sender, EventArgs e)
        {
            if (CurrentUsername == null) return;
            if (CurrentUsername != "admin")
            {
                if (CurrentGroupMember == null)
                {
                    Global.Logger.LogError("Unauthorized attempt to transfer a group by member " +
                                           CurrentUsername + " with IP " + Request.UserHostAddress + " (not a member)");
                    return;
                }
                if (!CurrentGroupMember.Active || CurrentGroupMember.Type != GroupMember.eType.Admin)
                {
                    Global.Logger.LogError("Unauthorized attempt to transfer a group by member " +
                                           CurrentUsername + " with IP " + Request.UserHostAddress +
                                           " (not active or admin)");
                    return;
                }
            }

            if (ddGroupMembers.SelectedValue == "admin" && !GroupMember.IsMember("admin", CurrentGroup.ID))
            {
                var groupMember = new GroupMember(CurrentGroup.ID, "admin")
                                      {
                                          Active = true,
                                          Type = GroupMember.eType.Admin
                                      };
                groupMember.Save();
                CurrentGroup.ActiveMembers++;
            }

            CurrentGroup.Owner = ddGroupMembers.SelectedValue;
            CurrentGroup.Save();

            MiscTemplates.TransferGroupOwnerMessage transferGroupOwnershipTemplate = null;
            User recipient = null;
            try
            {
                recipient = User.Load(ddGroupMembers.SelectedValue);
                transferGroupOwnershipTemplate = new MiscTemplates.TransferGroupOwnerMessage(recipient.LanguageId);
            }
            catch (NotFoundException)
            {
                transferGroupOwnershipTemplate = new MiscTemplates.TransferGroupOwnerMessage(PageBase.GetLanguageId());
            }
            var msg = new Message(((PageBase) Page).CurrentUserSession.Username, 
                ddGroupMembers.SelectedValue);
            string message = transferGroupOwnershipTemplate.Message;
            message = message.Replace("%%GROUP%%", Parsers.ProcessGroupName(CurrentGroup.Name));
            msg.Body = message;
            msg.Send();

            lblError.Text = Lang.Trans("The ownership has been transferred successfully.");

            mvEditGroup.SetActiveView(viewMain);
        }

        protected void btnCancelTransfer_Click(object sender, EventArgs e)
        {
            mvEditGroup.SetActiveView(viewMain);
        }

        public void ShowMessage(Misc.MessageType type, string message)
        {
            switch (type)
            {
                case Misc.MessageType.Error:
                    lblError.CssClass = "error";
                    break;
                case Misc.MessageType.Success:
                    lblError.CssClass = "message";
                    break;
            }
            lblError.Text = message;
        }

        protected void ddAccessLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            mvEditGroup.SetActiveView(viewEditGroup);

            pnlQuestion.Visible = ddAccessLevel.SelectedValue == ((int)Group.eAccessLevel.Moderated).ToString();
        }

        protected void cbAutomaticallyJoin_CheckedChanged(object sender, EventArgs e)
        {
            if (Config.Users.LocationPanelVisible)
            {
                pnlCountry.Visible = cbAutomaticallyJoin.Checked;
                pnlState.Visible = cbAutomaticallyJoin.Checked;
                pnlCity.Visible = cbAutomaticallyJoin.Checked;
            }
        }
    }
}