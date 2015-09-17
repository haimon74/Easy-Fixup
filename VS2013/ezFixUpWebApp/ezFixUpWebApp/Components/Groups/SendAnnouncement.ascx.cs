using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using ezFixUp.Classes;

namespace ezFixUp.Components.Groups
{
    public partial class SendAnnouncement : System.Web.UI.UserControl
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
                else
                {
                    return Group.Fetch(GroupID);
                }
            }
        }

        private UserSession CurrentUserSession
        {
            get { return ((PageBase) Page).CurrentUserSession; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            pnlSendAnnoucement.Visible = true;
            btnSendAnnouncement.Visible = true;

            if (!IsPostBack)
            {
                loadStrings();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            txtAnnouncement.Text = "";
            txtAgeFrom.Text = Config.Users.MinAge.ToString();
            txtAgeTo.Text = Config.Users.MaxAge.ToString();
        }

        private void loadStrings()
        {
            LargeBoxStart1.Title = Lang.Trans("Send Announcement");
            btnSendAnnouncement.Text = Lang.Trans("Send");

            ddGender.Items.Add(new ListItem(Lang.Trans("All"), "-1"));

            if (!Config.Users.DisableGenderInformation)
            {
                ddGender.Items.Add(new ListItem(Lang.Trans("All males"), ((int)User.eGender.Male).ToString()));
                ddGender.Items.Add(new ListItem(Lang.Trans("All females"), ((int)User.eGender.Female).ToString()));

                if (Config.Users.CouplesSupport)
                {
                    ddGender.Items.Add(
                        new ListItem(Lang.Trans("All couples"), ((int)User.eGender.Couple).ToString()));
                }
            }

            pnlGender.Visible = !Config.Users.DisableGenderInformation;
            pnlAge.Visible = !Config.Users.DisableAgeInformation;
        }

        protected void btnSendAnnouncement_Click(object sender, EventArgs e)
        {
            #region Validate input data

            if (txtAnnouncement.Text.Trim() == String.Empty)
            {
                lblError.CssClass = "error";
                lblError.Text = Lang.Trans("Please enter announcement!");
                return;
            }

            if (Config.Users.DisableAgeInformation)
            {
                txtAgeFrom.Text = Config.Users.MinAge.ToString();
                txtAgeTo.Text = Config.Users.MaxAge.ToString();
            }

            int ageFrom, ageTo;
            if (!Int32.TryParse(txtAgeFrom.Text, out ageFrom) || !Int32.TryParse(txtAgeTo.Text, out ageTo))
            {
                lblError.CssClass = "error";
                lblError.Text = Lang.Trans("Please specify valid age!");
                return;
            }

            if (ageFrom < Config.Users.MinAge || ageTo > Config.Users.MaxAge)
            {
                lblError.CssClass = "error";
                lblError.Text = Lang.Trans("Please specify valid age!");
                return;
            }

            if (ageFrom > ageTo)
            {
                lblError.CssClass = "error";
                lblError.Text = Lang.Trans("Please specify valid age!");
                return;
            }

            #endregion

            GroupMember[] groupMembers = GroupMember.Fetch(GroupID, true);
            List<string> lRecipients = new List<string>();

            foreach (GroupMember groupMember in groupMembers)
            {
                User user = null;

                try
                {
                    user = User.Load(groupMember.Username);
                }
                catch (NotFoundException)
                {
                    continue;
                }

                if (!user.Deleted && user.Age >= Convert.ToInt32(txtAgeFrom.Text) && user.Age <= Convert.ToInt32(txtAgeTo.Text))
                {
                    if (ddGender.SelectedValue == "-1")
                    {
                        lRecipients.Add(groupMember.Username);
                    }
                    else if (ddGender.SelectedValue == ((int)User.eGender.Male).ToString())
                    {
                        if (user.Gender == User.eGender.Male)
                        {
                            lRecipients.Add(groupMember.Username);
                        }
                    }
                    else if (ddGender.SelectedValue == ((int)User.eGender.Female).ToString())
                    {
                        if (user.Gender == User.eGender.Female)
                        {
                            lRecipients.Add(groupMember.Username);
                        }
                    }
                    else if (ddGender.SelectedValue == ((int)User.eGender.Couple).ToString())
                    {
                        if (user.Gender == User.eGender.Couple)
                        {
                            lRecipients.Add(groupMember.Username);
                        }
                    }
                }
            }

            foreach (string recipient in lRecipients)
            {
                Message msg = new Message(CurrentUserSession.Username, recipient);
                msg.Body = Config.Misc.EnableBadWordsFilterGroups ? Parsers.ProcessBadWords(txtAnnouncement.Text.Trim()) : txtAnnouncement.Text.Trim();
                msg.Send();
            }

            pnlSendAnnoucement.Visible = false;
            btnSendAnnouncement.Visible = false;
            lblError.CssClass = "message";
            lblError.Text = Lang.Trans("Your announcement has been sent!");
        }
    }
}