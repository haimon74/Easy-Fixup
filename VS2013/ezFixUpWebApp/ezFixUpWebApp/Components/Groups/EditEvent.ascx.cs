using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using ezFixUp.Classes;

namespace ezFixUp.Components.Groups
{
    public partial class EditEvent : System.Web.UI.UserControl
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

        public int EventID
        {
            get
            {
                if (ViewState["CurrentEventID"] != null)
                {
                    return (int)ViewState["CurrentEventID"];
                }
                else
                {
                    throw new Exception("The event id is not set!");
                }
            }
            set
            {
                ViewState["CurrentEventID"] = value;
            }
        }

        public DateTime? EventDate
        {
            get { return (DateTime?) ViewState["EventDate"]; }
            set { ViewState["EventDate"] = value; }
        }

        private UserSession CurrentUserSession
        {
            get { return ((PageBase)Page).CurrentUserSession; }
        }

        public enum eType
        {
            AddEvent,
            EditEvent
        }

        private eType type;
        public eType Type
        {
            get { return type; }
            set { type = value; }
        }

        private bool loadGroupEvent = false;
        public bool LoadGroupEvent
        {
            get { return loadGroupEvent; }
            set { loadGroupEvent = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadStrings();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            setControl();

            if (LoadGroupEvent)
            {
                populateGroupEvent();
            }
        }

        private void loadStrings()
        {
            btnAdd.Text = Lang.Trans("Add");
            btnUpdate.Text = Lang.Trans("Update");
            datePicker1.MinYear = DateTime.Now.Year;
            datePicker1.MaxYear = DateTime.Now.Year + 5;
        }

        private void setControl()
        {
            switch (Type)
            {
                case eType.AddEvent :
                    LargeBoxStart1.Title = Lang.Trans("Add event");
                    pnlEventImage.Visible = false;
                    btnAdd.Visible = true;
                    btnUpdate.Visible = false;
                    datePicker1.SelectedDate = EventDate ?? DateTime.Now;
                    break;
                case eType.EditEvent :
                    pnlEventImage.Visible = true;
                    LargeBoxStart1.Title = Lang.Trans("Edit event");
                    btnAdd.Visible = false;
                    btnUpdate.Visible = true;
                    break;
            }
        }

        private void populateGroupEvent()
        {
            GroupEvent groupEvent = GroupEvent.Fetch(EventID);

            if (groupEvent != null)
            {
                txtTitle.Text = groupEvent.Title;
                txtDescription.Text = groupEvent.Description;
                datePicker1.SelectedDate = groupEvent.Date;
                txtHoursMin.Text = groupEvent.Date.ToShortTimeString();
                txtLocation.Text = groupEvent.Location;
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (CurrentUserSession != null)
            {
                #region validate fields

                string title = txtTitle.Text.Trim();
                string description = txtDescription.Text.Trim();
                DateTime date = DateTime.MinValue;
                string location = txtLocation.Text.Trim();

                if (title.Length == 0)
                {
                    lblError.Text = Lang.Trans("Please enter event name.");
                    return;
                }

                if (description.Length == 0)
                {
                    lblError.Text = Lang.Trans("Please enter description.");
                    return;
                }

                if (!datePicker1.ValidDateEntered)
                {
                    lblError.Text = Lang.Trans("Please select valid date!");
                    return;
                }

                if (txtHoursMin.Text.Trim().Length > 0)
                {
                    date =
                        datePicker1.SelectedDate.AddHours(DateTime.Parse(txtHoursMin.Text.Trim()).Hour).AddMinutes(
                            DateTime.Parse(txtHoursMin.Text.Trim()).Minute);
                }

                #endregion

                GroupEvent groupEvent = new GroupEvent(GroupID, CurrentUserSession.Username);
                groupEvent.Title = Server.HtmlEncode(title);
                groupEvent.Description = Server.HtmlEncode(description);
                groupEvent.Date = date == DateTime.MinValue ? datePicker1.SelectedDate : date;
                groupEvent.Location = location;

                groupEvent.Save();

                if (fuImage.PostedFile.FileName.Length > 0)
                {
                    Image image = null;
                    try
                    {
                        image = Image.FromStream(fuImage.PostedFile.InputStream);
                    }
                    catch
                    {
                        lblError.Text = Lang.Trans("Invalid image!");
                        return;
                    }

                    GroupEvent.SaveImage(groupEvent.ID.Value, image);

                    string cacheFileDir = Config.Directories.ImagesCacheDirectory + "/" + groupEvent.ID % 10;
                    string cacheFileMask = String.Format("groupEventID{0}_*.jpg", groupEvent.ID);
                    foreach (string file in Directory.GetFiles(cacheFileDir, cacheFileMask))
                    {
                        File.Delete(file);
                    }
                }

                #region Add NewGroupEvent Event

                Event newEvent = new Event(GroupID);

                newEvent.Type = Event.eType.NewGroupEvent;
                NewGroupEvent newGroupEvent = new NewGroupEvent();
                newGroupEvent.GroupEventID = groupEvent.ID.Value;
                newEvent.DetailsXML = Misc.ToXml(newGroupEvent);

                newEvent.Save();

                Group group = Group.Fetch(GroupID);
                GroupMember[] groupMembers = GroupMember.Fetch(GroupID, true);

                foreach (GroupMember groupMember in groupMembers)
                {
                    if (groupMember.Username == CurrentUserSession.Username) continue;

                    if (Config.Users.NewEventNotification)
                    {
                        if (group != null)
                        {
                            string text = String.Format("There is a new event {0} in the {1} group".Translate(),
                                                        "<b>" + Server.HtmlEncode(groupEvent.Title) + "</b>",
                                                        Parsers.ProcessGroupName(group.Name));
                            string thumbnailUrl = GroupIcon.CreateImageUrl(group.ID, 50, 50, true);
                            User.SendOnlineEventNotification(CurrentUserSession.Username, groupMember.Username, text,
                                                             thumbnailUrl,
                                                             UrlRewrite.CreateShowGroupEventsUrl(group.ID.ToString()));
                        }
                    }
                }

                #endregion

                Response.Redirect(UrlRewrite.CreateShowGroupEventsUrl(GroupID.ToString()));
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (CurrentUserSession != null)
            {

                #region validate fields

                string title = txtTitle.Text.Trim();
                string description = txtDescription.Text.Trim();
                DateTime date = DateTime.MinValue;
                string location = txtLocation.Text.Trim();

                if (title.Length == 0)
                {
                    lblError.Text = Lang.Trans("Please enter event name.");
                    return;
                }

                if (description.Length == 0)
                {
                    lblError.Text = Lang.Trans("Please enter description.");
                    return;
                }

                if (!datePicker1.ValidDateEntered)
                {
                    lblError.Text = Lang.Trans("Please select valid date!");
                    return;
                }

                if (txtHoursMin.Text.Trim().Length > 0)
                {
                    date =
                        datePicker1.SelectedDate.AddHours(DateTime.Parse(txtHoursMin.Text.Trim()).Hour).AddMinutes(
                            DateTime.Parse(txtHoursMin.Text.Trim()).Minute);
                }

                #endregion

                GroupEvent groupEvent = GroupEvent.Fetch(EventID);

                if (groupEvent != null)
                {
                    groupEvent.Title = title;
                    groupEvent.Description = description;
                    groupEvent.Date = date == DateTime.MinValue ? datePicker1.SelectedDate : date;
                    groupEvent.Location = location;

                    groupEvent.Save();

                    if (fuImage.PostedFile.FileName.Length > 0)
                    {
                        Image image = null;
                        try
                        {
                            image = Image.FromStream(fuImage.PostedFile.InputStream);
                        }
                        catch
                        {
                            lblError.Text = Lang.Trans("Invalid image!");
                            return;
                        }

                        GroupEvent.SaveImage(groupEvent.ID.Value, image);

                        string cacheFileDir = Config.Directories.ImagesCacheDirectory + "/" + groupEvent.ID % 10;
                        string cacheFileMask = String.Format("groupEventID{0}_*.jpg", groupEvent.ID);
                        foreach (string file in Directory.GetFiles(cacheFileDir, cacheFileMask))
                        {
                            File.Delete(file);
                        }
                    }

                    Response.Redirect(UrlRewrite.CreateShowGroupEventsUrl(GroupID.ToString()));
                }
            }
        }
    }
}