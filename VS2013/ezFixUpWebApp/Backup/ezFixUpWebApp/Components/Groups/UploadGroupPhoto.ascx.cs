using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using ezFixUp.Classes;
using System.Drawing;

namespace ezFixUp.Components.Groups
{
    public partial class UploadGroupPhoto : System.Web.UI.UserControl
    {
        /// <summary>
        /// Gets or sets the group ID.
        /// </summary>
        /// <value>The group ID.</value>
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
                    throw new Exception("The group ID is not set!");
                }
            }
            set
            {
                ViewState["CurrentGroupId"] = value;
            }
        }

        private UserSession CurrentUserSession
        {
            get { return ((PageBase) Page).CurrentUserSession; }
        }

        /// <summary>
        /// Gets the current group member.
        /// </summary>
        /// <value>The current group member.</value>
        public GroupMember CurrentGroupMember
        {
            get
            {
                if (Page is ShowGroupPhotos)
                {
                    return ((ShowGroupPhotos)Page).CurrentGroupMember;
                }
                else if (CurrentUserSession != null)
                {
                    return GroupMember.Fetch(GroupID, CurrentUserSession.Username);
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadStrings();
            }
        }

        private void loadStrings()
        {
            LargeBoxStart1.Title = Lang.Trans("Upload Group Photo");
            btnUpload.Text = Lang.Trans("Upload");
        }

        /// <summary>
        /// Handles the Click event of the btnUpload control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (CurrentUserSession == null || (CurrentGroupMember == null && !CurrentUserSession.IsAdmin())) 
                return;

            string name = Config.Misc.EnableBadWordsFilterGroups ? Parsers.ProcessBadWords(txtName.Text.Trim()) : txtName.Text.Trim();
            string description = Config.Misc.EnableBadWordsFilterGroups ? Parsers.ProcessBadWords(txtDescription.Text.Trim()) : txtDescription.Text.Trim();

            if (name.Length == 0)
            {
                lblError.Text = Lang.Trans("Please enter name");
                return;
            }

            if (description.Length == 0)
            {
                lblError.Text = Lang.Trans("Please enter description");
                return;
            }

            if (fuGroupPhoto.PostedFile.FileName.Length > 0)
            {
                GroupPhoto groupPhoto = new GroupPhoto(GroupID, CurrentUserSession.Username);

                try
                {
                    groupPhoto.Image = System.Drawing.Image.FromStream(fuGroupPhoto.PostedFile.InputStream);    
                }
                catch
                {
                    lblError.Text = Lang.Trans("Invalid image!");
                    return;
                }

                groupPhoto.Name = name;
                groupPhoto.Description = description;

                groupPhoto.Save();

                string cacheFileDir = Config.Directories.ImagesCacheDirectory + "/" + groupPhoto.ID % 10;
                string cacheFileMask = String.Format("groupPhoto{0}_*.jpg", groupPhoto.ID);
                foreach (string file in Directory.GetFiles(cacheFileDir, cacheFileMask))
                {
                    File.Delete(file);
                }

                #region Add NewGroupPhoto Event

                Event newEvent = new Event(CurrentUserSession.Username);

                newEvent.FromGroup = GroupID;
                newEvent.Type = Event.eType.NewGroupPhoto;
                NewGroupPhoto newGroupPhoto = new NewGroupPhoto();
                newGroupPhoto.GroupPhotoID = groupPhoto.ID;
                newEvent.DetailsXML = Misc.ToXml(newGroupPhoto);

                newEvent.Save();

                Group group = Group.Fetch(groupPhoto.GroupID);
                string[] usernames = User.FetchMutuallyFriends(CurrentUserSession.Username);

                foreach (string friendUsername in usernames)
                {
                    if (Config.Users.NewEventNotification)
                    {
                        if (group != null)
                        {
                            string text =
                                    String.Format("Your friend {0} has uploaded a new photo in the {1} group".Translate(),
                                                  "<b>" + CurrentUserSession.Username + "</b>", Server.HtmlEncode(group.Name));
                            string thumbnailUrl = GroupImage.CreateImageUrl(groupPhoto.ID, 50, 50, true);
                            User.SendOnlineEventNotification(CurrentUserSession.Username, friendUsername, text,
                                                             thumbnailUrl,
                                                             UrlRewrite.CreateShowGroupPhotosUrl(group.ID.ToString()));
                        }
                    }
                }

                GroupMember[] groupMembers = GroupMember.Fetch(GroupID, true);

                foreach (GroupMember groupMember in groupMembers)
                {
                    if (groupMember.Username == CurrentUserSession.Username) continue;

                    if (Config.Users.NewEventNotification)
                    {
                        if (group != null)
                        {
                            string text =
                                        String.Format("There is a new photo in the {0} group".Translate(),
                                                      "<b>" + Parsers.ProcessGroupName(group.Name) + "</b>");
                            string thumbnailUrl = GroupImage.CreateImageUrl(groupPhoto.ID, 50, 50, true);
                            User.SendOnlineEventNotification(CurrentUserSession.Username, groupMember.Username, text,
                                                             thumbnailUrl,
                                                             UrlRewrite.CreateShowGroupPhotosUrl(group.ID.ToString()));
                        }
                    }
                }

                #endregion

                Response.Redirect(UrlRewrite.CreateShowGroupPhotosUrl(GroupID.ToString()));
            }
        }
    }
}