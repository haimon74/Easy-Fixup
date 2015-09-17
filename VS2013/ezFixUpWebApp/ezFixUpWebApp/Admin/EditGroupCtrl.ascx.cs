using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Drawing;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using ezFixUp.Classes;

namespace ezFixUp.Admin
{
    public partial class EditGroupCtrl : System.Web.UI.UserControl
    {
        public int GroupID
        {
            get { return (int) ViewState["CurrentGroupID"]; }
            set { ViewState["CurrentGroupID"] = value; }
        }

        public Group Group
        {
            get
            {
                if (Page is EditGroup)
                {
                    return ((EditGroup) Page).Group;
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
                loadGroupData();
            }
        }

        private void loadStrings()
        {
            btnSave.Text = Lang.TransA(" Save Changes >> ");
            if (!((AdminPageBase)Page).HasWriteAccess) btnSave.Enabled = false;
            btnCancel.Text = Lang.TransA("Cancel");

            #region load Age restrictions

            ddAgeRestriction.Items.Add(new ListItem(Lang.TransA("No restriction"), "-1"));
            for (int i = Config.Users.MinAge; i <= Config.Users.MaxAge; i++)
            {
                ddAgeRestriction.Items.Add(new ListItem(i.ToString()));
            }

            #endregion
        }

        private void loadGroupData()
        {
            #region Populate Controls

            txtName.Text = Group.Name;
            txtDescription.Text = Group.Description;

            Category[] categories = Category.Fetch();
            Category[] categoriesByGroup = Category.FetchCategoriesByGroup(Group.ID);

            if (categories.Length > 0)
            {
                for (int i = 0; i < categories.Length; i++)
                {
                    lbCategories.Items.Add(new ListItem(categories[i].Name, categories[i].ID.ToString()));

                    foreach (Category categoryByGroup in categoriesByGroup)
                    {
                        if (categoryByGroup.ID == categories[i].ID)
                        {
                            lbCategories.Items[i].Selected = true;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show(Lang.TransA("There are no categories."), Misc.MessageType.Error);
                lbCategories.Visible = false;
            }

            GroupMember[] groupAdmins = GroupMember.Fetch(GroupID, GroupMember.eType.Admin);

            if (groupAdmins.Length > 0)
            {
                foreach (GroupMember groupMember in groupAdmins)
                {
                    ddOwner.Items.Add(new ListItem(groupMember.Username));        
                }
            }
            else
            {
                MessageBox.Show(Lang.TransA("There are no administrators for this group!"), Misc.MessageType.Error);
            }

            ddOwner.SelectedValue = Group.Owner;

            ddAccessLevel.Items.Add(new ListItem(Lang.TransA("Public Group"), ((int)Group.eAccessLevel.Public).ToString()));
            ddAccessLevel.Items.Add(new ListItem(Lang.TransA("Moderated Group"), ((int)Group.eAccessLevel.Moderated).ToString()));
            ddAccessLevel.Items.Add(new ListItem(Lang.TransA("Private Group"), ((int)Group.eAccessLevel.Private).ToString()));

            ddAccessLevel.SelectedValue = ((int) Group.AccessLevel).ToString();

            ddApproved.Items.Add(new ListItem(Lang.TransA("Yes")));
            ddApproved.Items.Add(new ListItem(Lang.TransA("No")));

            ddApproved.SelectedIndex = Group.Approved ? 0 : 1;

            ddAgeRestriction.SelectedValue = Group.MinAge.HasValue ? Group.MinAge.Value.ToString() : "-1";

            #endregion
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!((AdminPageBase)Page).HasWriteAccess) return;

            #region Set fields

            string name = Config.Misc.EnableBadWordsFilterGroups ? Parsers.ProcessBadWords(txtName.Text.Trim()) : txtName.Text.Trim();
            string description = Config.Misc.EnableBadWordsFilterGroups ? Parsers.ProcessBadWords(txtDescription.Text.Trim()) : txtDescription.Text.Trim();
            Group.eAccessLevel accessLevel = (Group.eAccessLevel)Convert.ToInt32(ddAccessLevel.SelectedValue);
            string owner = ddOwner.SelectedValue;
            bool approved = ddApproved.SelectedIndex == 0 ? true : false;

            #endregion

            #region Validate fields

            #region Validate Group name

            if (Group.Name != txtName.Text)
            {
                if (Group.IsNameUsed(name))
                {
                    MessageBox.Show(Lang.TransA("Group name already exists."), Misc.MessageType.Error);
                    return;
                }
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
                MessageBox.Show(Lang.TransA("Please select category."), Misc.MessageType.Error);
                return;
            }

            #endregion

            #region Validate Description

            if (description.Length == 0)
            {
                MessageBox.Show(Lang.TransA("Please enter group description."), Misc.MessageType.Error);
                return;
            }
            #endregion

            #region Validate Group Icon

            if (fuGroupImage.HasFile)
            {
                System.Drawing.Image image = null;
                try
                {
                    image = System.Drawing.Image.FromStream
                        (fuGroupImage.PostedFile.InputStream);
                }
                catch
                {
                    MessageBox.Show(Lang.TransA("Invalid image!"), Misc.MessageType.Error);
                    return;
                }

                Group.SaveIcon(Group.ID, image);
            }

            #endregion

            #endregion

            Group.Name = name;
            Group.Description = description;
            Group.AccessLevel = accessLevel;

            if (Group.Owner != owner) // if the admin changes group owner
            {
                try
                {
                    User user = User.Load(owner);
                    MiscTemplates.TransferGroupOwnerMessage transferGroupOwnershipTemplate =
                        new MiscTemplates.TransferGroupOwnerMessage(user.LanguageId);
                    Message msg = new Message(Config.Users.SystemUsername, owner);
                    string message = transferGroupOwnershipTemplate.Message;
                    message = message.Replace("%%GROUP%%", Parsers.ProcessGroupName(Group.Name));
                    msg.Body = message;
                    msg.Send();
                }
                catch (NotFoundException)
                {
                }
            }
            
            Group.Owner = owner;
            Group.Approved = approved;
            if (ddAgeRestriction.SelectedValue != "-1") Group.MinAge = Convert.ToInt32(ddAgeRestriction.SelectedValue);
            else Group.MinAge = null;

            Group.Save();
            Group.SetCategories(lCategoriesIDs.ToArray());

            MessageBox.Show(Lang.TransA("The group has been successfully updated!"), Misc.MessageType.Success);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Admin/BrowseGroups.aspx");
        }
    }
}