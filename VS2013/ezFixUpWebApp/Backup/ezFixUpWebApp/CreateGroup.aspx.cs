using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using ezFixUp.Classes;
using Image=System.Drawing.Image;

namespace ezFixUp
{
    public partial class CreateGroup : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            cvCreateGroupRequirement.Text = "Create Group Requirements".Translate();
            if (!IsPostBack)
            {
                var browsePermissionCheckResult = CurrentUserSession.CanBrowseGroups();
                var createPermissionCheckResult = CurrentUserSession.CanCreateGroups();

                if ((browsePermissionCheckResult == PermissionCheckResult.Yes &&
                    createPermissionCheckResult == PermissionCheckResult.Yes) ||
                    (CurrentUserSession.Level != null &&
                                       CurrentUserSession.Level.Restrictions.CanCreateGroups))
                {
                }
                else if (browsePermissionCheckResult == PermissionCheckResult.YesButMoreCreditsNeeded ||
                         browsePermissionCheckResult == PermissionCheckResult.YesButPlanUpgradeNeeded ||
                         createPermissionCheckResult == PermissionCheckResult.YesButPlanUpgradeNeeded ||
                         createPermissionCheckResult == PermissionCheckResult.YesButMoreCreditsNeeded)
                {
                    Global.GetSessionState()["BillingPlanOption"] = CurrentUserSession.BillingPlanOptions.CanCreateGroups;
                    Response.Redirect("~/ManageProfile.aspx?sel=payment");
                    return;
                }
                else if (browsePermissionCheckResult == PermissionCheckResult.No || createPermissionCheckResult == PermissionCheckResult.No)
                {
                    StatusPageMessage = Lang.Trans("You are not allowed to create groups!");
                    Response.Redirect("ShowStatus.aspx");
                    return;
                }

                //bool canCreateGroup = CurrentUserSession.BillingPlanOptions.CanCreateGroups
                //                      ||
                //                      (CurrentUserSession.Level != null &&
                //                       CurrentUserSession.Level.Restrictions.CanCreateGroups);
                //if (!CurrentUserSession.BillingPlanOptions.CanBrowseGroups || !canCreateGroup)
                //{
                //    if (Config.Users.PaymentRequired && !CurrentUserSession.Paid)
                //        Response.Redirect("~/ManageProfile.aspx?sel=payment");
                //    else
                //    {
                //        StatusPageMessage = Lang.Trans("You are not allowed to create groups!");
                //        Response.Redirect("ShowStatus.aspx");
                //    }
                //}

                loadStrings();
                populateControls();
            }
        }

        private void loadStrings()
        {
            CascadingDropDownCity.LoadingText = "[Loading cities...]".Translate();
            lnkBrowseGroups.Text = Lang.Trans("Back to Groups");
            SmallBoxStart1.Title = Lang.Trans("Actions");
            LargeBoxStart1.Title = Lang.Trans("Create Group");
            btnCreateGroup.Text = Lang.Trans("Create Group");
            btnCancel.Text = Lang.Trans("Cancel");

            if (CurrentUserSession.Username == Config.Users.SystemUsername)
            {
                pnlAutomaticallyJoin.Visible = true;
                CascadingDropDownCountry.PromptText = "Any country".Translate();
                CascadingDropDownState.PromptText = "Any region".Translate();
                CascadingDropDownCity.PromptText = "Any city".Translate();
            }
        }

        private void populateControls()
        {
            #region load Categories

            Category[] categories = Category.Fetch();

            bool userCanCreateGroups =
                Array.Exists(categories, delegate(Category match) { return match.UsersCanCreateGroups; });
            if (CurrentUserSession == null || (!CurrentUserSession.IsAdmin() && !userCanCreateGroups))
            {
                StatusPageMessage = Lang.Trans("You are not allowed to create groups!");
                Response.Redirect("ShowStatus.aspx");
            }

            if (categories.Length > 0)
            {
                foreach (Category category in categories)
                {
                    if (CurrentUserSession.IsAdmin() || category.UsersCanCreateGroups)
                        lbCategories.Items.Add(new ListItem(category.Name, category.ID.ToString()));
                }

                if (Request.Params["cid"] != null && Request.Params["cid"] != "")
                {
                    lbCategories.SelectedValue = Request.Params["cid"];
                }
            }
            else
            {
                lblError.Text = Lang.Trans("There are no categories.");
            }

            #endregion

            #region load Access Levels

            ddAccessLevel.Items.Add(
                new ListItem(Lang.Trans("Public Group"), ((int) Group.eAccessLevel.Public).ToString()));
            ddAccessLevel.Items.Add(
                new ListItem(Lang.Trans("Moderated Group"), ((int) Group.eAccessLevel.Moderated).ToString()));
            ddAccessLevel.Items.Add(
                new ListItem(Lang.Trans("Private Group"), ((int) Group.eAccessLevel.Private).ToString()));

            #endregion

            #region load Age restrictions

            ddAgeRestriction.Items.Add(new ListItem(Lang.Trans("No restriction"), "-1"));
            for (int i = Config.Users.MinAge; i <= Config.Users.MaxAge; i++)
            {
                ddAgeRestriction.Items.Add(new ListItem(i.ToString()));
            }

            #endregion
        }

        protected void btnCreateGroup_Click(object sender, EventArgs e)
        {
            #region Check groups per member limit

            int memberOf = GroupMember.Fetch(CurrentUserSession.Username).Length;
            int maxGroupsPermitted = 0;// Config.Groups.MaxGroupsPerMember;
            if (CurrentUserSession.BillingPlanOptions.MaxGroupsPerMember.Value > maxGroupsPermitted)
                maxGroupsPermitted = CurrentUserSession.BillingPlanOptions.MaxGroupsPerMember.Value;
            if (CurrentUserSession.Level != null && CurrentUserSession.Level.Restrictions.MaxGroupsPerMember > maxGroupsPermitted)
                maxGroupsPermitted = CurrentUserSession.Level.Restrictions.MaxGroupsPerMember;

            if (memberOf >= maxGroupsPermitted && !CurrentUserSession.IsAdmin())
            {
                lblError.Text = String.Format(Lang.Trans("You are already a member of {0} groups. Please leave one of them first."), maxGroupsPermitted);
                return;
            }

            #endregion

            #region Set fields

            string terms = Config.Misc.EnableBadWordsFilterGroups
                               ? Parsers.ProcessBadWords(txtTerms.Text.Trim())
                               : txtTerms.Text.Trim();
            string name = Config.Misc.EnableBadWordsFilterGroups
                              ? Parsers.ProcessBadWords(txtName.Text.Trim())
                              : txtName.Text.Trim();
            string description = Config.Misc.EnableBadWordsFilterGroups
                                     ? Parsers.ProcessBadWords(txtDescription.Text.Trim())
                                     : txtDescription.Text.Trim();
            string question = Config.Misc.EnableBadWordsFilterGroups
                                  ? Parsers.ProcessBadWords(txtQuestion.Text.Trim())
                                  : txtQuestion.Text.Trim();
            Group.eAccessLevel accessLevel = (Group.eAccessLevel) Convert.ToInt32(ddAccessLevel.SelectedValue);
            string owner = CurrentUserSession.Username;

            #endregion

            #region Validate fields

            #region Validate Group name

            if (name.Length == 0)
            {
                lblError.Text = Lang.Trans("Please enter group name.");
                return;
            }

            if (Group.IsNameUsed(name))
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

            #region Validate Group Icon

            Image image = null;

            if (fuGroupImage.PostedFile.FileName.Length == 0)
            {
                image = Image.FromFile(Server.MapPath("~/Images") + "/defaultgroupicon.jpg");
            }
            else
            {
                try
                {
                    image = Image.FromStream
                        (fuGroupImage.PostedFile.InputStream);
                }
                catch
                {
                    lblError.Text = Lang.Trans("Invalid image!");
                    return;
                }
            }

            #endregion

            #endregion

            if (Group.IsNameUsed(name))
            {
                StatusPageMessage = Lang.Trans("Group with such name already exists!");

                Response.Redirect("~/ShowStatus.aspx");                
            }

            Group group = new Group();
            group.JoinTerms = terms;
            group.Name = name;
            group.Description = description;
            if (ddAgeRestriction.SelectedValue != "-1") group.MinAge = Convert.ToInt32(ddAgeRestriction.SelectedValue);

            if (CurrentUserSession != null && CurrentUserSession.IsAdmin() || Config.Groups.AutoApproveGroups)
            {
                group.Approved = true;
            }

            if (cbAutomaticallyJoin.Checked)
            {
                group.Autojoin = true;
                group.AutojoinCountry = dropCountry.SelectedValue.Trim() != String.Empty ? dropCountry.SelectedValue.Trim() : null;
                group.AutojoinRegion = dropRegion.SelectedValue.Trim() != String.Empty ? dropRegion.SelectedValue.Trim() : null;
                group.AutojoinCity = dropCity.SelectedValue.Trim() != String.Empty ? dropCity.SelectedValue.Trim() : null;
            }
            
            group.AccessLevel = accessLevel;
            group.Owner = owner;
            if (accessLevel == Group.eAccessLevel.Moderated)
                group.JoinQuestion = question;
            group.ActiveMembers++;
            group.Save();
            group.SetCategories(lCategoriesIDs.ToArray());

            Group.SaveIcon(group.ID, image);

            GroupMember groupMember = new GroupMember(group.ID, CurrentUserSession.Username);
            groupMember.Active = true;
            groupMember.Type = GroupMember.eType.Admin;
            groupMember.Save();

            StatusPageMessage = Lang.Trans("Your group has been created successfully!");

            Response.Redirect("~/ShowStatus.aspx");
        }

        protected void lnkBrowseGroups_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Groups.aspx");
        }

        protected void lnkMyGroups_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Groups.aspx?show=mg");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Groups.aspx");
        }

        protected void lnkNewGroups_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Groups.aspx?show=ng");
        }

        protected void lnkPendingInvitations_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Groups.aspx?show=pi");
        }

        protected void ddAccessLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddAccessLevel.SelectedValue == ((int) Group.eAccessLevel.Moderated).ToString())
                pnlQuestion.Visible = true;
            else
                pnlQuestion.Visible = false;
        }

        protected void cbAutomaticallyJoin_CheckedChanged(object sender, EventArgs e)
        {
            if (Config.Users.LocationPanelVisible)
            {
                trCountry.Visible = cbAutomaticallyJoin.Checked;
                trState.Visible = cbAutomaticallyJoin.Checked;
                trCity.Visible = cbAutomaticallyJoin.Checked;
            }
        }
    }
}